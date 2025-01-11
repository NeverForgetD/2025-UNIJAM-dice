using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [System.Serializable]
    public class EnemyStatus
    {
        [Header("HP ����")]
        public int hp;

        [Header("Attack ����")]
        public int attack;


        [Header("Defense ����")]
        public int defence;

        [Header("Potential ����")]
        public int pot;
    }

    public EnemyStatus[] EnemyStats; // ���庰 ���� �迭

}
