
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// 애 추상클이에요 인터페이스 아니야 
/// </summary>
public abstract class IBuildUI : MonoBehaviour
{
    public abstract void Init(BuildManager bm);
}