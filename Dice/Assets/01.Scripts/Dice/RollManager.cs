using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollManager : MonoBehaviour
{
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
    public TextMeshProUGUI[] diceImages;
    public bool isRolling = false;

    public void OnRollButton()
    {
        StartCoroutine(Roll());
    }
    
    
    public IEnumerator Roll()
    {
        if (!isRolling) // 굴리는 중이 아닐 때만 실행
        {
            yield return StartCoroutine(RollDiceForDuration(2f)); // 2초 동안 주사위를 굴림
        }
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
    }

    private void RollDice()
    {
        for (int i = 0; i < diceImages.Length; i++)
        {
            int randomValue = Random.Range(1, 7); // 1부터 6까지 랜덤 숫자 생성
            diceImages[i].text = randomValue.ToString(); // UI 텍스트 업데이트
        }
    }

    private void OnEnable()
    {
        diceNum = new List<int>();
        diceCombinations = new List<string>();

        for (int i = 0; i < 5; i++)
        {
            diceNum.Add(PlayerManager.Instance.dices[i].GetEye());
        }
        for (int i = 0; i < diceNum.Count; i++)
        {
            Debug.Log("Dice: "+diceNum[i]);
        }
        diceCombinations=CheckCombinations(diceNum);
        for (int i = 0; i < diceCombinations.Count; i++)
        {
            Debug.Log(diceCombinations[i]);
        }
        Debug.Log(GetScore(diceNum, diceCombinations));
        diceNum.Clear();
        diceCombinations.Clear();
    }

    void Update()
    {
        
    }
}
