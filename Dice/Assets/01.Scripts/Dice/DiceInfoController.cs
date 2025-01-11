using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceInfoController : MonoBehaviour
{
    public Image[] diceEyes;
    public Sprite[] diceNums;
    public Sprite[] diceNums10;
    public Sprite[] diceNums20;
    public Sprite[] diceNums30;
    public Sprite[] diceNums40;
    public Sprite[] diceNums50;

    public void Show(List<int> eyes)
    {
        for (int i = 0; i < diceEyes.Length; i++)
        {
            diceEyes[i].sprite = diceNums[eyes[i]-1];
        }
    }
}
