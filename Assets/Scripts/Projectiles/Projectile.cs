using System.Collections;
using UnityEngine;

public abstract class Projectile : PoolObject
{
    [SerializeField] private protected float MoveSpeed;

    private protected float DeathTime;
    private protected string OwnerTag;
    private protected int Damage;

    private float _lifeTime = 5f;

    public virtual void OnEnable()
    {
        DeathTime = Time.time + _lifeTime;

        StartCoroutine(Move());
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(OwnerTag) == false)
        {
            if (collision.TryGetComponent(out IDestructible destructible))
            {
                destructible.LoseHealth(Damage);

                // Проблема
                ObjectPooling<Projectile>.Return(this);
            }
        }
    }

    public abstract IEnumerator Move();

    public void Setup(string ownerTag, int damage)
    {
        OwnerTag = ownerTag;
        Damage = damage;
    }
}
