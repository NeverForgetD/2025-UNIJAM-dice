using System;
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

        anyClicked = false;
        upgradeClicked = false;
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

    [SerializeField] TextMeshProUGUI[] texts;
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
    #endregion

    #region Generate Options
    public void OnButtonDice()
    {
        GenerateDiceOption();
    }

    private void GenerateDiceOption()
    {
        diceOptions = new Dice[3];
        for (int i = 0; i < 3; i++)
        {
            diceOptions[i] = new Dice();
            diceOptions[i].Init(StateManager.Instance.Round);
            texts[i].text = $"{diceOptions[i].eyes[0]}, {diceOptions[i].eyes[1]}, {diceOptions[i].eyes[2]}, {diceOptions[i].eyes[3]}, {diceOptions[i].eyes[4]}, {diceOptions[i].eyes[5]}";
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
        for (int i = 0; i < texts.Length; i++)
        {
            if (upgradeState[i].clicked)
            {
                upgradeClicked = true;
                break;
            }
            else
            {
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
