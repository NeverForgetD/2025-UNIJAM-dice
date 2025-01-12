using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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

    #region SerializeField
    [SerializeField] private TextMeshProUGUI[] roundText;
    [SerializeField] private Image playerSprite; // 급한 불 끄기
    #endregion

    #region State Management
    public void StartGame()
    {
        round = 0;
        roundText[0].text = ((round+1)/10).ToString();
        roundText[1].text = ((round+1) % 10).ToString();
        PlayerManager.Instance.Init();
        playerSprite.color = Color.white; // 급한 불 끄기
        currentState = GameState.Roll;
        isGameOver = false;
        StartCoroutine(TaskStateRoll());
        StatusManager.Instance.playerStatus.ChangeStatusAtOnce(0, 0, 0, 0);
    }
    
    IEnumerator TaskStateRoll()
    {
        
        Debug.Log("roll");
        
        SoundManager.Instance.PlayBGM("BGM3");
        StatusManager.Instance.UpdateCurrentEnemyStatus();
        StatusManager.Instance.InitPlayer();
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
        round++;
        roundText[0].text = ((round+1)/10).ToString();
        roundText[1].text = ((round+1) % 10).ToString();
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

        //SceneManager.LoadScene(1);
        StartGame();
    }
    #endregion

}