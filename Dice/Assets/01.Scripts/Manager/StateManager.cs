using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StateManager : MonoBehaviour
{
    #region Singleton Init
    public static StateManager Instance { get; private set; }
    private void Awake()
    {
        // ï¿½Ì±ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ï¿½ï¿½ï¿½ï¿½ ï¿½Î½ï¿½ï¿½Ï½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿?ï¿½ï¿½ ï¿½Î½ï¿½ï¿½Ï½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ï¿½ï¿½ ï¿½ï¿½È¯ ï¿½ï¿½ ï¿½Ä±ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
    }
    #endregion

    #region Variables

    public enum GameState
    {
        Roll = 0,
        Battle = 1,
        UpgradeDice = 2,
        UpgradeBonus = 3,
    }

    private GameState currentState;
    public GameState CurrentState { get { return currentState; } }

    private bool isGameOver;
    public bool IsGameOver { get { return isGameOver; } }

    public static event Action OnCurrentStateCompleted;
    private bool isCurrentStateDone = false;
    #endregion

    #region State Management
    public void StartGame()
    {
        currentState = GameState.Roll;
        isGameOver = false;
        StartCoroutine(TaskStateRoll());
    }
    
    IEnumerator TaskStateRoll()
    {
        Debug.Log("roll");

        PoolManager.Instance.ActivateObject("Roll");

        yield return StartCoroutine(WaitForStateCompletion());

        PoolManager.Instance.DeactivateObject("Roll");
        RunNextState();
    }

    IEnumerator TaskStateBattle()
    {
        Debug.Log("battle");

        PoolManager.Instance.ActivateObject("Battle");

        yield return StartCoroutine(WaitForStateCompletion());

        PoolManager.Instance.DeactivateObject("Battle");
        RunNextState();
    }

    IEnumerator TaskStateUpGradeDice()
    {
        // ÁÖ»çÀ§ °­È­
        // Á·º¸ °­È­
        yield return StartCoroutine(WaitForStateCompletion());
        RunNextState();
    }

    IEnumerator TaskStateUpGradeBonus()
    {
        // ÁÖ»çÀ§ °­È­
        // Á·º¸ °­È­
        yield return StartCoroutine(WaitForStateCompletion());
        RunNextState();
    }

    IEnumerator WaitForStateCompletion()
    {
        isCurrentStateDone = false; // init
        OnCurrentStateCompleted += () => isCurrentStateDone = true;

        yield return new WaitUntil(() => isCurrentStateDone);

        // ï¿½Ìºï¿½Æ® ï¿½ï¿½ï¿½ï¿½ (ï¿½Þ¸ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½)
        OnCurrentStateCompleted -= () => isCurrentStateDone = true;
    }

    private void RunNextState()
    {
        currentState = (GameState)(((int)currentState + 1) % System.Enum.GetValues(typeof(GameState)).Length);
        Debug.Log($"ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿?ï¿½ï¿½ï¿½ï¿½: {currentState}");

        switch (currentState)
        {
            case GameState.Roll:
                StartCoroutine(TaskStateRoll());
                break;
            case GameState.Battle:
                StartCoroutine(TaskStateBattle());
                break;
            case GameState.UpgradeDice:
                StartCoroutine(TaskStateUpGradeDice());
                break;
            case GameState.UpgradeBonus:
                StartCoroutine(TaskStateUpGradeBonus());
                break;
        }
    }
    #endregion

    #region Public State Method
    public void AdvanceToNextState()
    {
        isCurrentStateDone = true;
    }

    public void EndGame()
    {
        isGameOver = true;
        PoolManager.Instance.ActivateObject("Gameover");
    }

    public void StartNewGame()
    {
        PoolManager.Instance.DeactivateAllObjects();
        StartGame();
        Debug.Log("new Game Start");
    }


    #endregion

}