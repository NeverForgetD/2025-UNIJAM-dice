using System.Collections;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
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
    [SerializeField] Sprite enemyQuestionDiceSprite;

    [SerializeField] Image[] playerCharges;
    [SerializeField] Image[] enemyCharges;

    [SerializeField] Image playerSprite;
    [SerializeField] Image enemySprite;
    [SerializeField] Sprite[] playerSpriteContainer;
    [SerializeField] Sprite[] enemySpriteContainer;

    [SerializeField] GameObject playerDamage;
    [SerializeField] GameObject enemyDamage;

    [SerializeField] TextMeshProUGUI playerDamageText;
    [SerializeField] TextMeshProUGUI enemyDamageText;

    [SerializeField] Transform playerTransform;
    [SerializeField] Transform enemyTransform;
    #endregion

    #region Properties
    public int playerCharge { get; private set; }
    public int enemyCharge {  get; private set; }
    public int enemyNum {  get; private set; }
    public bool isActing {  get; private set; }
    #endregion

    #region Charge Visulaize
    private void ActivateCharges(Image[] charges, int count)
    {
        for (int i = 0; i < charges.Length; i++)
        {
            charges[i].gameObject.SetActive(i < count); // ������ ������ Ȱ��ȭ
        }
    }
    private void DeactivateAllCharges(Image[] charges)
    {
        foreach (var charge in charges)
        {
            charge.gameObject.SetActive(false); // ��� charge ��Ȱ��ȭ
        }
    }
    #endregion

    #region Enemy Dice Visualize
    /// <summary>
    /// ���ڸ� �����ϰ� �����̳ʿ� �����ϰ� �������� ��Ȱ��ȭ.
    /// </summary>
    public void ResetEnemytable()
    {
        ResetContainers(); // ���� �� �����̳� �ʱ�ȭ

        // 1���� 6������ ���� ����
        int[] numbers = { 1, 2, 3, 4, 5, 6 };

        // �� ���ڸ� �����ϰ� �ϳ��� �����̳ʿ� ����On
        foreach (int number in numbers)
        {
            AssignToRandomContainer(number);
        }
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
        int enemyDiceIndex = UnityEngine.Random.Range(0, 6);
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
    public void StartBattle()
    {
        playerCharge = 0;
        enemyCharge = 0;
        isActing = false;
        playerSprite.color = Color.white;
        enemyDice.sprite = enemyQuestionDiceSprite;

        DeactivateAllCharges(playerCharges);
        DeactivateAllCharges(enemyCharges);
        ResetEnemytable();
    }

    public void OnPlayerMove(int playerIndex) // ��ư Ŭ������ �ߵ�
    {
        if(isActing) return;
        if(playerIndex == 2 && playerCharge == 5) return;
        SoundManager.Instance.PlaySFX_RandomPitch("Dice", 0.6f, 1.4f);
        isActing = true;
        StartCoroutine(ExecuteBattle(playerIndex));
    }
    public IEnumerator ExecuteBattle(int playerIndex)
    {
        yield return StartCoroutine(RollDiceForDuration(0.6f)); // 0.6�� ���� �ֻ����� ����
        EnemyRoll(); // enemy�� Index ���� �� ��������Ʈ ����

        DetermineResult(playerIndex);
        StartCoroutine(CheckBattleEnd());
    }

    public void DetermineResult(int index) // index�� �÷��̾�
    {
        int pAtk = StatusManager.Instance.playerStatus._atk + playerCharge * StatusManager.Instance.playerStatus._pot;
        int pDef = StatusManager.Instance.playerStatus._def;

        int eAtk = StatusManager.Instance.enemyStatus._atk + enemyCharge * StatusManager.Instance.enemyStatus._pot;
        int eDef = StatusManager.Instance.enemyStatus._def;

        //pAtk -= eDef;
        //eAtk -= pAtk;

        if (index == 0) // �÷��̾� ����
        {
            SoundManager.Instance.PlaySFX_RandomPitch("Attack", 0.8f, 1.2f);
            StartCoroutine(PlayerSpriteChange(1));
            if (enemyIndex == 0)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Attack", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(1));
                ApplyBattleDamage(pAtk, eAtk);
            }
            else if (enemyIndex == 1)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Defence", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(2));
                ApplyBattleDamage(Mathf.Max(0, pAtk - eDef), 0);
                //추가
                if (pAtk < eDef)
                {
                    StartCoroutine(ActivateThenDeactivate(enemyDamage));
                    enemyDamageText.text = 0.ToString();
                }
            }
            else if (enemyIndex == 2)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Charge", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(3));
                ApplyBattleDamage(pAtk, 0);
                enemyCharge++;
                ActivateCharges(enemyCharges, enemyCharge);
            }
            else
            {
                Debug.Log($"���� �ε����� �ùٸ��� �ʽ��ϴ�. {enemyIndex}");
            }
        }
        else if (index == 1) // �÷��̾� ���
        {
            SoundManager.Instance.PlaySFX_RandomPitch("Defence", 0.8f, 1.2f);
            StartCoroutine(PlayerSpriteChange(2));
            if (enemyIndex == 0)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Attack", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(1));
                ApplyBattleDamage(0, Mathf.Max(0, eAtk -pDef));
                //추가
                if (eAtk < pDef)
                {
                    StartCoroutine(ActivateThenDeactivate(playerDamage));
                    playerDamageText.text = 0.ToString();
                }
            }
            else if (enemyIndex == 1)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Defence", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(2));
                ApplyBattleDamage(0, 0);
            }
            else if (enemyIndex == 2)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Charge", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(3));
                ApplyBattleDamage(0, 0);
                enemyCharge++;
                ActivateCharges(enemyCharges, enemyCharge);
            }
            else
            {
                Debug.Log($"���� �ε����� �ùٸ��� �ʽ��ϴ�. {enemyIndex}");
            }
        }
        else if (index == 2) // �÷��̾� ����
        {
            SoundManager.Instance.PlaySFX_RandomPitch("Charge", 0.8f, 1.2f);
            StartCoroutine(PlayerSpriteChange(3));
            playerCharge++;
            ActivateCharges(playerCharges, playerCharge);
            if (enemyIndex == 0)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Attack", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(1));
                ApplyBattleDamage(0, eAtk);
            }
            else if (enemyIndex == 1)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Defence", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(2));
                ApplyBattleDamage(0, 0);
            }
            else if (enemyIndex == 2)
            {
                SoundManager.Instance.PlaySFX_RandomPitch("Charge", 0.8f, 1.2f);
                StartCoroutine(EnemySpriteChange(3));
                ApplyBattleDamage(0, 0);
                enemyCharge++;
                ActivateCharges(enemyCharges, enemyCharge);
            }
            else
            {
                Debug.Log($"���� �ε����� �ùٸ��� �ʽ��ϴ�. {enemyIndex}");
            }
        }
        else
        {
            Debug.Log($"�÷��̾��� �ε����� �ùٸ��� �ʽ��ϴ�. {index}");
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

    private IEnumerator PlayerSpriteChange(int spriteType){
        playerSprite.sprite = playerSpriteContainer[spriteType];
        playerTransform.GetChild(spriteType - 1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        playerTransform.GetChild(spriteType - 1).gameObject.SetActive(false);
        isActing = false;

        if(StatusManager.Instance.playerStatus._hp < 0)
            playerSprite.color = Color.clear;
        playerSprite.sprite = playerSpriteContainer[0];
        

    }

    private IEnumerator EnemySpriteChange(int spriteType){
        enemySprite.sprite = enemySpriteContainer[enemyNum * 4 + spriteType];
        enemyTransform.GetChild(spriteType - 1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        enemyTransform.GetChild(spriteType - 1).gameObject.SetActive(false);

        if(StatusManager.Instance.enemyStatus._hp < 0){
            enemySprite.color = Color.clear;
            enemyNum = (enemyNum + 1) % 3;
            isActing = true;
        }
        else{
            isActing = false;
            ResetEnemytable();
        }
  
        enemySprite.sprite = enemySpriteContainer[enemyNum * 4];
        
    }
    private IEnumerator CheckBattleEnd()
    {
        if (StatusManager.Instance.playerStatus._hp < 0)
        {
            StateManager.Instance.EndGame();
        }
        else if (StatusManager.Instance.enemyStatus._hp < 0)
        {
            yield return new WaitForSeconds(2f); // 2�� ���

            StateManager.Instance.AdvanceToNextState();
        }
    }

    #endregion

    #region Util
    private void ApplyBattleDamage(int playerD, int enemyD)
    {
        StatusManager.Instance.playerStatus.ModifyStatus("hp", -enemyD);
        StatusManager.Instance.enemyStatus.ModifyStatus("hp", -playerD);

        if (enemyD > 0)
        {
            StartCoroutine(Shake(playerSprite));
            StartCoroutine(ActivateThenDeactivate(playerDamage));
            playerDamageText.text = enemyD.ToString();
        }
        if (playerD > 0)
        {
            StartCoroutine(Shake(enemySprite));
            StartCoroutine(ActivateThenDeactivate(enemyDamage));
            enemyDamageText.text = playerD.ToString();
        }
        
    }

    private System.Collections.IEnumerator ActivateThenDeactivate(GameObject targetObject, float duration = 0.8f)
    {
        if (targetObject == null) yield break;

        targetObject.SetActive(true); // 활성화
        yield return new WaitForSeconds(duration); // 지정된 시간 대기
        targetObject.SetActive(false); // 비활성화
    }

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

    private IEnumerator Shake(Image targetImage, float duration = 0.8f, int shakeMagnitude = 10)
    {
        Vector3 originalPosition = targetImage.rectTransform.localPosition;
        float elapsedTime = 0f;
        bool moveRight = true; // 방향 제어 변수

        while (elapsedTime < duration)
        {
            // 왼쪽 또는 오른쪽으로 이동
            float offsetX = moveRight ? shakeMagnitude : -shakeMagnitude;
            targetImage.rectTransform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z);

            // 방향 전환
            moveRight = !moveRight;

            elapsedTime += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        // 흔들림 종료 후 원래 위치로 복귀
        targetImage.rectTransform.localPosition = originalPosition;
    }

    /*
    private IEnumerator ShakeMad(Image targetImage, float duration, int shakeMagnitude)
    {
        Vector3 originalPosition = targetImage.rectTransform.localPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // X축으로 흔들림 계산
            float offsetX = Random.Range(-shakeMagnitude, shakeMagnitude);
            targetImage.rectTransform.localPosition = new Vector3(originalPosition.x + offsetX, originalPosition.y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 흔들림 종료 후 원래 위치로 복귀
        targetImage.rectTransform.localPosition = originalPosition;
    }
    */
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