using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 빌드 커맨드 베이스
/// </summary>
[System.Serializable]
public abstract class BuildCommand : MonoBehaviour
{
    /// <summary>
    /// 빌드 컨텍스트
    /// </summary>
    protected IBuildCommandContext context;

    /// <summary>
    /// 빌드커맨드가 설치하는 건물의 고스트
    /// </summary>
    
    [field: SerializeField]
    public GameObject ghost {get; protected set;}

    /// <summary>
    /// 빌드 커맨드가 설치하는 건물의 유형
    /// </summary>

    public UnitType buildUnitType  {get; protected set;}
    public abstract void Build(Vector2Int pos);

    public abstract void Init();

    /// <summary>
    /// 빌드 컨텍스트 설정
    /// </summary>
    /// <param name="context">빌드 컨텍스트</param>
    public void SetContext(IBuildCommandContext context)
    {
        this.context = context;
    }
}
