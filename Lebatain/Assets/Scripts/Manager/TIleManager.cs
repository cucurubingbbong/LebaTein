using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform gridObj;
    [SerializeField] private Tile tilePrefab;

    /// <summary>
    /// 타일의 위치 그리드
    /// </summary>
    public int[,] grid;
    /// <summary>
    /// 유닛베이스를 상속한것들의 그리드
    /// </summary>
    public UnitBase[,] unitGrid;
    public Tile[,] tileArr;

    private void Start()
    {
        SetTile(10, 10);
    }
    public void SetTile(int width, int height)
    {
        grid = new int[width, height];
        tileArr = new Tile[width, height];

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {
                int colorIndex = Util.GetRandomInt(0, 5);

                grid[w, h] = colorIndex;

                string tileName = $"{w}_{h}_{(ColorType)colorIndex}";

                Tile tile = Instantiate(tilePrefab);
                tile.transform.SetParent(gridObj, false);
                tile.transform.localPosition = new Vector3(w, 0, h);

                tile.Init(100, (ColorType)colorIndex, tileName, 5);
                tileArr[w, h] = tile;
            }
        }
    }
}
