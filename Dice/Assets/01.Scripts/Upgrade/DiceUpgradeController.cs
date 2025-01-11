using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class DiceUpgradeController : MonoBehaviour
{
    #region privates
    Dice[] diceOptions;
    private bool anyClicked;
    private bool upgradeClicked;
    
    #endregion

    #region Unity LifeCycle

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        GenerateDiceOption();
        for (int i = 0; i < curDice.Length; i++)
        {
            diceState[i].eyes = PlayerManager.Instance.dices[i].GetEyes();
            curDice[i].sprite = diceImages[diceState[i].eyes[0] - 1];
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
        anyClicked = false;
        upgradeClicked = false;
        for (int i = 0; i < diceOptions.Length; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (i == 0)
                {
                    
                }else if (i == 1)
                {
                    
                }
                else
                {
                    
                }
            }
        }
    }

    private void OnDisable()
    {

    }
    #endregion

    #region SerializedField
    //[SerializeField] private Image optionImage1;
    //[SerializeField] private Image optionImage2;
    //[SerializeField] private Image optionImage3;

    //[SerializeField] private TextMeshProUGUI text1;
    //[SerializeField] private TextMeshProUGUI text2;
    //[SerializeField] private TextMeshProUGUI text3;
    
    [SerializeField] TextMeshProUGUI[] combinationText;
    [SerializeField] public Sprite[] diceImages;
    [SerializeField] public Sprite[] diceImages10;
    [SerializeField] public Sprite[] diceImages20;
    [SerializeField] public Sprite[] diceImages30;
    [SerializeField] public Sprite[] diceImages40;
    [SerializeField] public Sprite[] diceImages50;
    [SerializeField] Image[] curDice;
    [SerializeField] DiceButtonUpgrade[] diceState;
    [SerializeField] UpdrageButton[] upgradeState;
    [SerializeField] Image[] diceOptionImage1;
    [SerializeField] Image[] diceOptionImage2;
    [SerializeField] Image[] diceOptionImage3;
    #endregion

    #region Generate Options
    public void OnButtonDice()
    {
        GenerateDiceOption();
    }

    private void GenerateDiceOption()
    {
        List<int> eyes=new List<int>();
        diceOptions = new Dice[3];
        for (int i = 0; i < 3; i++)
        {
            diceOptions[i] = new Dice();
            diceOptions[i].Init(StateManager.Instance.Round);
            eyes = diceOptions[i].GetEyes();
            for (int j = 0; j < 6; j++)
            {
                if (i == 0)
                {
                    diceOptionImage1[j].sprite = diceImages[eyes[j] - 1];
                }else if (i == 1)
                {
                    diceOptionImage2[j].sprite = diceImages[eyes[j] - 1];
                }
                else
                {
                    diceOptionImage3[j].sprite = diceImages[eyes[j] - 1];
                }
            }
        }
    }

    private void Change()
    {
        if (anyClicked && upgradeClicked)
        {
            for (int i = 0; i < diceState.Length; i++)
            {
                if (diceState[i].clicked)
                {
                    for (int j = 0; j < upgradeState.Length; j++)
                    {
                        if (upgradeState[j].clicked)
                        {
                            PlayerManager.Instance.dices[i] = diceOptions[j];
                        }
                    }
                }
            }
            StateManager.Instance.AdvanceToNextState();
        }
    }
    #endregion

    public void CheckClicked()
    {
        for (int i = 0; i < diceState.Length; i++)
        {
            if (diceState[i].clicked)
            {
                anyClicked = true;
                break;
            }
            else
            {
                anyClicked = false;
            }
        }
    }

    public void CheckUpgradeClicked()
    {
        for (int i = 0; i < diceOptions.Length; i++)
        {
            if (upgradeState[i].clicked)
            {
                upgradeClicked = true;
                for (int j = 0; j < diceImages.Length; j++)
                {
                    if (i == 0)
                    {
                        diceOptionImage1[j].color=new Color(1, 1, 1, 0.5f);
                    }
                    else if (i == 1)
                    {
                        diceOptionImage2[j].color=new Color(1, 1, 1, 0.5f);
                    }
                    else
                    {
                        diceOptionImage3[j].color=new Color(1, 1, 1, 0.5f);
                    }
                }
                break;
            }
            else
            {
                for (int j = 0; j < diceImages.Length; j++)
                {
                    if (i == 0)
                    {
                        diceOptionImage1[j].color=new Color(1, 1, 1);
                    }
                    else if (i == 1)
                    {
                        diceOptionImage2[j].color=new Color(1, 1, 1);
                    }
                    else
                    {
                        diceOptionImage3[j].color=new Color(1, 1, 1);
                    }
                }
                upgradeClicked = false;
            }
        }
    }

    public bool GetClicked()
    {
        return anyClicked;
    }

    public bool GetUpgradeClicked()
    {
        return upgradeClicked;
    }
    
    public void Skip()
    {
        StateManager.Instance.AdvanceToNextState();
    }

}
