using UnityEngine;
public class GameManager : MonoBehaviour
{
    #region Singleton Init
    public static GameManager Instance { get; private set; }
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
        //초기화 로직
    }

    public void EndGame()
    {
        //강제 종료 로직
    }
    #endregion

}