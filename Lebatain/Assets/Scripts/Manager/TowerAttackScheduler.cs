using System.Collections.Generic;
using UnityEngine;

public class TowerAttackScheduler : MonoBehaviour
{
    public static TowerAttackScheduler Instance { get; private set; }

    [SerializeField] private float tick = 0.05f;
    [SerializeField] private int perTick = 20;

    private readonly List<TowerBase> list = new(128);

    private float acc;
    private int idx;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Register(TowerBase tower)
    {
        if (tower == null || list.Contains(tower)) return;
        list.Add(tower);
    }

    public void Unregister(TowerBase tower)
    {
        if (tower == null) return;

        if (!list.Remove(tower)) return;
        if (idx > list.Count)
        {
            idx = 0;
        }
    }

    private void Update()
    {
        if (list.Count == 0) return;

        acc += Time.deltaTime;
        if (acc < tick) return;

        int run = (int)(acc / tick);
        acc -= run * tick;

        for (int i = 0; i < run; i++)
        {
            RunOne();
        }
    }

    private void RunOne()
    {
        if (list.Count == 0) return;

        int batch = perTick;
        if (batch > list.Count)
        {
            batch = list.Count;
        }

        for (int i = 0; i < batch; i++)
        {
            if (idx >= list.Count)
            {
                idx = 0;
            }

            TowerBase tower = list[idx];
            idx++;

            if (tower == null || !tower.isActiveAndEnabled) continue;
            tower.OnSchedulerTick(tick);
        }
    }
}
