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

    private int round;
    public int Round { get { return round; } }
    #endregion

    #region State Management
    public void StartGame()
    {
        round = 0;
        currentState = GameState.Roll;
        isGameOver = false;
        StatusManager.Instance.InitPlayer();
        StartCoroutine(TaskStateRoll());
    }
    
    IEnumerator TaskStateRoll()
    {
        Debug.Log("roll");
        
        SoundManager.Instance.PlayBGM("BGM3");
        StatusManager.Instance.UpdateCurrentEnemyStatus();
        PoolManager.Instance.ActivateObject("Roll");

        yield return StartCoroutine(WaitForStateCompletion());

        //PoolManager.Instance.DeactivateObject("Roll");
        PoolManager.Instance.DeactivateAllObjects();
        RunNextState();
    }

    IEnumerator TaskStateBattle()
    {
        Debug.Log("battle");

        SoundManager.Instance.PlayBGM("BGM4");
        PoolManager.Instance.ActivateObject("Battle");

        yield return StartCoroutine(WaitForStateCompletion());

        //PoolManager.Instance.DeactivateObject("Battle");
        PoolManager.Instance.DeactivateAllObjects();
        round++; // 0���忡�� ����, �̱�� ���� �ѹ� +1
        RunNextState();
    }

    IEnumerator TaskStateUpGradeDice()
    {
        SoundManager.Instance.PlayBGM("BGM2");
        PoolManager.Instance.ActivateObject("Upgradedice");
        yield return StartCoroutine(WaitForStateCompletion());
        PoolManager.Instance.DeactivateAllObjects();
        RunNextState();
    }

    IEnumerator TaskStateUpGradeBonus()
    {
        PoolManager.Instance.ActivateObject("UpgradeBonus");
        yield return StartCoroutine(WaitForStateCompletion());
        PoolManager.Instance.DeactivateAllObjects();
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