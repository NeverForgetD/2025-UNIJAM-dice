using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] public EnemyStatus[] EnemyStats;
}

[System.Serializable]
public class EnemyStatus
{
    [Header("�߾Ӱ� ����")]
    public int med;


    /*
    [Header("HP ����")]
    public int hp;

    [Header("Attack ����")]
    public int atk;


    [Header("Defense ����")]
    public int def;

    [Header("Potential ����")]
    public int pot;
    */
}
