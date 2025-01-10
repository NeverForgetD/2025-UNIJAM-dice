using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerEX : MonoBehaviour
{
    #region Singleton Init
    public static SceneManagerEX Instance { get; private set; }
    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 기존 인스턴스가 있을 경우 새 인스턴스를 삭제
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // 씬 전환 시 파괴되지 않음
    }
    #endregion

    public enum GameState
    {
        Roll = 0,
        Battle = 1,
        Dice = 2
    }

    private GameState currentState;
    public GameState CurrentState { get { return currentState; } }

    #region Scene 전환

    public void GoToNextState()
    {
        if (currentState == GameState.Roll)
        {
            currentState = GameState.Battle;
        }
        else if (currentState == GameState.Battle)
        {
            currentState = GameState.Dice;
        }
        else
        {
            currentState = GameState.Roll;
        }
    }

    public void LoadScene(GameState currentState)
    {
        //SceneManager.LoadScene(0);
    }
    #endregion



}
