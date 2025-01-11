using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class RollManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI rollBtnText;

    public List<string> CheckCombinations(List<int> dice)
    {
        for (int i = 0; i < dice.Count; i++)
        {
            dice[i] %= 10;
        }
        List<string> combinations = new List<string>();
        var counts = dice.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
        var countValues = counts.Values.OrderByDescending(v => v).ToList();
        var uniqueNumbers = counts.Keys.OrderBy(x => x).ToList();
        if(countValues.SequenceEqual(new List<int>{5}))
            combinations.Add("YZ");
        else if (uniqueNumbers.SequenceEqual(new List<int> { 1, 2, 3, 4, 5 }) ||
                  uniqueNumbers.SequenceEqual(new List<int> { 2, 3, 4, 5, 6 }))
        {
            combinations.Add("LS");
        }
        else if(uniqueNumbers.SequenceEqual(new List<int>{1,2,3,4}) ||
                uniqueNumbers.SequenceEqual(new List<int>{2,3,4,5}) ||
                uniqueNumbers.SequenceEqual(new List<int>{3,4,5,6}))
            combinations.Add("SS");
        else if(countValues.SequenceEqual(new List<int>{4,1}))
            combinations.Add("FK");
        else if(countValues.SequenceEqual(new List<int>{3,2}))
            combinations.Add("FH");
        
        else if(countValues.SequenceEqual(new List<int>{2,2,1}))
            combinations.Add("TP");
        else if(countValues.SequenceEqual(new List<int>{3,1,1}))
            combinations.Add("T");
        else if (countValues.SequenceEqual(new List<int> { 2, 1, 1, 1 }))
        {
            combinations.Add("P");
        }
        if(dice.All(x=>x%2!=0))
            combinations.Add("O");
        if(dice.All(x=>x%2==0))
            combinations.Add("E");
        if(combinations==null||combinations.Count==0)
            combinations.Add("N");
        return combinations;
    }

    public int GetScore(List<int> dice, List<string> combinations)
    {
        int score = 0;
        int sum = 0;
        for (int i = 0; i < dice.Count; i++)
            score += dice[i];
        sum = score;
        if (combinations.Contains("YZ"))
            score *= PlayerManager.Instance.handsLevel["YZ"];
        else if (combinations.Contains("LS"))
            score *= PlayerManager.Instance.handsLevel["LS"];
        else if (combinations.Contains("SS"))
            score *= PlayerManager.Instance.handsLevel["SS"];
        else if (combinations.Contains("FK"))
            score *= PlayerManager.Instance.handsLevel["FK"];
        else if (combinations.Contains("FH"))
            score *= PlayerManager.Instance.handsLevel["FH"];
        else if (combinations.Contains("TP"))
            score *= PlayerManager.Instance.handsLevel["TP"];
        else if (combinations.Contains("T"))
            score *= PlayerManager.Instance.handsLevel["T"];
        else if (combinations.Contains("P"))
            score *= PlayerManager.Instance.handsLevel["P"];
        else if (combinations.Contains("N"))
            score *= PlayerManager.Instance.handsLevel["N"];
        scoreText.text = sum.ToString() + " x " + ((float)score / sum).ToString();
        if (combinations.Contains("O") || combinations.Contains("E"))
        {
            score *= PlayerManager.Instance.handsLevel["O"];
            scoreText.text += " x 2";
        }
        scoreText.text += " = " + score.ToString();
        return score;
    }

    public void ShowCombination(List<string> combinations)
    {
        if (combinations.Contains("YZ"))
        {
            combinationImage[0].enabled = true;
        }
        else if (combinations.Contains("LS"))
        {
            combinationImage[1].enabled = true;
        }
        else if (combinations.Contains("SS"))
        {
            combinationImage[2].enabled = true;
        }
        else if (combinations.Contains("FK"))
        {
            combinationImage[3].enabled = true;
        }
        else if (combinations.Contains("FH"))
        {
            combinationImage[4].enabled = true;
        }
        else if (combinations.Contains("TP"))
        {
            combinationImage[5].enabled = true;
        }
        else if (combinations.Contains("T"))
        {
            combinationImage[6].enabled = true;
        }
        else if (combinations.Contains("P"))
        {
            combinationImage[7].enabled = true;
        }
        else if (combinations.Contains("N"))
        {
            combinationImage[9].enabled = true;
        }

        if (combinations.Contains("O")||combinations.Contains("E"))
        {
            combinationImage[8].enabled = true;
        }
    }

    public TextMeshProUGUI[] combinationText;
    public Image[] combinationImage;
    public List<int> diceNum;
    public List<int> actualDiceNum;
    private List<string> diceCombinations;
    public Sprite randomDice;
    public Sprite[] diceImages;
    public Sprite[] diceImages10;
    public Sprite[] diceImages20;
    public Sprite[] diceImages30;
    public Sprite[] diceImages40;
    public Sprite[] diceImages50;
    public Image[] curDice;
    public DiceButton[] diceState;
    public bool isRolling;
    public int rollCount;
    public bool allClicked;
    public TextMeshProUGUI scoreText;
    public GameObject diceControlSet;
    public GameObject statControlSet;
    public int score;
    public StatButton[] statButtons;
    

    public void CheckAllClicked()
    {
        for (int i = 0; i < curDice.Length; i++)
        {
            if (!diceState[i].clicked)
            {
                allClicked = false;
                break;
            }
            else
            {
                allClicked = true;
            }
        }
    }
    public void OnRollButton()
    {
        CheckAllClicked();
        if (!isRolling && rollCount<3 && !allClicked) // can Roll
        {
            for (int i = 0; i < combinationImage.Length; i++)
            {
                combinationImage[i].enabled = false;
            }
            SoundManager.Instance.PlaySFX_RandomPitch("DiceRoll01", 0.6f, 1.4f);
            SetDice();
            rollCount += 1;
            rollBtnText.text = $"Roll {3-rollCount} / 3";
        }
    }

    public void SetDice()
    {
        if (rollCount == 0)
        {
            for (int i = 0; i < 5; i++)
            {
                diceNum.Add(PlayerManager.Instance.dices[i].GetEye());
            }
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (!diceState[i].clicked)
                {
                    diceNum[i]=PlayerManager.Instance.dices[i].GetEye();
                }
            }
        }
        actualDiceNum = diceNum;
        diceCombinations=CheckCombinations(diceNum);
        StartCoroutine(Roll());
    }
    
    public IEnumerator Roll()
    {
        yield return StartCoroutine(RollDiceForDuration(0.6f)); // 0.6초 동안 주사위를 굴림
    }
    
    private IEnumerator RollDiceForDuration(float duration)
    {
        isRolling = true; // 굴리기 시작
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            RollDice(); // 주사위 숫자 갱신
            elapsedTime += Time.deltaTime; // 경과 시간 업데이트
            yield return null; // 다음 프레임까지 대기
        }

        isRolling = false; // 굴리기 종료
        for (int i = 0; i < curDice.Length; i++)
        {
            curDice[i].sprite = diceImages[diceNum[i] - 1];
        }
        score = GetScore(actualDiceNum, diceCombinations);
        ShowCombination(diceCombinations);
    }

    private void RollDice()
    {
        for (int i = 0; i < curDice.Length; i++)
        {
            if (!diceState[i].clicked)
            {
                int rand = Random.Range(0, diceImages.Length);
                curDice[i].sprite = diceImages[rand];
            }
        }
    }

    public void Choose()
    {
        if (!isRolling && rollCount>0)
        {
            rollCount = 0;
            rollBtnText.text = "Roll 3 / 3";
            Init();
            diceControlSet.SetActive(false);
            statControlSet.SetActive(true);
            for (int i = 0; i < statButtons.Length; i++)
            {
                statButtons[i].score = score;
            }
        }
    }
    
    private void OnEnable()
    {
        for (int i = 0; i < curDice.Length; i++)
        {
            diceState[i].eyes = PlayerManager.Instance.dices[i].GetEyes();
            diceState[i].type = PlayerManager.Instance.dices[i].GetType();
        }
        combinationText[0].text="x"+PlayerManager.Instance.handsLevel["YZ"].ToString();
        combinationText[1].text="x"+PlayerManager.Instance.handsLevel["LS"].ToString();
        combinationText[2].text="x"+PlayerManager.Instance.handsLevel["SS"].ToString();
        combinationText[3].text="x"+PlayerManager.Instance.handsLevel["FK"].ToString();
        combinationText[4].text="x"+PlayerManager.Instance.handsLevel["FH"].ToString();
        combinationText[5].text="x"+PlayerManager.Instance.handsLevel["TP"].ToString();
        combinationText[6].text="x"+PlayerManager.Instance.handsLevel["T"].ToString();
        combinationText[7].text="x"+PlayerManager.Instance.handsLevel["P"].ToString();
        combinationText[8].text="x"+PlayerManager.Instance.handsLevel["O"].ToString();
        combinationText[9].text="x"+PlayerManager.Instance.handsLevel["N"].ToString();
        for (int i = 0; i < statButtons.Length; i++)
        {
            statButtons[i].isClicked = false;
        }
        scoreText.text = "";
        Init();
    }

    void Init()
    {
        allClicked = false;
        rollCount=0;
        isRolling = false;
        diceNum = new List<int>();
        diceCombinations = new List<string>();
        diceCombinations.Clear();
        diceNum.Clear();
        for (int i = 0; i < curDice.Length; i++)
        {
            curDice[i].sprite = randomDice;
        }
        rollBtnText.text = "Roll 3 / 3";
        for (int i = 0; i < combinationImage.Length; i++)
        {
            combinationImage[i].enabled = false;
        }
        diceControlSet.SetActive(true);
        statControlSet.SetActive(false);
    }
    

    void Update()
    {
        for (int i = 0; i < curDice.Length; i++)
        {
            diceState[i].eyes = PlayerManager.Instance.dices[i].GetEyes();
            diceState[i].type = PlayerManager.Instance.dices[i].GetType();
        }
    }
}
