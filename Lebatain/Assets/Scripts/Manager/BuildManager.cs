using System;
using UnityEngine;

public enum BuildState
{
    /// <summary>
    /// 대기
    /// </summary>
    Idle,
    /// <summary>
    /// 고스트 미리보기 상태
    /// </summary>
    Preview,
    /// <summary>
    /// 배치 처리 중
    /// </summary>
    Placing,
    /// <summary>
    /// 취소 처리 중
    /// </summary>
    Cancelled
}

public class BuildManager : MonoBehaviour, IBuildCommandContext, IMaterialProvider
{
    public static BuildManager Instance { get; private set; }

    /// <summary>
    /// 레이가 맞지 않았을 때 사용하는 무효 좌표
    /// </summary>
    private static readonly Vector3 InvalidPos = new Vector3(-1 , -1 , -1);

    /// <summary>
    /// 색 머테리얼 배열
    /// </summary>
    public Material[] colorMaterialArr = new Material[7];

    /// <summary>
    /// 건축 명령 배열
    /// </summary>
    [SerializeField] BuildCommand[] buildCommands = null;

    /// <summary>
    /// 그리드 쿼리 제공자
    /// </summary>
    [SerializeField] private MonoBehaviour tileManager = null;

    /// <summary>
    /// 고스트 미리보기 제공자
    /// </summary>
    [SerializeField] private MonoBehaviour ghostManager = null;

    private IGridQuery gridQuery;
    private ITileAccessor tileAccessor;
    private IBuildPreview ghostPreview;

    /// <summary>
    /// 현재 빌드 상태
    /// </summary>
    [SerializeField] private BuildState currentState = BuildState.Idle;

    /// <summary>
    /// 건축중인지
    /// </summary>
    public bool isBuilding => currentState == BuildState.Preview;

    /// <summary>
    /// 상태 전이표
    /// Idle -> Preview  : Build()
    /// Preview -> Placing  : Select()
    /// Preview -> Cancelled : Cancel()
    /// Placing -> Idle : PlaceComplete()
    /// Cancelled-> Idle : CancelComplete()
    /// </summary>

    /// <summary>
    /// 현재 선택된 색깔 인덱스
    /// </summary>
    public int selColorIndex = 0;

    /// <summary>
    /// 현재 선택된 색깔 인덱스
    /// </summary>
    public int SelectedColorIndex => selColorIndex;


    /// <summary>
    /// 선택된 빌드커맨드 인덱스
    /// </summary>
    [SerializeField] int selectedIndex = 0;

    private void Awake()
    {
        Instance = this;
        gridQuery = tileManager as IGridQuery;
        tileAccessor = tileManager as ITileAccessor;
        ghostPreview = ghostManager as IBuildPreview;
    }

    void Start()
    {
        foreach(BuildCommand bc in buildCommands)
        {
            bc.SetContext(this);
            bc.Init();
        }
    }

    void Update()
    {
        if (currentState == BuildState.Preview) UpdateBuildFlow();
    }

    /// <summary>
    /// 빌드 입력 및 상태 흐름
    /// </summary>
    private void UpdateBuildFlow()
    {
        Vector3 pos = SelectPos();
        if (pos == InvalidPos)
        {
            UpdateGhost(false, new Vector2Int(-1, -1));
            HandleCancelInput();
            return;
        }

        Vector2Int gridPos = Util.GetVector2RoundInt(pos);
        bool canBuild = isBuild(pos);

        UpdateGhost(canBuild, gridPos);
        HandleBuildInput(canBuild, gridPos);
        HandleCancelInput();
    }

    /// <summary>
    /// 고스트 위치 및 재질 갱신
    /// </summary>
    /// <param name="canBuild">건축 가능 여부</param>
    /// <param name="gridPos">그리드 좌표</param>
    private void UpdateGhost(bool canBuild, Vector2Int gridPos)
    {
        ghostPreview.Ghost(canBuild, gridPos);
    }

    /// <summary>
    /// 클릭 입력 처리
    /// </summary>
    /// <param name="canBuild">건축 가능 여부</param>
    /// <param name="gridPos">그리드 좌표</param>
    private void HandleBuildInput(bool canBuild, Vector2Int gridPos)
    {
        if (Input.GetMouseButtonDown(0) && canBuild) Select(gridPos);
    }

    /// <summary>
    /// 취소 입력 처리
    /// </summary>
    private void HandleCancelInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) Cancel();
    }

    public Material GetMarterial(int index)
    {
        return GetMaterial(index);
    }

    public Material GetMaterial(int index)
    {
        return colorMaterialArr[index];
    }

    public void Build(int index)
    {
        if(isBuilding) return;
        selectedIndex = index;
        ghostPreview.GetGhost(buildCommands[index]);
        ChangeState(BuildState.Preview);
    }

    /// <summary>
    /// 이미 계산된 그리드 좌표로 건설 처리
    /// </summary>
    /// <param name="gridPos">그리드 좌표</param>
    private void Select(Vector2Int gridPos)
    {
        //Debug.Log(gridPos);
        ChangeState(BuildState.Placing);
        buildCommands[selectedIndex].Build(gridPos);
        PlaceComplete();
    }

    public void Cancel()
    {
        if (!isBuilding) return;
        ChangeState(BuildState.Cancelled);
        selectedIndex = -1;
        ghostPreview.GhostCancel();
        CancelComplete();
    }

    /// <summary>
    /// 배치 완료 처리
    /// </summary>
    private void PlaceComplete()
    {
        selectedIndex = -1;
        ghostPreview.GhostCancel();
        ChangeState(BuildState.Idle);
    }

    /// <summary>
    /// 취소 완료 처리
    /// </summary>
    private void CancelComplete()
    {
        ChangeState(BuildState.Idle);
    }

    /// <summary>
    /// 빌드 상태 변경
    /// </summary>
    /// <param name="newState">변경할 상태</param>
    private void ChangeState(BuildState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private Vector3 SelectPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPoint = hit.point;
            return hit.point;
        }
        else return InvalidPos;


    }

    /// <summary>
    /// 건축이 가능한지
    /// </summary>
    /// <param name="pos">레이를 쏘고 맞은 좌표</param>
    /// <returns>가능하면 true , 안되면 false</returns>
    public bool isBuild(Vector3 pos)
    {
        if(pos == InvalidPos) return false;
        Vector2Int gridPos = Util.GetVector2RoundInt(pos);
        if (gridQuery == null) return false;
        return gridQuery.CanBuild(gridPos);
    }

    /// <summary>
    /// 타일 접근
    /// </summary>
    /// <param name="pos">그리드 좌표</param>
    /// <returns>타일</returns>
    public TileBase GetTile(Vector2Int pos)
    {
        return tileAccessor.GetTile(pos);
    }
}
