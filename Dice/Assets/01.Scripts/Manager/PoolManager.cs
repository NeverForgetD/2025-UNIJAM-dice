using System.Collections.Generic;
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
    /// <summary>
    /// 관리할 오브젝트 리스트.
    /// </summary>
    [SerializeField]
    private List<GameObject> poolableObjects;
    #endregion

    #region Object Management Methods
    /// <summary>
    /// 특정 오브젝트를 활성화합니다.
    /// </summary>
    /// <param name="objectName">활성화할 오브젝트의 이름.</param>
    public void ActivateObject(string objectName)
    {
        GameObject obj = FindObjectByName(objectName);
        if (obj != null)
        {
            obj.SetActive(true);
            Debug.Log($"오브젝트 활성화됨: {objectName}");
        }
        else
        {
            Debug.LogWarning($"'{objectName}'이라는 이름의 오브젝트를 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// 특정 오브젝트를 비활성화합니다.
    /// </summary>
    /// <param name="objectName">비활성화할 오브젝트의 이름.</param>
    public void DeactivateObject(string objectName)
    {
        GameObject obj = FindObjectByName(objectName);
        if (obj != null)
        {
            obj.SetActive(false);
            Debug.Log($"오브젝트 비활성화됨: {objectName}");
        }
        else
        {
            Debug.LogWarning($"'{objectName}'이라는 이름의 오브젝트를 찾을 수 없습니다.");
        }
    }

    /// <summary>
    /// 특정 오브젝트를 인스턴스화합니다.
    /// </summary>
    /// <param name="objectName">인스턴스화할 오브젝트의 이름.</param>
    /// <param name="position">생성 위치.</param>
    /// <param name="rotation">생성 회전 값.</param>
    /// <returns>인스턴스화된 게임 오브젝트.</returns>
    public GameObject InstantiateObject(string objectName, Vector3 position, Quaternion rotation)
    {
        GameObject obj = FindObjectByName(objectName);
        if (obj != null)
        {
            GameObject newInstance = Instantiate(obj, position, rotation);
            newInstance.name = obj.name; // 이름 복사
            Debug.Log($"오브젝트 인스턴스화됨: {objectName} 위치: {position}");
            return newInstance;
        }
        else
        {
            Debug.LogWarning($"'{objectName}'이라는 이름의 오브젝트를 찾을 수 없습니다.");
            return null;
        }
    }

    /// <summary>
    /// 특정 오브젝트를 삭제합니다.
    /// </summary>
    /// <param name="obj">삭제할 게임 오브젝트.</param>
    /// <param name="delay">삭제 지연 시간.</param>
    public void DestroyObject(GameObject obj, float delay = 0f)
    {
        if (obj != null)
        {
            Destroy(obj, delay);
            Debug.Log($"오브젝트 삭제됨: {obj.name} 지연 시간: {delay}초");
        }
        else
        {
            Debug.LogWarning("삭제하려는 오브젝트가 null입니다.");
        }
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// 이름으로 오브젝트를 검색합니다.
    /// </summary>
    /// <param name="objectName">검색할 오브젝트의 이름.</param>
    /// <returns>찾은 게임 오브젝트.</returns>
    private GameObject FindObjectByName(string objectName)
    {
        foreach (GameObject obj in poolableObjects)
        {
            if (obj.name == objectName)
            {
                return obj;
            }
        }
        return null;
    }
    #endregion
}
