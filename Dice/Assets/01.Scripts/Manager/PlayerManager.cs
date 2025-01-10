using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton Init
    public static PlayerManager Instance { get; private set; }
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

    // �÷��̾� ���� ���������� �������ִ� �̱��� �Ŵ���

    #region Properites
    


    #endregion
}
