using UnityEngine;

public class TIleManager : MonoBehaviour
{
    /// <summary>
    /// -1 = 비어있다
    /// 0 ~ 6 = 할당된 타일색
    /// </summary>
    public int[,] grid;

    public Tile[,] tileArr;

    public GameObject gridObj = null;
    private void Start()
    {
        
    }

    public void SetTIle(int width , int height)
    {
        grid = new int[width, height];

        for (int w = 0; w < width; w++)
        {
            for (int h = 0; h < height; h++)
            {

            }
        }
    }
}
