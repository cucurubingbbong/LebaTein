using System;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    /// <summary>
    /// 색 머테리얼 배열
    /// </summary>
    public Material[] colorMaterialArr = new Material[7];

    /// <summary>
    /// 건축 명령 배열
    /// </summary>
    [SerializeField] BuildCommand[] buildCommands = null;

    public TileManager tileManager = null;

    /// <summary>
    /// 건축중인지
    /// </summary>

    public bool isBuilding =false;

    /// <summary>
    /// 위치를 선택중인지
    /// </summary>
    public bool isSelecting = false;

    /// <summary>
    /// 현재 선택된 색깔 인덱스
    /// </summary>
    public int selColorIndex = 0;


    /// <summary>
    /// 선택된 빌드커맨드 인덱스
    /// </summary>
    [SerializeField] int selectedIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if(!isSelecting && !isBuilding) return;
        if (Input.GetMouseButtonDown(0)) Select();
        if (Input.GetKeyDown(KeyCode.Q)) Cancel();
        
    }

    public Material GetMarterial(int index)
    {
        return colorMaterialArr[index];
    }

    public void Build(int index)
    {
        if(isBuilding) return;
        isBuilding = true;
        isSelecting = true;
        selectedIndex = index;
    }

    private void Select()
    {
        isSelecting = false;
        Vector3 pos = SelectPos();
        //Debug.Log(pos);
        //Debug.Log(Util.GetVector2RoundInt(pos));
        buildCommands[selectedIndex].Build(Util.GetVector2RoundInt(pos));
    }

    public void Cancel()
    {
        isBuilding = false;
        isSelecting = false;
        selectedIndex = -1;
    }

    private Vector3 SelectPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPoint = hit.point;
        }

        return hit.point;

    }

    public bool isBuild(Vector3 pos)
    {
        return false;
    }
}
