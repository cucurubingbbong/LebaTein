using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    [SerializeField] private EnemyPoolOption[] opts;
    [SerializeField] private Transform sleepRoot;
    [SerializeField] private Transform liveRoot;

    private readonly Dictionary<EnemyType, EnemyPoolOption> optMap = new();
    private readonly Dictionary<EnemyType, Queue<EnemyBase>> qMap = new();
    private readonly Dictionary<EnemyType, int> countMap = new();

    private void Awake()
    {
        Build();
    }

    private void Build()
    {
        optMap.Clear();
        qMap.Clear();
        countMap.Clear();

        if (opts == null) return;

        for (int i = 0; i < opts.Length; i++)
        {
            EnemyPoolOption opt = opts[i];
            if (opt == null || opt.prefab == null) continue;

            optMap[opt.type] = opt;
            qMap[opt.type] = new Queue<EnemyBase>();
            countMap[opt.type] = 0;

            int limit = opt.size;
            if (limit < 1) limit = 1;

            int warm = opt.warmCount;
            if (warm < 0) warm = 0;
            if (warm > limit) warm = limit;

            for (int j = 0; j < warm; j++)
            {
                EnemyBase enemy = NewEnemy(opt);
                if (enemy == null) continue;

                qMap[opt.type].Enqueue(enemy);
                countMap[opt.type]++;
            }
        }
    }

    public EnemyBase Spawn(EnemyType type, Vector3 position, Quaternion rotation)
    {
        if (!optMap.TryGetValue(type, out EnemyPoolOption opt)) return null;

        if (!qMap.TryGetValue(type, out Queue<EnemyBase> q))
        {
            q = new Queue<EnemyBase>();
            qMap[type] = q;
            countMap[type] = 0;
        }

        EnemyBase enemy = q.Count > 0 ? q.Dequeue() : Make(type, opt);
        if (enemy == null) return null;

        Transform root = liveRoot != null ? liveRoot : transform;
        enemy.transform.SetParent(root);
        enemy.transform.SetPositionAndRotation(position, rotation);

        enemy.PrepareForSpawn();
        enemy.gameObject.SetActive(true);
        enemy.OnSpawned();

        return enemy;
    }

    public void Despawn(EnemyBase enemy)
    {
        if (enemy == null) return;

        if (!qMap.TryGetValue(enemy.Type, out Queue<EnemyBase> q))
        {
            q = new Queue<EnemyBase>();
            qMap[enemy.Type] = q;

            if (!countMap.ContainsKey(enemy.Type))
            {
                countMap[enemy.Type] = 0;
            }
        }

        enemy.OnDespawned();
        enemy.gameObject.SetActive(false);

        Transform root = sleepRoot != null ? sleepRoot : transform;
        enemy.transform.SetParent(root);

        q.Enqueue(enemy);
    }

    private EnemyBase Make(EnemyType type, EnemyPoolOption opt)
    {
        if (!countMap.TryGetValue(type, out int made))
        {
            made = 0;
        }

        int limit = opt.size;
        if (limit < 1) limit = 1;
        if (made >= limit) return null;

        int warm = opt.warmCount;
        if (warm < 0) warm = 0;
        if (made >= warm && !opt.allowExpand) return null;

        EnemyBase enemy = NewEnemy(opt);
        if (enemy == null) return null;

        countMap[type] = made + 1;
        return enemy;
    }

    private EnemyBase NewEnemy(EnemyPoolOption opt)
    {
        Transform root = sleepRoot != null ? sleepRoot : transform;
        EnemyBase enemy = Instantiate(opt.prefab, root);
        enemy.gameObject.SetActive(false);
        enemy.BindPool(this);
        enemy.SetType(opt.type);
        return enemy;
    }
}
