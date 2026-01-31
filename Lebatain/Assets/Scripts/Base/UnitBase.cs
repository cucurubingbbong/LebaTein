using UnityEngine;

public abstract class UnitBase : MonoBehaviour
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
    /// 타겟팅 
    /// </summary>
    public int priority = 0;

    /// <summary>
    /// 메쉬 렌더러
    /// </summary>
    [SerializeField] private MeshRenderer meshRenderer;


    public virtual void TakeDamage(int damage, ColorType damageType)
    {
        currentHp -= damage;
    }

    public void SetColor(int colorIndex)
    {
        color = (ColorType)colorIndex;

        meshRenderer.sharedMaterial = BuildManager.Instance.GetMarterial(colorIndex);
    }


}
