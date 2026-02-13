using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TowerBuild : BuildCommand
{
    [SerializeField] GameObject[] towerGhosts;

    [SerializeField] UnitBase[] buildUnits;
    public override void Build(Vector2Int pos)
    {
        UnitBase unit = Instantiate(buildUnit , new Vector3(pos.x , 0 , pos.y),Quaternion.identity);
        TowerBase tower = unit.GetComponent<TowerBase>();
        tower.Init();
        tower.color = (ColorType)context.SelectedColorIndex;
        tower.SetColor(context.SelectedColorIndex);
        context.SetUnit(tower , pos);
    }
    public override void Init()
    {
        TowerSelect(0);
        buildUnitType = UnitType.Tower;
    }

    /// <summary>
    /// 타워선택
    /// </summary>
    /// <param name="index">인덱스</param>
    public void TowerSelect(int index)
    {
        ghost = towerGhosts[index];
        buildUnit = buildUnits[index];
    }

    
}
