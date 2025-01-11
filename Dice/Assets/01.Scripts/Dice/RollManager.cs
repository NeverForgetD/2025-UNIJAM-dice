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
        for (int i = 0; i < dice.Count; i++)
            score += dice[i];
        //배율은 나중에 수정
        if (combinations.Contains("YZ"))
            score *= 7;
        else if (combinations.Contains("LS"))
            score *= 6;
        else if (combinations.Contains("SS"))
            score *= 5;
        else if (combinations.Contains("FK"))
            score *= 5;
        else if (combinations.Contains("FH"))
            score *= 4;
        else if (combinations.Contains("TP"))
            score *= 3;
        else if (combinations.Contains("T"))
            score *= 3;
        else if (combinations.Contains("P"))
            score *= 2;
        else if (combinations.Contains("N"))
            score *= 1;
        if (combinations.Contains("O"))
            score *= 2;
        if (combinations.Contains("E"))
            score *= 2;
        return score;
    }

    public List<int> diceNum;
    private int[] dice = {1, 2, 3, 4, 5, 6};
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
        diceCombinations=CheckCombinations(diceNum);
        StartCoroutine(Roll());
    }
    
    public IEnumerator Roll()
    {
        SoundManager.Instance.PlaySFX_RandomPitch("DiceRoll01", 0.8f, 1.3f);
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
    
    private void OnEnable()
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
            diceState[i].eyes = PlayerManager.Instance.dices[i].GetEyes();
            diceState[i].type = PlayerManager.Instance.dices[i].GetType();
            curDice[i].sprite = randomDice;
        }
        rollBtnText.text = "Roll 3 / 3";
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
