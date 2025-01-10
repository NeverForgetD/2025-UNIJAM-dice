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
        yield return StartCoroutine(WaitForStateCompletion());

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

        // 이벤트 정리 (메모리 누수 방지)
        OnCurrentStateCompleted -= () => isCurrentStateDone = true;
    }

    private void RunNextState()
    {
        currentState = (GameState)(((int)currentState + 1) % System.Enum.GetValues(typeof(GameState)).Length);
        Debug.Log($"다음 페이즈로 진행: {currentState}");

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