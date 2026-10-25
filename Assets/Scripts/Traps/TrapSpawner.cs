
using UnityEngine;

public class TrapSpawner : MonoBehaviour
{
    [Header("Random Trap Prefabs")]
    public GameObject[] trapPrefabs;

    [Header("Scene Ingredient")]
    public GameObject sceneIngredient;

    [Header("Spawn Position")]
    [Tooltip("Place the SpawnPoint at ground level.")]
    public bool alignToGround = true;

    private void Awake()
    {
        if (sceneIngredient != null)
        {
            sceneIngredient.SetActive(false);
        }
    }

    private void Start()
    {
        if (trapPrefabs == null || trapPrefabs.Length == 0)
        {
            Debug.LogWarning(
                $"{gameObject.name}: No trap prefabs assigned!"
            );
            return;
        }

        int randomIndex = Random.Range(0, trapPrefabs.Length);
        GameObject selectedPrefab = trapPrefabs[randomIndex];

        if (selectedPrefab == null)
        {
            Debug.LogWarning(
                $"{gameObject.name}: Selected prefab is null!"
            );
            return;
        }

        GameObject spawnedTrap = Instantiate(
            selectedPrefab,
            transform.position,
            transform.rotation
        );

        Trap trapComponent = spawnedTrap.GetComponent<Trap>();

        if (trapComponent == null)
        {
            Debug.LogError(
                $"{spawnedTrap.name} is missing a Trap component!"
            );
            Destroy(spawnedTrap);
            return;
        }

        // Position the bottom of the collider at the spawn point.
        if (alignToGround)
        {
            Collider2D trapCollider =
                spawnedTrap.GetComponent<Collider2D>();

            if (trapCollider != null)
            {
                float bottomY = trapCollider.bounds.min.y;
                float difference = transform.position.y - bottomY;

                spawnedTrap.transform.position +=
                    new Vector3(0f, difference, 0f);
            }
            else
            {
                Debug.LogWarning(
                    $"{spawnedTrap.name} has no Collider2D!"
                );
            }
        }

        // Connect the existing scene ingredient if assigned.
        if (sceneIngredient != null)
        {
            trapComponent.sceneIngredient = sceneIngredient;
            trapComponent.collectiblePrefab = null;
        }

        Debug.Log(
            $"[TRAP SPAWNER] Spawned {spawnedTrap.name} " +
            $"at {spawnedTrap.transform.position}"
        );
    }
}
