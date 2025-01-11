using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] public EnemyStatus[] EnemyStats;
}

[System.Serializable]
public class EnemyStatus
{
    [Header("HP 설정")]
    public int hp;

    [Header("Attack 설정")]
    public int atk;


    [Header("Defense 설정")]
    public int def;

    [Header("Potential 설정")]
    public int pot;
}
