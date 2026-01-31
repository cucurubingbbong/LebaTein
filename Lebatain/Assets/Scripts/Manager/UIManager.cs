using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// UI 유형
/// </summary>
public enum UIScreenType
{
    None = 0,
    Title = 1,
    Game_BuildSelect = 2,
    Game_BuildTile = 3,
    Game_BuildTower = 4,
}

[System.Serializable]
public class UIScreen
{
    public UIScreenType type;
    public GameObject root;
}


public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public Dictionary<UIScreenType, GameObject> screenMap = new Dictionary<UIScreenType, GameObject>();

    public UIScreenType currentScreen = UIScreenType.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        UiSetting();

        ShowScreen(UIScreenType.None);
    }


    public void UiSetting()
    {
        UIScreenRoot[] uiElements = FindObjectsByType<UIScreenRoot>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var elements in uiElements)
        {
            elements.Assign();
        }
    }

    /// <summary>
    /// 패널 등록
    /// </summary>
    /// <param name="type">유형</param>
    /// <param name="root">등록할 패널</param>

    public void RegisterScreen(UIScreenType type, GameObject root)
    {
        screenMap[type] = root;
        root.SetActive(false);
    }

    /// <summary>
    /// 패널 해제
    /// </summary>
    /// <param name="type">해제할 유형</param>
    /// <param name="root">해제할 패널</param>
    public void UnregisterScreen(UIScreenType type, GameObject root)
    {
        if (screenMap.TryGetValue(type, out var stored) && stored == root)
        {
            screenMap.Remove(type);
        }
    }

    /// <summary>
    /// 패널 보여주기
    /// </summary>
    /// <param name="type">보여줄 유형</param>
    public void ShowScreen(UIScreenType type)
    {

        if (screenMap.TryGetValue(currentScreen, out var prevRoot))
            prevRoot.SetActive(false);

        if (screenMap.TryGetValue(type, out var newRoot))
        {
            newRoot.SetActive(true);
            currentScreen = type;
        }
    }

    public void ShowScreenInt(int type)
    {
        ShowScreen((UIScreenType)type);
    }


}
