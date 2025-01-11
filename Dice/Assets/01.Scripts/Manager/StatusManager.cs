using TMPro;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    #region Singleton Init
    public static StatusManager Instance { get; private set; }
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

        playerStatus = new Status();
        enemyStatus = new Status();
    }
    #endregion

    #region SerializedField
    [SerializeField] EnemyData enemyData;

    [SerializeField] TextMeshProUGUI[] playerText;
    [SerializeField] TextMeshProUGUI[] enemyText;
    #endregion

    #region Private
    private int currentRound;
    #endregion

    #region Properties
    public Status playerStatus { get; private set; }
    public Status enemyStatus { get; private set; }

    #endregion

    #region Status
    public void UpdateCurrentEnemyStatus()
    {
        currentRound = StateManager.Instance.Round;
        enemyStatus.ChangeStatus("hp", enemyData.EnemyStats[currentRound].hp);
        enemyStatus.ChangeStatus("atk", enemyData.EnemyStats[currentRound].atk);
        enemyStatus.ChangeStatus("def", enemyData.EnemyStats[currentRound].def);
        enemyStatus.ChangeStatus("pot", enemyData.EnemyStats[currentRound].pot);
    }

    public void InitPlayer()
    {
        playerStatus.ChangeStatusAtOnce(0, 0, 0, 0);
    }
    #endregion

    private void Update()
    {
        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        playerText[0].text = playerStatus._hp.ToString();
        playerText[1].text = playerStatus._atk.ToString();
        playerText[2].text = playerStatus._def.ToString();
        playerText[3].text = playerStatus._pot.ToString();

        enemyText[0].text = enemyStatus._hp.ToString();
        enemyText[1].text = enemyStatus._atk.ToString();
        enemyText[2].text = enemyStatus._def.ToString();
        enemyText[3].text = enemyStatus._pot.ToString();

    }

}
