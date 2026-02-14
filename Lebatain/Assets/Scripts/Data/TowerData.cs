using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "GData/TowerData")]

    /// <summary>
    /// 타워의 기초 스텟
    /// </summary>
public class TowerData : ScriptableObject
{
    [SerializeField] int m_maxHp;

    [SerializeField] ColorType m_color;

    [SerializeField] string m_unitName;

    [SerializeField] int m_priority;

    [SerializeField] int m_attackPower;
    
    public int maxHp => m_maxHp;

    public ColorType color => m_color;

    public string unitName => m_unitName;

    public int priority => m_priority;

    public int attackPower => m_attackPower;

}
