using UnityEngine;

public abstract class Trap : MonoBehaviour, IDamagable
{
    [Header("Trap")]
    public int maxHp = 50;
    public int currentHp;
    public int damage = 20;

    [Header("Collectible Drop")]
    public GameObject collectiblePrefab;

    [Tooltip("An ingredient already placed in the scene.")]
    public GameObject sceneIngredient;

    private bool destroyed = false;

    protected virtual void Awake()
    {
        currentHp = maxHp;

        // Hide the ingredient before the first frame
        if (sceneIngredient != null)
        {
            sceneIngredient.SetActive(false);
        }
    }

    public abstract void ApplyDamage(IDamagable damagable);

    public virtual void TakeDamage(int howMuch)
    {
        if (destroyed)
            return;

        currentHp -= howMuch;

        Debug.Log(
            gameObject.name + " HP: " + currentHp
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (destroyed)
            return;

        destroyed = true;

        // Activate an ingredient already placed in the scene
        if (sceneIngredient != null)
        {
            sceneIngredient.SetActive(true);
        }
        // Otherwise spawn a new ingredient prefab
        else if (collectiblePrefab != null)
        {
            Instantiate(
                collectiblePrefab,
                transform.position,
                Quaternion.identity
            );
        }
        else
        {
            Debug.LogWarning(
                gameObject.name +
                " has no ingredient assigned!"
            );
        }

        Destroy(gameObject);
    }

    protected void DamagePlayer(GameObject player)
    {
        IDamagable damagable =
            player.GetComponent<IDamagable>();

        if (damagable != null)
        {
            ApplyDamage(damagable);
        }
    }
}