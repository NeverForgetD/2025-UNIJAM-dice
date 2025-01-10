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
    public void InitState()
    {
        currentState = GameState.Roll;
    }


    public void StartGame()
    {
        isGameOver = false;
        StartCoroutine(MainSequence());
    }

    private IEnumerator MainSequence()
    {
        yield return StartCoroutine(TaskStateRoll());

        yield return StartCoroutine(TaskStateBattle());

        yield return StartCoroutine(TaskStateUpGrade());
    }

    
    IEnumerator TaskStateRoll()
    {
        yield return new WaitForSeconds(2);
    }

    IEnumerator TaskStateBattle()
    {
        yield return new WaitForSeconds(2);
    }

    IEnumerator TaskStateUpGrade()
    {
        yield return new WaitForSeconds(2);
    }


    public void AdvanceToNextPhase()
    {
        currentState = (GameState)(((int)currentState + 1) % System.Enum.GetValues(typeof(GameState)).Length);
        //Debug.Log($"���� ������� ����: {currentState}");
        //StartPhase();
    }


    #endregion

}