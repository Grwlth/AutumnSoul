using UnityEngine;

public abstract class Trap : MonoBehaviour, IDamagable
{
    [Header("Trap")]
    public int maxHp = 50;
    public int currentHp;

    public int damage = 20;

    protected virtual void Start()
    {
        currentHp = maxHp;
    }

    public abstract void ApplyDamage(IDamagable damagable);

    public virtual void TakeDamage(int howMuch)
    {
        currentHp -= howMuch;

        Debug.Log(gameObject.name + " HP: " + currentHp);

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
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