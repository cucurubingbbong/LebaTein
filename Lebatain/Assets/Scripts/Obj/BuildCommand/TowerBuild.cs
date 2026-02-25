using UnityEngine;
public class TowerBuild : BuildCommand
{
    [SerializeField] GameObject[] towerGhosts;

    [SerializeField] UnitBase[] buildUnits;
    [SerializeField] EnemyManager enemyMgr;
    [SerializeField] TowerAttackScheduler sched;

    public override void Build(Vector2Int pos , IBuildPreview ibp)
    {
        UnitBase unit = Instantiate(buildUnit , new Vector3(pos.x , 0 , pos.y),ibp.GetGhostObj().transform.rotation);
        TowerBase tower = unit.GetComponent<TowerBase>();
        tower.SetMgr(enemyMgr, sched);
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
