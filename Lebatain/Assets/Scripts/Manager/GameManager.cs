using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Loading,
    Title,
    Game,
    Result
}
public class GameManager : ManagerBase
{
    public GameState _currentState = GameState.Loading;


    public void SetState(GameState newState)
    {
        _currentState = newState;
        SceneManager.LoadScene(_currentState.ToString());

    }

    public override void Initialize()
    {
        base.Initialize();
    }


}
