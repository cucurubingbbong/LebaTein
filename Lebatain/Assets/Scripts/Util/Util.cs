
using UnityEngine;

public static class Util 
{
    public static int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }

    public static Vector2Int GetVector2Int(Vector3 pos)
    {
        return new Vector2Int((int)pos.x , (int)pos.z);
    }
}
