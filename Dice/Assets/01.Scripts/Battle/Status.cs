using UnityEngine;

public class Status : MonoBehaviour
{
    #region properties
    private int hp;
    private int atk;
    private int def;
    private int pot;

    public int _hp { get; private set; }
    public int _atk { get; private set; }
    public int _def { get; private set; }
    public int _pot { get; private set; }
    #endregion

    #region Public Method
    public void InitStatus(int hp, int atk, int def, int pot)
    {
        this.hp = hp;
        this.atk = atk;
        this.def = def;
        this.pot = pot;
    }

    /// <summary>
    /// 플레이어 스탯값에 값을 더하거나 뺍니다. {hp, atk, def, pot} {정수값 + -}
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="value"></param>
    public void UpdateStatus(string stat, int value)
    {
        switch (stat)
        {
            case ("hp"):
                hp = ++value; break;
            case ("atk"):
                atk = ++value; break;
            case ("def"):
                def = ++value; break;
            case ("pot"):
                pot = ++value; break;
            default:
                Debug.Log($"{stat} wrong name : hp, atk, def, pot");
                break;
        }
    }
    #endregion

}
