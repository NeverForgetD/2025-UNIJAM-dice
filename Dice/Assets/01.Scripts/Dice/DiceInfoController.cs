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
            switch (eyes[0]/10)
            {
                case 0:
                    diceEyes[i].sprite = diceNums[eyes[i]-1];
                    break;
                case 1:
                    diceEyes[i].sprite = diceNums10[eyes[i] - 11];
                    break;
                case 2:
                    diceEyes[i].sprite = diceNums20[eyes[i] - 21];
                    break;
                case 3:
                    diceEyes[i].sprite = diceNums30[eyes[i] - 31];
                    break;
                case 4:
                    diceEyes[i].sprite = diceNums40[eyes[i] - 41];
                    break;
                case 5:
                    diceEyes[i].sprite = diceNums50[eyes[i] - 51];
                    break;
            }
            
        }
    }
}
