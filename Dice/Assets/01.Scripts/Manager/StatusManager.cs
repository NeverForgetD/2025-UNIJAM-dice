using TMPro;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    #region Singleton Init
    public static StatusManager Instance { get; private set; }
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
        float baseValue = enemyData.EnemyStats[currentRound].med;

        int enemy_hp = Mathf.RoundToInt(baseValue * Random.Range(0.5f, 1.5f) * 2);
        int enemy_atk = Mathf.RoundToInt(baseValue * Random.Range(0.5f, 1.5f));
        int enemy_def = Mathf.RoundToInt(baseValue * Random.Range(0.5f, 1.5f));
        int enemy_pot = Mathf.RoundToInt(baseValue * Random.Range(0.5f, 1.5f));


        enemyStatus.ChangeStatus("hp", enemy_hp);
        enemyStatus.ChangeStatus("atk", enemy_atk);
        enemyStatus.ChangeStatus("def", enemy_def);
        enemyStatus.ChangeStatus("pot", enemy_pot);
    }

    public void InitPlayer()
    {

        playerStatus.ChangeStatusAtOnce(0, 0, 0, 0);
        //playerStatus.ChangeStatusAtOnce(playerStatus._hp, 0, 0, 0);
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
