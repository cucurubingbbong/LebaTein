using UnityEngine;

public class TileBase : UnitBase
{
    /// <summary>
    /// 타일 초기화
    /// </summary>
    /// <param name="maxHp">최대체력</param>
    /// <param name="color">색깔</param>
    /// <param name="unitName">이름</param>
    /// <param name="priority">우선순위</param>
    public void Init(int maxHp, ColorType color, string unitName, int priority, IMaterialProvider materialProvider)
    {
        this.name = unitName;
        this.maxHp = maxHp;
        this.currentHp = maxHp;
        this.unitName = unitName;
        this.priority = priority;
        gameObject.name = unitName;
        SetMaterialProvider(materialProvider);
        SetColor((int)color);
    }
}
