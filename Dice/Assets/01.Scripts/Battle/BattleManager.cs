using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum Act{
    Attack, Defence, Charge
}

public class BattleManager : MonoBehaviour
{
    public Stat Player;
    public Stat Enemy;

    public TMP_Text T_Php, T_Pad, T_Pdef, T_Ppot, T_Pbonus;
    public TMP_Text T_Ehp, T_Ead, T_Edef, T_Epot, T_Ebonus;

    public void Init(){
        Player = new Stat();
        Player.Init();
        Enemy = new Stat();
        Enemy.Init();
    }

    private void OnEnable(){
        Init();
    }

    private void Update(){
        T_Php.text = "Player HP = " + Player.hp.ToString();
        T_Pad.text = "Player AD = " + Player.ad.ToString();
        T_Pdef.text = "Player DEF = " + Player.def.ToString();
        T_Ppot.text = "Player POT = " + Player.pot.ToString();
        T_Pbonus.text = "Player BONUS = " + Player.bonus.ToString();
        T_Ehp.text = "Enemy HP = " + Enemy.hp.ToString();
        T_Ead.text = "Enemy AD = " + Enemy.ad.ToString();
        T_Edef.text = "Enemy DEF = " + Enemy.def.ToString();
        T_Epot.text = "Enemy POT = " + Enemy.pot.ToString();
        T_Ebonus.text = "Enemy BONUS = " + Enemy.bonus.ToString();
        if (StateManager.Instance.IsGameOver)
            return;
    }

    public void Attack(){
        switch(GetEnemyAction()){
            case Act.Attack:
                Player.hp -= Enemy.Atk;
                Enemy.hp -= Player.Atk;
                Debug.Log("Atk Atk");
                break;
            case Act.Defence:
                Enemy.hp -= Mathf.Max(0, Player.Atk - Enemy.def);
                Debug.Log("Atk Def");
                break;
            case Act.Charge:
                Enemy.hp -= Player.Atk;
                Enemy.bonus += Enemy.pot;
                Debug.Log("Atk Chg");
                break;
        }
        CheckDead();
    }
    public void Defence(){
        switch(GetEnemyAction()){
            case Act.Attack:
                Player.hp -= Mathf.Max(0, Enemy.Atk - Player.def);
                Debug.Log("Def Atk");
                break;
            case Act.Defence:
                Debug.Log("Def Def");
                break;
            case Act.Charge:
                Enemy.bonus += Enemy.pot;
                Debug.Log("Def Chg");
                break;
        }
        CheckDead();
    }
    public void Charge(){
        Player.bonus += Player.pot;
        switch(GetEnemyAction()){
            case Act.Attack:
                Player.hp -= Enemy.Atk;
                Debug.Log("Chg Atk");
                break;
            case Act.Defence:
                Debug.Log("Chg Def");
                break;
            case Act.Charge:
                Enemy.bonus += Enemy.pot;
                Debug.Log("Chg Chg");
                break;
        }
        CheckDead();
    }

    public Act GetEnemyAction(){
        return (Act)Random.Range(0,3);
    }

    public void CheckDead(){
        if (Player.hp <= 0)
        {
            // 게임종료
            StateManager.Instance.EndGame();
            Debug.Log("Player Dead");
        }
        if (Enemy.hp <= 0)
        {
            StateManager.Instance.AdvanceToNextState(); // Battle Win
            Debug.Log("Enemy Dead");
        }
    }
}
