using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerEX : MonoBehaviour
{
    #region Singleton Init
    public static SceneManagerEX Instance { get; private set; }
    private void Awake()
    {
        // �̱��� ���� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���� �ν��Ͻ��� ���� ��� �� �ν��Ͻ��� ����
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� ����
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

    #region Scene ��ȯ

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
