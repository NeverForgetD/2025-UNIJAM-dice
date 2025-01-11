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
        ResetContainers(); // 컨테이너 초기화
        AssignDiceToContainers(); // 주사위 배치
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
    /// 숫자를 랜덤하게 컨테이너에 배정하고 나머지는 비활성화.
    /// </summary>
    public void AssignDiceToContainers()
    {
        ResetContainers(); // 실행 시 컨테이너 초기화

        // 1부터 6까지의 숫자 생성
        int[] numbers = { 1, 2, 3, 4, 5, 6 };

        // 각 숫자를 랜덤하게 하나의 컨테이너에 배정
        foreach (int number in numbers)
        {
            AssignToRandomContainer(number);
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
    public void OnBattleStart()
    {
        ResetContainers(); // 컨테이너 초기화
        AssignDiceToContainers(); // 주사위 배치
    }

    public void OnPlayerMove(int index)
    {
        StartCoroutine(ExecuteBattle());

            

    }
    public IEnumerator ExecuteBattle()
    {
        yield return StartCoroutine(RollDiceForDuration(0.6f)); // 0.6초 동안 주사위를 굴림
        EnemyRoll();
    }

    public void DetermineResult(int index)
    {
        if (index == 0)
        {
            if (enemyIndex == 1)
            {
                // 적에게 데미지 -방어력
            }
            else
            {
                // 적에게 순수 데미지
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
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            //yield return new WaitForSeconds(0.02f);
            yield return null; // 다음 프레임까지 대기
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