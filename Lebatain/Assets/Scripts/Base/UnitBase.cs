using UnityEngine;

public class UnitBase : MonoBehaviour
{
    /// <summary>
    /// 최대체력
    /// </summary>
    public int maxHp = 0;

    /// <summary>
    /// 현재체력
    /// </summary>
    public int currentHp = 0;

    /// <summary>
    /// 유닛의 색
    /// </summary>
    public ColorType color = ColorType.White;

    /// <summary>
    /// 유닛의 이름
    /// </summary>
    public string unitName = null;

    /// <summary>
    /// 타겟팅 우선순위
    /// </summary>
    public int priority = 0;

    public virtual void TakeDamage(int damage , ColorType damageType)
    {

    }


}
