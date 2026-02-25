using UnityEngine;

public class EnemyBase : UnitBase
{
    [SerializeField] private EnemyType type = EnemyType.Default;

    private EnemyPool pool;

    public EnemyType Type => type;
    public bool IsAlive => currentHp > 0;

    public void BindPool(EnemyPool ownerPool)
    {
        pool = ownerPool;
    }

    public void SetType(EnemyType type)
    {
        this.type = type;
    }

    // Spawn 직전에 초기화
    public virtual void PrepareForSpawn()
    {
        currentHp = maxHp;
    }

    // Spawn 직후 1회 훅
    public virtual void OnSpawned()
    {
        EnemyManager.Instance.Register(this);
    }

    // Despawn 직전 1회 훅
    public virtual void OnDespawned()
    {
        EnemyManager.Instance.Unregister(this);
    }

    public override void TakeDamage(int damage, ColorType damageType)
    {
        if (!IsAlive || damage <= 0) return;

        currentHp -= damage;
        if (currentHp > 0) return;

        currentHp = 0;
        Die();
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, ColorType.White);
    }

    private void Die()
    {
        pool.Despawn(this);
    }
}
