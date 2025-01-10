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
        // �̱��� ���� ����
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ���� �ν��Ͻ��� ���� ���?�� �ν��Ͻ��� ����
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // �� ��ȯ �� �ı����� ����
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
        // �ֻ��� ��ȭ
        // ���� ��ȭ
        yield return StartCoroutine(WaitForStateCompletion());
        RunNextState();
    }

    IEnumerator TaskStateUpGradeBonus()
    {
        // �ֻ��� ��ȭ
        // ���� ��ȭ
        yield return StartCoroutine(WaitForStateCompletion());
        RunNextState();
    }

    IEnumerator WaitForStateCompletion()
    {
        isCurrentStateDone = false; // init
        OnCurrentStateCompleted += () => isCurrentStateDone = true;

        yield return new WaitUntil(() => isCurrentStateDone);

        // �̺�Ʈ ���� (�޸� ���� ����)
        OnCurrentStateCompleted -= () => isCurrentStateDone = true;
    }

    private void RunNextState()
    {
        currentState = (GameState)(((int)currentState + 1) % System.Enum.GetValues(typeof(GameState)).Length);
        Debug.Log($"���� �������?����: {currentState}");

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