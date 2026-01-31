using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using UnityEngine;

[System.Serializable]
public abstract class BuildCommand : MonoBehaviour
{
    public abstract void Build(Vector2Int pos);

}
