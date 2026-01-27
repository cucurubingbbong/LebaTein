using UnityEngine;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Transform gridObj;
    [SerializeField] private Tile tilePrefab;

    private int[,] grid;
    private Tile[,] tileArr;

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
                int colorIndex = Util.GetRandomInt(0, 6);

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
