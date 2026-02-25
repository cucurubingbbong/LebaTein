using UnityEngine;

/// <summary>
/// 타워베이스 
/// </summary>
public class TowerBase : UnitBase
{
    [SerializeField] TowerData towerData = null;

    [SerializeField] private TowerRangeShape shape = TowerRangeShape.Circle;
    [SerializeField] private TowerAttackMode mode = TowerAttackMode.Single;
    [SerializeField] private float radius = 3f;
    [SerializeField] private Vector2 box = new Vector2(3f, 3f);
    [SerializeField] private float angle = 60f;
    [SerializeField] private Vector3 offset = Vector3.zero;

    [SerializeField] private int power = 5;
    [SerializeField] private float interval = 0.5f;
    [SerializeField] private int areaCount = 3;
    [SerializeField] private EnemyManager enemyMgr;
    [SerializeField] private TowerAttackScheduler sched;

    private float cool = 0f;
    private bool schedOn = false;

    public virtual void Init()
    {
        maxHp = towerData.maxHp;
        currentHp = maxHp;
        color = towerData.color;
        unitName = towerData.unitName;
        priority = towerData.priority;
        power = towerData.attackPower;
        cool = 0f;
        Reg();
    }

    public void SetMgr(EnemyManager enemyMgr, TowerAttackScheduler sched)
    {
        this.enemyMgr = enemyMgr;
        this.sched = sched;
    }

    public override void SetColor(int colorIndex)
    {
        color = (ColorType)colorIndex;
        Material colorMat = BuildManager.Instance.GetMarterial(colorIndex);

        MeshRenderer[] childRenderers = GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < childRenderers.Length; i++)
        {
            childRenderers[i].sharedMaterial = colorMat;
        }
    }

    private void OnDisable()
    {
        if (!schedOn) return;

        sched.Unregister(this);
        schedOn = false;
    }

    private void Reg()
    {
        if (schedOn) return;
        sched.Register(this);
        schedOn = true;
    }

    public void OnSchedulerTick(float deltaTime)
    {
        cool -= deltaTime;
        if (cool > 0f) return;

        bool hit = DoAttack();
        if (hit)
        {
            cool = interval;
            if (cool < 0.05f) cool = 0.05f;
            return;
        }

        cool = interval;
        if (cool > 0.1f) cool = 0.1f;
    }

    private bool DoAttack()
    {
        if (mode == TowerAttackMode.Single)
        {
            return HitOne();
        }

        return HitArea();
    }

    private bool HitOne()
    {
        EnemyBase hitEnemy = null;
        float best = 999999999f;
        Vector3 center = Center();

        var enemies = enemyMgr.Enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBase enemy = enemies[i];
            if (!CanHit(enemy)) continue;

            Vector3 pos = enemy.transform.position;
            if (!InRange(pos, center)) continue;

            float distSqr = (pos - center).sqrMagnitude;
            if (distSqr >= best) continue;

            best = distSqr;
            hitEnemy = enemy;
        }

        if (hitEnemy == null) return false;

        hitEnemy.TakeDamage(power, color);
        return true;
    }

    private bool HitArea()
    {
        int hit = 0;
        int limit = areaCount;
        if (limit < 1) limit = 1;

        Vector3 center = Center();

        var enemies = enemyMgr.Enemies;
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyBase enemy = enemies[i];
            if (!CanHit(enemy)) continue;
            if (!InRange(enemy.transform.position, center)) continue;

            enemy.TakeDamage(power, color);
            hit++;

            if (hit >= limit) break;
        }

        return hit > 0;
    }

    private static bool CanHit(EnemyBase enemy)
    {
        return enemy != null && enemy.isActiveAndEnabled && enemy.IsAlive;
    }

    private Vector3 Center()
    {
        return transform.position + transform.TransformDirection(offset);
    }

    private bool InRange(Vector3 worldPoint, Vector3 center)
    {
        Vector3 delta = worldPoint - center;
        delta.y = 0f;

        switch (shape)
        {
            case TowerRangeShape.Circle:
                return delta.sqrMagnitude <= radius * radius;

            case TowerRangeShape.Box:
                Vector3 local = Quaternion.Inverse(transform.rotation) * delta;
                return Mathf.Abs(local.x) <= box.x * 0.5f &&
                       Mathf.Abs(local.z) <= box.y * 0.5f;

            case TowerRangeShape.Cone:
                float rangeSqr = radius * radius;
                float distSqr = delta.sqrMagnitude;
                if (distSqr > rangeSqr) return false;
                if (distSqr < 0.0001f) return true;

                Vector3 forward = transform.forward;
                forward.y = 0f;
                if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
                forward.Normalize();

                Vector3 dir = delta.normalized;
                float cosThreshold = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
                return Vector3.Dot(forward, dir) >= cosThreshold;

            default:
                return false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 center = Center();
        Gizmos.color = mode == TowerAttackMode.Single
            ? new Color(0.2f, 0.8f, 1f, 0.8f)
            : new Color(1f, 0.4f, 0.2f, 0.8f);

        switch (shape)
        {
            case TowerRangeShape.Circle:
                Gizmos.DrawWireSphere(center, radius);
                break;

            case TowerRangeShape.Box:
                Matrix4x4 old = Gizmos.matrix;
                Gizmos.matrix = Matrix4x4.TRS(center, transform.rotation, Vector3.one);
                Gizmos.DrawWireCube(Vector3.zero, new Vector3(box.x, 0.05f, box.y));
                Gizmos.matrix = old;
                break;

            case TowerRangeShape.Cone:
                DrawCone(center);
                break;
        }
    }

    private void DrawCone(Vector3 center)
    {
        Vector3 forward = transform.forward;
        forward.y = 0f;
        if (forward.sqrMagnitude < 0.0001f) forward = Vector3.forward;
        forward.Normalize();

        float halfAngle = angle * 0.5f;
        Vector3 left = Quaternion.AngleAxis(-halfAngle, Vector3.up) * forward * radius;
        Vector3 right = Quaternion.AngleAxis(halfAngle, Vector3.up) * forward * radius;

        Gizmos.DrawLine(center, center + left);
        Gizmos.DrawLine(center, center + right);

        const int segments = 12;
        Vector3 prev = center + left;
        for (int i = 1; i <= segments; i++)
        {
            float t = i / (float)segments;
            float a = Mathf.Lerp(-halfAngle, halfAngle, t);
            Vector3 p = center + (Quaternion.AngleAxis(a, Vector3.up) * forward * radius);
            Gizmos.DrawLine(prev, p);
            prev = p;
        }
    }
}
