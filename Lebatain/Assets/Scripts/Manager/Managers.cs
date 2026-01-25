using UnityEngine;

public class Managers : MonoBehaviour
{
    public static GameManager Game { get; private set; }
    private ManagerBase[] _managers;
    private void Awake()
    {
        _managers = new ManagerBase[]
        {
            Game = GetComponentInChildren<GameManager>(),
        };

        foreach (var manager in _managers)
        {
            manager.Initialize();
        }

        Game.SetState(GameState.Title);
        DontDestroyOnLoad(gameObject);
    }
}


