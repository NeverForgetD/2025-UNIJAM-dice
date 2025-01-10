using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton Init
    public static PlayerManager Instance { get; private set; }
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

    // 플레이어 정보 영구적으로 가지고있는 싱글톤 매니저

    #region Properites
    


    #endregion
}
