using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    #region SerializedField
    [SerializeField] Sprite[] enemyDiceSprite;
    [SerializeField] Image[] attackContainer;
    [SerializeField] Image[] defenceContainer;
    [SerializeField] Image[] chargeContainer;

    [SerializeField] Image enemyDice;

    #endregion

    #region Enemy Dice Visualize
    private void Start()
    {
        ResetContainers(); // �����̳� �ʱ�ȭ
        AssignDiceToContainers(); // �ֻ��� ��ġ
    }

    /// <summary>
    /// ��� �����̳ʸ� �ʱ�ȭ(��Ȱ��ȭ).
    /// </summary>
    private void ResetContainers()
    {
        ResetContainer(attackContainer);
        ResetContainer(defenceContainer);
        ResetContainer(chargeContainer);
    }

    /// <summary>
    /// Ư�� �����̳��� ��� �̹����� �ʱ�ȭ(��Ȱ��ȭ).
    /// </summary>
    private void ResetContainer(Image[] container)
    {
        foreach (Image image in container)
        {
            image.sprite = null; // Sprite �ʱ�ȭ
            image.gameObject.SetActive(false); // ��Ȱ��ȭ
        }
    }

    /// <summary>
    /// ���ڸ� �����ϰ� �����̳ʿ� �����ϰ� �������� ��Ȱ��ȭ.
    /// </summary>
    public void AssignDiceToContainers()
    {
        ResetContainers(); // ���� �� �����̳� �ʱ�ȭ

        // 1���� 6������ ���� ����
        int[] numbers = { 1, 2, 3, 4, 5, 6 };

        // �� ���ڸ� �����ϰ� �ϳ��� �����̳ʿ� ����
        foreach (int number in numbers)
        {
            AssignToRandomContainer(number);
        }
    }

    /// <summary>
    /// ���ڸ� ������ �����̳ʿ� ����.
    /// </summary>
    private void AssignToRandomContainer(int number)
    {
        int randomContainer = Random.Range(0, 3); // 0: attack, 1: defence, 2: charge
        Image[] targetContainer = randomContainer switch
        {
            0 => attackContainer,
            1 => defenceContainer,
            2 => chargeContainer,
            _ => null
        };

        // ��� �ִ� ������ ã�� Sprite ����
        foreach (Image image in targetContainer)
        {
            if (image.sprite == null) // Sprite�� ��� �ִ� ����
            {
                image.sprite = enemyDiceSprite[number - 1]; // 1~6�� �´� Sprite ����
                image.gameObject.SetActive(true); // Ȱ��ȭ
                return;
            }
        }
    }
    #endregion

    #region Enemy Dice Roll
    int enemyIndex;
    public void EnemyRoll()
    {
        int enemyDiceIndex = Random.Range(0, 6);
        enemyIndex = GetContainerForNumber(enemyDiceIndex);

        enemyDice.sprite = enemyDiceSprite[enemyDiceIndex];
    }

    private int GetContainerForNumber(int number)
    {
        // ���ڿ� �ش��ϴ� Sprite ��������
        Sprite targetSprite = enemyDiceSprite[number];

        // �� �����̳ʿ��� �ش� Sprite�� �˻�
        if (ContainsSprite(attackContainer, targetSprite)) return 0; // attack
        if (ContainsSprite(defenceContainer, targetSprite)) return 1; // defence
        if (ContainsSprite(chargeContainer, targetSprite)) return 2; // charge

        // �ش� ���ڰ� ��� �����̳ʿ��� ���� ���
        return -1; // not found
    }

    /// <summary>
    /// �����̳ʰ� Ư�� Sprite�� �����ϰ� �ִ��� Ȯ��.
    /// </summary>
    private bool ContainsSprite(Image[] container, Sprite sprite)
    {
        foreach (Image image in container)
        {
            if (image.sprite == sprite) return true;
        }
        return false;
    }
    #endregion

    #region Battle Loop
    public void OnBattleStart()
    {
        ResetContainers(); // �����̳� �ʱ�ȭ
        AssignDiceToContainers(); // �ֻ��� ��ġ
    }

    public void OnPlayerMove(int index)
    {
        StartCoroutine(ExecuteBattle());

            

    }
    public IEnumerator ExecuteBattle()
    {
        yield return StartCoroutine(RollDiceForDuration(0.6f)); // 0.6�� ���� �ֻ����� ����
        EnemyRoll();
    }

    public void DetermineResult(int index)
    {
        if (index == 0)
        {
            if (enemyIndex == 1)
            {
                // ������ ������ -����
            }
            else
            {
                // ������ ���� ������
            }
        }
    }

    #endregion

    #region Util

    private IEnumerator RollDiceForDuration(float duration)
    {
        float elapsedTime = 0f;
        int i = 0;
        while (elapsedTime < duration)
        {
            enemyDice.sprite = enemyDiceSprite[i % enemyDiceSprite.Length];
            i++;
            elapsedTime += Time.deltaTime; // ��� �ð� ������Ʈ
            //yield return new WaitForSeconds(0.02f);
            yield return null; // ���� �����ӱ��� ���
        }
    }
    #endregion

    #region Test
    public void OnBtn()
    {
        OnBattleStart();
    }
    #endregion


}


















/*
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
            // ��������
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
*/