using UnityEngine;

public class TileManager : MonoBehaviour, IGridQuery, ITileAccessor
{
    [SerializeField] private Transform gridObj;
    [SerializeField] private TileBase tilePrefab;
    [SerializeField] private MonoBehaviour materialProviderSource;

    private IMaterialProvider materialProvider;

    /// <summary>
    /// 타일의 위치 그리드
    /// </summary>
    public int[,] grid;
    /// <summary>
    /// 유닛베이스를 상속한것들의 그리드
    /// </summary>
    public UnitBase[,] unitGrid;
    public TileBase[,] tileArr;

    private void Start()
    {
        materialProvider = materialProviderSource as IMaterialProvider;
        SetTile(10, 10);
    }
    public void SetTile(int width, int height)
    {
        grid = new int[width, height];
        tileArr = new TileBase[width, height];
        unitGrid = new UnitBase[width , height];

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                int colorIndex = Util.GetRandomInt(0, 5);

                grid[w, h] = colorIndex;

                string tileName = $"{w}_{h}_{(ColorType)colorIndex}";

                TileBase tile = Instantiate(tilePrefab);
                tile.transform.SetParent(gridObj, false);
                tile.transform.localPosition = new Vector3(w, 0, h);

                tile.Init(100, (ColorType)colorIndex, tileName, 5, materialProvider);
                tileArr[w, h] = tile;
            }
        }
    }

    /// <summary>
    /// 건축 가능한 그리드인지 확인
    /// </summary>
    /// <param name="gridPos">그리드 좌표</param>
    /// <returns>가능하면 true , 안되면 false</returns>
    public bool CanBuild(Vector2Int gridPos)
    {
        if (unitGrid == null) return false;
        int width = unitGrid.GetLength(0);
        int height = unitGrid.GetLength(1);
        if (gridPos.x < 0 || gridPos.x >= width || gridPos.y < 0 || gridPos.y >= height) return false;
        return unitGrid[gridPos.x , gridPos.y] == null;
    }

    /// <summary>
    /// 타일 접근
    /// </summary>
    /// <param name="gridPos">그리드 좌표</param>
    /// <returns>타일</returns>
    public TileBase GetTile(Vector2Int gridPos)
    {
        return tileArr[gridPos.x , gridPos.y];
    }
}
