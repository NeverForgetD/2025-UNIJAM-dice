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

    [SerializeField]
    private GameObject BattleCanvas;

    #endregion

    /*
    #region Pool Method

    public void InstantiateObject(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("BattleCanvas�� �������� �ʾҽ��ϴ�.");
            return null;
        }

        GameObject newObject = Instantiate(BattleCanvas, position, rotation);
        Debug.Log($"��ü ������: {newObject.name} ��ġ: {position}");
        return newObject;
    }

    public void DestroyObject()
    {

    }


    #endregion
    */
}
