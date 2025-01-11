using UnityEngine;
public class GameManager : MonoBehaviour
{
    #region Singleton Init
    public static GameManager Instance { get; private set; }
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

        Init();
    }

    private void Init()
    {

    }

    #endregion

    #region Unity LifeCycle
    private void Start()
    {
        InitializeGame();


        StateManager.Instance.StartGame();
    }
    #endregion

    #region Util
    public void InitializeGame()
    {
        //�ʱ�ȭ ����
    }

    public void EndGame()
    {
        //���� ���� ����
    }
    #endregion

}