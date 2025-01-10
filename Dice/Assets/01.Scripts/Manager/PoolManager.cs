using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PoolManager : MonoBehaviour
{
    #region Singleton Init
    public static PoolManager Instance { get; private set; }
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

    #region Poolable Objects
    /// <summary>
    /// ������ ������Ʈ ����Ʈ.
    /// </summary>
    [SerializeField]
    private List<GameObject> poolableObjects;
    #endregion

    #region Object Management Methods
    /// <summary>
    /// Ư�� ������Ʈ�� Ȱ��ȭ�մϴ�.
    /// </summary>
    /// <param name="objectName">Ȱ��ȭ�� ������Ʈ�� �̸�.</param>
    public void ActivateObject(string objectName)
    {
        GameObject obj = FindObjectByName(objectName);
        if (obj != null)
        {
            obj.SetActive(true);
            Debug.Log($"������Ʈ Ȱ��ȭ��: {objectName}");
        }
        else
        {
            Debug.LogWarning($"'{objectName}'�̶�� �̸��� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    /// <summary>
    /// Ư�� ������Ʈ�� ��Ȱ��ȭ�մϴ�.
    /// </summary>
    /// <param name="objectName">��Ȱ��ȭ�� ������Ʈ�� �̸�.</param>
    public void DeactivateObject(string objectName)
    {
        GameObject obj = FindObjectByName(objectName);
        if (obj != null)
        {
            obj.SetActive(false);
            Debug.Log($"������Ʈ ��Ȱ��ȭ��: {objectName}");
        }
        else
        {
            Debug.LogWarning($"'{objectName}'�̶�� �̸��� ������Ʈ�� ã�� �� �����ϴ�.");
        }
    }

    /// <summary>
    /// Ư�� ������Ʈ�� �ν��Ͻ�ȭ�մϴ�.
    /// </summary>
    /// <param name="objectName">�ν��Ͻ�ȭ�� ������Ʈ�� �̸�.</param>
    /// <param name="position">���� ��ġ.</param>
    /// <param name="rotation">���� ȸ�� ��.</param>
    /// <returns>�ν��Ͻ�ȭ�� ���� ������Ʈ.</returns>
    public GameObject InstantiateObject(string objectName, Vector3 position, Quaternion rotation)
    {
        GameObject obj = FindObjectByName(objectName);
        if (obj != null)
        {
            GameObject newInstance = Instantiate(obj, position, rotation);
            newInstance.name = obj.name; // �̸� ����
            Debug.Log($"������Ʈ �ν��Ͻ�ȭ��: {objectName} ��ġ: {position}");
            return newInstance;
        }
        else
        {
            Debug.LogWarning($"'{objectName}'�̶�� �̸��� ������Ʈ�� ã�� �� �����ϴ�.");
            return null;
        }
    }

    /// <summary>
    /// Ư�� ������Ʈ�� �����մϴ�.
    /// </summary>
    /// <param name="obj">������ ���� ������Ʈ.</param>
    /// <param name="delay">���� ���� �ð�.</param>
    public void DestroyObject(GameObject obj, float delay = 0f)
    {
        if (obj != null)
        {
            Destroy(obj, delay);
            Debug.Log($"������Ʈ ������: {obj.name} ���� �ð�: {delay}��");
        }
        else
        {
            Debug.LogWarning("�����Ϸ��� ������Ʈ�� null�Դϴ�.");
        }
    }
    #endregion

    #region Helper Methods
    /// <summary>
    /// �̸����� ������Ʈ�� �˻��մϴ�.
    /// </summary>
    /// <param name="objectName">�˻��� ������Ʈ�� �̸�.</param>
    /// <returns>ã�� ���� ������Ʈ.</returns>
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
