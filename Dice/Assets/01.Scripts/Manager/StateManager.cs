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

    public enum GameState
    {
        Roll = 0,
        Battle = 1,
        Upgrade = 2
    }

    private GameState currentState;
    public GameState CurrentState { get { return currentState; } }

    private bool isGameOver;



    #region State Management
    public void StartGame()
    {
        currentState = GameState.Roll;
        isGameOver = false;
        //StartCoroutine(MainSequence());
        StartCoroutine(TaskStateRoll());
    }

    private IEnumerator MainSequence()
    {
        yield return StartCoroutine(TaskStateRoll());
    }

    
    IEnumerator TaskStateRoll()
    {
        Debug.Log("roll");
        yield return new WaitForSeconds(2);
        AdvanceToNextState();
    }

    IEnumerator TaskStateBattle()
    {
        Debug.Log("battl");

        PoolManager.Instance.ActivateObject("Battle");
        yield return new WaitForSeconds(3);
        PoolManager.Instance.DeactivateObject("Battle");
        AdvanceToNextState();
    }

    IEnumerator TaskStateUpGrade()
    {
        Debug.Log("Upgrade");
        yield return new WaitForSeconds(2);
        AdvanceToNextState();
    }


    public void AdvanceToNextState()
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
    #endregion

}