using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "EnemyPoolOption", menuName = "Scriptable Objects/EnemyPoolOption")]
public class EnemyPoolOption : ScriptableObject
{
    public EnemyType type;

    public EnemyBase prefab;

    public int warmCount = 100;

    public int size = 2000;

    public bool allowExpand;
}
