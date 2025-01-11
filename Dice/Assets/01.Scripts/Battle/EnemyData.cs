using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{

    [System.Serializable]
    public class RoundStats
    {
        public int round; // 라운드 번호

        [Header("HP 설정")]
        public bool useDefaultHP = true; // true: 자동 생성, false: 직접 입력
        public float averageHP = 100f; // 평균값
        public float hpRange = 20f; // 오차범위
        public int customHP; // 직접 입력 값

        [Header("Attack 설정")]
        public bool useDefaultATK = true;
        public float averageATK = 10f;
        public float atkRange = 3f;
        public int customATK;

        [Header("Defense 설정")]
        public bool useDefaultDEF = true;
        public float averageDEF = 5f;
        public float defRange = 2f;
        public int customDEF;

        [Header("Position 설정")]
        public Vector3 position; // 몬스터의 위치
    }

    public RoundStats[] roundStats; // 라운드별 스탯 배열

    /// <summary>
    /// 특정 라운드의 스탯 가져오기
    /// </summary>
    /// <param name="round">라운드 번호</param>
    /// <returns>해당 라운드의 스탯</returns>
    public RoundStats GetStatsForRound(int round)
    {
        foreach (var stats in roundStats)
        {
            if (stats.round == round)
            {
                return stats;
            }
        }

        Debug.LogWarning($"라운드 {round}에 해당하는 스탯이 없습니다.");
        return null;
    }
}
