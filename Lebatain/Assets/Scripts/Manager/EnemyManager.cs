using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [SerializeField] private EnemyPool pool;

    readonly List<EnemyBase> list = new(512);
    readonly Dictionary<int, int> map = new(512);

    public IReadOnlyList<EnemyBase> Enemies => list;

    public int Count => list.Count;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public EnemyBase Spawn(EnemyType type, Vector3 position, Quaternion rotation)
    {
        return pool.Spawn(type, position, rotation);
    }

    public void Despawn(EnemyBase enemy)
    {
        if (enemy == null) return;
        pool.Despawn(enemy);
    }

    public void Register(EnemyBase enemy)
    {
        if (enemy == null) return;

        int id = enemy.GetInstanceID();
        if (map.ContainsKey(id)) return;

        map[id] = list.Count;
        list.Add(enemy);
    }

    public void Unregister(EnemyBase enemy)
    {
        if (enemy == null) return;

        int id = enemy.GetInstanceID();
        if (!map.TryGetValue(id, out int index)) return;

        int lastIndex = list.Count - 1;
        if (index != lastIndex)
        {
            EnemyBase last = list[lastIndex];
            list[index] = last;
            map[last.GetInstanceID()] = index;
        }

        list.RemoveAt(lastIndex);
        map.Remove(id);
    }
}
