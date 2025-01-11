using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [System.Serializable]
    public class EnemyStatus
    {
        [Header("HP 설정")]
        public int hp;

        [Header("Attack 설정")]
        public int attack;


        [Header("Defense 설정")]
        public int defence;

        [Header("Potential 설정")]
        public int pot;
    }

    public EnemyStatus[] EnemyStats; // 라운드별 스탯 배열

}
