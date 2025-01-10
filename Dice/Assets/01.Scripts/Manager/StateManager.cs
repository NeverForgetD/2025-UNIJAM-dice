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
            Destroy(gameObject); // ���� �ν��Ͻ��� ���� ��� �� �ν��Ͻ��� ����
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
        Upgrade = 2
    }

    private GameState currentState;
    public GameState CurrentState { get { return currentState; } }

    private bool isGameOver;

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

    private IEnumerator MainSequence()
    {
        yield return StartCoroutine(TaskStateRoll());
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

    IEnumerator TaskStateUpGrade()
    {
        Debug.Log("Upgrade");
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
        Debug.Log($"���� ������� ����: {currentState}");

        switch (currentState)
        {
            case GameState.Roll:
                StartCoroutine(TaskStateRoll());
                break;
            case GameState.Battle:
                StartCoroutine(TaskStateBattle());
                break;
            case GameState.Upgrade:
                StartCoroutine(TaskStateUpGrade());
                break;
        }
    }


    public void AdvanceToNextState()
    {
        isCurrentStateDone = true;
    }
    #endregion

}