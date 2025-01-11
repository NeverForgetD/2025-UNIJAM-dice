using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{

    [System.Serializable]
    public class RoundStats
    {
        public int round; // ���� ��ȣ

        [Header("HP ����")]
        public bool useDefaultHP = true; // true: �ڵ� ����, false: ���� �Է�
        public float averageHP = 100f; // ��հ�
        public float hpRange = 20f; // ��������
        public int customHP; // ���� �Է� ��

        [Header("Attack ����")]
        public bool useDefaultATK = true;
        public float averageATK = 10f;
        public float atkRange = 3f;
        public int customATK;

        [Header("Defense ����")]
        public bool useDefaultDEF = true;
        public float averageDEF = 5f;
        public float defRange = 2f;
        public int customDEF;

        [Header("Position ����")]
        public Vector3 position; // ������ ��ġ
    }

    public RoundStats[] roundStats; // ���庰 ���� �迭

    /// <summary>
    /// Ư�� ������ ���� ��������
    /// </summary>
    /// <param name="round">���� ��ȣ</param>
    /// <returns>�ش� ������ ����</returns>
    public RoundStats GetStatsForRound(int round)
    {
        foreach (var stats in roundStats)
        {
            if (stats.round == round)
            {
                return stats;
            }
        }

        Debug.LogWarning($"���� {round}�� �ش��ϴ� ������ �����ϴ�.");
        return null;
    }
}
