using UnityEngine;

public static class Util 
{
    public static int GetRandomInt(int min, int max)
    {
        return Random.Range(min, max + 1);
    }
}
