using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    #region SerializedField & Dependecies
    [SerializeField] Sprite[] enemyDiceSprite;
    [SerializeField] Image[] attackContainer;
    [SerializeField] Image[] defenceContainer;
    [SerializeField] Image[] chargeContainer;

    [SerializeField] Image enemyDice;

    [SerializeField] Image[] playerCharges;
    [SerializeField] Image[] enemyCharges;
    #endregion

    #region Properties
    public int playerCharge { get; private set; }
    public int enemyCharge {  get; private set; }
    #endregion

    #region Charge Visulaize
    private void ActivateCharges(Image[] charges, int count)
    {
        for (int i = 0; i < charges.Length; i++)
        {
            charges[i].gameObject.SetActive(i < count); // 지정된 개수만 활성화
        }
    }
    private void DeactivateAllCharges(Image[] charges)
    {
        foreach (var charge in charges)
        {
            charge.gameObject.SetActive(false); // 모든 charge 비활성화
        }
    }
    #endregion

    #region Enemy Dice Visualize
    /// <summary>
    /// 숫자를 랜덤하게 컨테이너에 배정하고 나머지는 비활성화.
    /// </summary>
    public void ResetEnemytable()
    {
        ResetContainers(); // 실행 시 컨테이너 초기화

        // 1부터 6까지의 숫자 생성
        int[] numbers = { 1, 2, 3, 4, 5, 6 };

        // 각 숫자를 랜덤하게 하나의 컨테이너에 배정On
        foreach (int number in numbers)
        {
            AssignToRandomContainer(number);
        }
    }

    /// <summary>
    /// 모든 컨테이너를 초기화(비활성화).
    /// </summary>
    private void ResetContainers()
    {
        ResetContainer(attackContainer);
        ResetContainer(defenceContainer);
        ResetContainer(chargeContainer);
    }

    /// <summary>
    /// 특정 컨테이너의 모든 이미지를 초기화(비활성화).
    /// </summary>
    private void ResetContainer(Image[] container)
    {
        foreach (Image image in container)
        {
            image.sprite = null; // Sprite 초기화
            image.gameObject.SetActive(false); // 비활성화
        }
    }


    /// <summary>
    /// 숫자를 랜덤한 컨테이너에 배정.
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

        // 비어 있는 슬롯을 찾아 Sprite 배정
        foreach (Image image in targetContainer)
        {
            if (image.sprite == null) // Sprite가 비어 있는 슬롯
            {
                image.sprite = enemyDiceSprite[number - 1]; // 1~6에 맞는 Sprite 설정
                image.gameObject.SetActive(true); // 활성화
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
        // 숫자에 해당하는 Sprite 가져오기
        Sprite targetSprite = enemyDiceSprite[number];

        // 각 컨테이너에서 해당 Sprite를 검색
        if (ContainsSprite(attackContainer, targetSprite)) return 0; // attack
        if (ContainsSprite(defenceContainer, targetSprite)) return 1; // defence
        if (ContainsSprite(chargeContainer, targetSprite)) return 2; // charge

        // 해당 숫자가 어느 컨테이너에도 없는 경우
        return -1; // not found
    }

    /// <summary>
    /// 컨테이너가 특정 Sprite를 포함하고 있는지 확인.
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
    public void StartBattle()
    {
        playerCharge = 0;
        enemyCharge = 0;
        DeactivateAllCharges(playerCharges);
        DeactivateAllCharges(enemyCharges);
        ResetEnemytable();
    }

    public void OnPlayerMove(int playerIndex) // 버튼 클릭으로 발동
    {
        ResetEnemytable();
        StartCoroutine(ExecuteBattle(playerIndex));
    }
    public IEnumerator ExecuteBattle(int playerIndex)
    {
        yield return StartCoroutine(RollDiceForDuration(0.6f)); // 0.6초 동안 주사위를 굴림
        EnemyRoll(); // enemy의 Index 결정 및 스프라이트 지정

        DetermineResult(playerIndex);
        StartCoroutine(CheckBattleEnd());
    }

    public void DetermineResult(int index) // index는 플레이어
    {
        int pAtk = StatusManager.Instance.playerStatus._atk + playerCharge*StatusManager.Instance.playerStatus._pot;
        int pDef = StatusManager.Instance.playerStatus._def;

        int eAtk = StatusManager.Instance.enemyStatus._atk + enemyCharge* StatusManager.Instance.enemyStatus._pot;
        int eDef = StatusManager.Instance.enemyStatus._def;


        if (index == 0) // 플레이어 공격
        {
            if (enemyIndex == 0)
            {
                ApplyBattleDamage(pAtk, eAtk);
            }
            else if (enemyIndex == 1)
            {
                ApplyBattleDamage(Mathf.Max(0, pAtk - eDef), 0);
            }
            else if (enemyIndex == 2)
            {
                ApplyBattleDamage(pAtk, 0);
                enemyCharge++;
                ActivateCharges(enemyCharges, enemyCharge);
            }
            else
            {
                Debug.Log($"적의 인덱스가 올바르지 않습니다. {enemyIndex}");
            }
        }
        else if (index == 1) // 플레이어 방어
        {
            if (enemyIndex == 0)
            {
                ApplyBattleDamage(0, Mathf.Max(0, eAtk -pDef));
            }
            else if (enemyIndex == 1)
            {
                ApplyBattleDamage(0, 0);
            }
            else if (enemyIndex == 2)
            {
                ApplyBattleDamage(0, 0);
            }
            else
            {
                Debug.Log($"적의 인덱스가 올바르지 않습니다. {enemyIndex}");
            }
        }
        else if (index == 2) // 플레이어 충전
        {
            playerCharge++;
            ActivateCharges(playerCharges, playerCharge);
            if (enemyIndex == 0)
            {
                ApplyBattleDamage(0, eAtk);
            }
            else if (enemyIndex == 1)
            {
                ApplyBattleDamage(0, 0);
            }
            else if (enemyIndex == 2)
            {
                ApplyBattleDamage(0, 0);
            }
            else
            {
                Debug.Log($"적의 인덱스가 올바르지 않습니다. {enemyIndex}");
            }
        }
        else
        {
            Debug.Log($"플레이어의 인덱스가 올바르지 않습니다. {index}");
        }

        if (playerCharge > 0 && index == 0)
        {
            playerCharge = 0;
            DeactivateAllCharges(playerCharges); 
        }
        if (enemyCharge > 0 && enemyIndex == 0)
        {
            enemyCharge = 0;
            DeactivateAllCharges(enemyCharges);
        }

        
    }

    private IEnumerator CheckBattleEnd()
    {
        if (StatusManager.Instance.playerStatus._hp < 0)
        {
            StateManager.Instance.EndGame();
        }
        else if (StatusManager.Instance.enemyStatus._hp < 0)
        {
            yield return new WaitForSeconds(2f); // 2초 대기
            Debug.Log("have to stop every button until go to next");
            StateManager.Instance.AdvanceToNextState();

        }
    }

    #endregion

    #region Util
    private void ApplyBattleDamage(int playerD, int enemyD)
    {
        StatusManager.Instance.playerStatus.ModifyStatus("hp", -enemyD);
        StatusManager.Instance.enemyStatus.ModifyStatus("hp", -playerD);
    }

    private IEnumerator RollDiceForDuration(float duration)
    {
        float elapsedTime = 0f;
        int i = 0;
        while (elapsedTime < duration)
        {
            enemyDice.sprite = enemyDiceSprite[i % enemyDiceSprite.Length];
            i++;
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            //yield return new WaitForSeconds(0.02f);
            yield return null; // 다음 프레임까지 대기
        }
    }
    #endregion

    #region Unity LifeCycle
    private void OnEnable()
    {
        StartBattle();
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
*/