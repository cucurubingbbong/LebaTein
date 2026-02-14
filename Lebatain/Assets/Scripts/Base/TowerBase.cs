using UnityEngine;

/// <summary>
/// 타워베이스 
/// </summary>
public class TowerBase : UnitBase
{
    [SerializeField] TowerData towerData = null;

    /// <summary>
    /// 타워의 공격력
    /// </summary>
    private int attackPower = 0;

    /// <summary>
    /// 타워 초기화
    /// </summary>

    public virtual void Init()
    {
        maxHp = towerData.maxHp;
        currentHp = maxHp;
        color = towerData.color;
        unitName = towerData.name;
        priority = towerData.priority;
        attackPower = towerData.attackPower;

    }
}
