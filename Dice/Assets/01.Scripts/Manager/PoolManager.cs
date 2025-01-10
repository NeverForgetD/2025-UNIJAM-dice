using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    #region Singleton Init
    public static PoolManager Instance { get; private set; }
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


    #region Poolable Objects

    [SerializeField]
    private GameObject BattleCanvas;

    #endregion

    /*
    #region Pool Method

    public void InstantiateObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("BattleCanvas가 설정되지 않았습니다.");
            return null;
        }

        GameObject newObject = Instantiate(BattleCanvas, position, rotation);
        Debug.Log($"객체 생성됨: {newObject.name} 위치: {position}");
        return newObject;
    }

    public void DestroyObject()
    {

    }


    #endregion
    */
}
