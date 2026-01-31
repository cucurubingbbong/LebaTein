using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public abstract class BuildCommand : MonoBehaviour
{
    public BuildManager build;
    public abstract void Build(Vector2Int pos);

}
