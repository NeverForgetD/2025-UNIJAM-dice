using UnityEngine;

public class Status// : MonoBehaviour
{
    #region properties
    
    private int hp;
    private int atk;
    private int def;
    private int pot;

    public int _hp => hp;
    public int _atk => atk;
    public int _def => def;
    public int _pot => pot;
    #endregion

    #region Public Method
    /// <summary>
    /// �÷��̾� ���Ȱ��� ���ϰų� ���� {hp, atk, def, pot} {������ + -}
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="value"></param>
    public void ModifyStatus(string stat, int value)
    {
        switch (stat)
        {
            case ("hp"):
                hp += value; break;
            case ("atk"):
                atk += value; break;
            case ("def"):
                def += value; break;
            case ("pot"):
                pot += value; break;
            default:
                Debug.Log($"{stat} wrong name : hp, atk, def, pot");
                break;
        }
    }

    /// <summary>
    /// �� ���� ��� ���Ȱ��� ���ϰų� ����.
    /// </summary>
    /// <param name="dHp"></param>
    /// <param name="dAtk"></param>
    /// <param name="dDef"></param>
    /// <param name="dPot"></param>
    public void ModifyStatusAtOnce(int dHp, int dAtk, int dDef, int dPot)
    {
        hp += dHp;
        atk += dAtk;
        def += dDef;
        pot += dPot;
    }

    /// <summary>
    /// �÷��̾� ���Ȱ��� ���� {hp, atk, def, pot} {������ + -}
    /// </summary>
    /// <param name="stat"></param>
    /// <param name="value"></param>
    public void ChangeStatus(string stat, int value)
    {
        switch (stat)
        {
            case ("hp"):
                hp = value; break;
            case ("atk"):
                atk = value; break;
            case ("def"):
                def = value; break;
            case ("pot"):
                pot = value; break;
            default:
                Debug.Log($"{stat} wrong name : hp, atk, def, pot");
                break;
        }
    }

    /// <summary>
    /// �� ���� ��� ��ġ�� ����
    /// </summary>
    /// <param name="newHp"></param>
    /// <param name="newAtk"></param>
    /// <param name="newDef"></param>
    /// <param name="newPot"></param>
    public void ChangeStatusAtOnce(int newHp, int newAtk, int newDef, int newPot)
    {
        hp = newHp;
        atk = newAtk;
        def = newDef;
        pot = newPot;
    }
    
    public void PrintStatus(string stat)
    {
        switch (stat)
        {
            case ("hp"):
                Debug.Log("hp: "+hp);
                break;
            case ("atk"):
                Debug.Log("atk: "+atk);
                break;
            case ("def"):
                Debug.Log("def: "+def);
                break;
            case ("pot"):
                Debug.Log("pot: "+pot);
                break;
            default:
                break;
        }
    }

    public void PrintAllStstus()
    {
        Debug.Log($"hp: {hp}, atk: {atk}, def, {def}, pot, {pot}");
    }
    #endregion

}
