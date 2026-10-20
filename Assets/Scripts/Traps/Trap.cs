using UnityEngine;

public abstract class Trap : MonoBehaviour, IDamagable
{
    [Header("Trap")]
    public int maxHp = 50;
    public int currentHp;
    public int damage = 20;

    [Header("Collectible Drop")]
    public GameObject collectiblePrefab;

    protected virtual void Start()
    {
        currentHp = maxHp;
    }

    public abstract void ApplyDamage(IDamagable damagable);

    public virtual void TakeDamage(int howMuch)
    {
        currentHp -= howMuch;

        Debug.Log(
            gameObject.name +
            " HP: " +
            currentHp
        );

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        if (collectiblePrefab != null)
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
                " has no collectible prefab assigned!"
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