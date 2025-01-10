using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class DiceController : MonoBehaviour
{
    public int GetDiceNum(int[] diceNums)
    {
        int temp=Random.Range(0, 6);
        return diceNums[temp];
    }

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
            combinations.Add("Yahtzee");
        else if (uniqueNumbers.SequenceEqual(new List<int> { 1, 2, 3, 4, 5 }) ||
                  uniqueNumbers.SequenceEqual(new List<int> { 2, 3, 4, 5, 6 }))
        {
            combinations.Add("Large Straight");
        }
        else if(uniqueNumbers.SequenceEqual(new List<int>{1,2,3,4}) ||
                uniqueNumbers.SequenceEqual(new List<int>{2,3,4,5}) ||
                uniqueNumbers.SequenceEqual(new List<int>{3,4,5,6}))
            combinations.Add("Small Straight");
        else if(countValues.SequenceEqual(new List<int>{4,1}))
            combinations.Add("Four of a Kind");
        else if(countValues.SequenceEqual(new List<int>{3,2}))
            combinations.Add("Full House");
        
        else if(countValues.SequenceEqual(new List<int>{2,2,1}))
            combinations.Add("Two Pair");
        else if(countValues.SequenceEqual(new List<int>{3,1,1}))
            combinations.Add("Triple");
        else if (countValues.SequenceEqual(new List<int> { 2, 1, 1, 1 }))
        {
            combinations.Add("Pair");
        }
        if(dice.All(x=>x%2!=0))
            combinations.Add("Odd");
        if(dice.All(x=>x%2==0))
            combinations.Add("Even");
        if(combinations==null||combinations.Count==0)
            combinations.Add("None");
        return combinations;
    }

    public int GetScore(List<int> dice, List<string> combinations)
    {
        int score = 0;
        for (int i = 0; i < dice.Count; i++)
            score += dice[i];
        //배율은 나중에 수정
        if (combinations.Contains("Yahtzee"))
            score *= 7;
        else if (combinations.Contains("Large Straight"))
            score *= 6;
        else if (combinations.Contains("Small Straight"))
            score *= 5;
        else if (combinations.Contains("Four of a Kind"))
            score *= 5;
        else if (combinations.Contains("Full House"))
            score *= 4;
        else if (combinations.Contains("Two Pair"))
            score *= 3;
        else if (combinations.Contains("Triple"))
            score *= 3;
        else if (combinations.Contains("Pair"))
            score *= 2;
        else if (combinations.Contains("None"))
            score *= 1;
        if (combinations.Contains("Odd"))
            score *= 2;
        if (combinations.Contains("Even"))
            score *= 2;
        return score;
    }

    public List<int> diceNum;
    private int[] dice = {1, 2, 3, 4, 5, 6};
    private List<string> diceCombinations;

    private void Start()
    {
        diceNum = new List<int>();
        diceCombinations = new List<string>();
    }

    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            diceNum.Add(GetDiceNum(dice));
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
}
