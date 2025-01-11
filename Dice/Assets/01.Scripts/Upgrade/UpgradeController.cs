using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    public List<string> options;
    #region privates
    
    private bool upgradeClicked;
    private int[] probability = { 8, 10, 12, 10, 13, 13, 12, 13, 1, 8  };
    
    #endregion

    #region Unity LifeCycle

    private void Update()
    {
        
    }

    private void OnEnable()
    {
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
        upgradeClicked = false;
        options = new List<string>();
        eyes = new List<int>();
        eyes.Clear();
        for (int i = 0; i < curDice.Length; i++)
        {
            eyes.Add(PlayerManager.Instance.dices[i].GetEyes()[i]);
            switch (eyes[i]/10)
            {
                case 0:
                    curDice[i].sprite = diceImages[eyes[i] - 1];
                    break;
                case 1:
                    curDice[i].sprite = diceImages10[eyes[i] - 11];
                    break;
                case 2:
                    curDice[i].sprite = diceImages20[eyes[i] - 21];
                    break;
                case 3:
                    curDice[i].sprite = diceImages30[eyes[i] - 31];
                    break;
                case 4:
                    curDice[i].sprite = diceImages40[eyes[i] - 41];
                    break;
                case 5:
                    curDice[i].sprite = diceImages50[eyes[i] - 51];
                    break;
            }
        }
        options.Clear();
        GenerateOption();
    }

    private void OnDisable()
    {

    }
    #endregion

    #region SerializedField

    [SerializeField] private TextMeshProUGUI[] texts;
    
    [SerializeField] TextMeshProUGUI[] combinationText;
    [SerializeField] BonusButton[] upgradeState;
    [SerializeField] public Sprite[] diceImages;
    [SerializeField] public Sprite[] diceImages10;
    [SerializeField] public Sprite[] diceImages20;
    [SerializeField] public Sprite[] diceImages30;
    [SerializeField] public Sprite[] diceImages40;
    [SerializeField] public Sprite[] diceImages50;
    [SerializeField] Image[] curDice;
    #endregion
    private List<int> eyes;

    #region Generate Options

    private void GenerateOption()
    {
        int a = GetRandomNum(probability);
        int b = GetRandomNum(probability, a);
        int c = GetRandomNum(probability, a, b);
        Debug.Log($"{a},{b},{c}");
        options.Add(GetOption(a));
        options.Add(GetOption(b));
        options.Add(GetOption(c));
        for (int i = 0; i < options.Count; i++)
        {
            switch (options[i])
            {
                case "YZ":
                    texts[i].text = "요트";
                    break;
                case "LS":
                    texts[i].text = "라지 스트레이트";
                    break;
                case "SS":
                    texts[i].text = "스몰 스트레이트";
                    break;
                case "FK":
                    texts[i].text = "포 오브 카인드";
                    break;
                case "FH":
                    texts[i].text = "풀하우스";
                    break;
                case "TP":
                    texts[i].text = "투페어";
                    break;
                case "T":
                    texts[i].text = "트리플";
                    break;
                case "P":
                    texts[i].text = "페어";
                    break;
                case "O":
                    texts[i].text = "홀짝";
                    break;
                case "N":
                    texts[i].text = "초이스";
                    break;
            }
        }
    }
    private int GetRandomNum(int[] arr, int a = -1, int b = -1)
    {
        float rand = Random.Range(0f, 100f);
        int ret = -1;
        for (int i = 0; i < arr.Length; i++)
        {
            if (rand < arr[i])
            {
                ret = i;
                break;
            }
            rand -= arr[i];
        }
        while (ret == a || ret == b)
        {
            rand = Random.Range(0f, 100f);
            for (int i = 0; i < arr.Length; i++)
            {
                if (rand < arr[i])
                {
                    ret = i;
                    break;
                }
                rand -= arr[i];
            }
        }
        return ret;
    }

    private string GetOption(int choice)
    {
        string combination = "";
        switch (choice)
        {
            case 0:
                combination = "YZ";
                break;
            case 1:
                combination = "LS";
                break;
            case 2:
                combination = "SS";
                break;
            case 3:
                combination = "FK";
                break;
            case 4:
                combination = "FH";
                break;
            case 5:
                combination = "TP";
                break;
            case 6:
                combination = "T";
                break;
            case 7:
                combination = "P";
                break;
            case 8:
                combination = "O";
                break;
            case 9:
                combination = "N";
                break;
        }
        return combination;
    }

    public void Upgrade()
    {
        if (upgradeClicked)
        {
            for (int j = 0; j < 3; j++)
            {
                if (upgradeState[j].clicked)
                {
                    PlayerManager.Instance.handsLevel[options[j]] += 1;
                }
            }
            StateManager.Instance.AdvanceToNextState();
        }
    }
    #endregion

    public void CheckUpgradeClicked()
    {
        for (int i = 0; i < 3; i++)
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

    public bool GetUpgradeClicked()
    {
        return upgradeClicked;
    }
    
    public void Skip()
    {
        StateManager.Instance.AdvanceToNextState();
    }

}
