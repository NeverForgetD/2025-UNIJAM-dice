using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    public List<string> options;
    #region privates
    
    private bool upgradeClicked;
    private int[] probability = { 10, 8, 12, 10, 12, 13, 11, 10, 4, 10 };
    
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
    #endregion

    #region Generate Options

    private void GenerateOption()
    {
        int a = GetRandomNum(probability);
        int b = GetRandomNum(probability, a);
        int c = GetRandomNum(probability, a, b);
        options.Add(GetOption(a));
        options.Add(GetOption(b));
        options.Add(GetOption(c));
        for (int i = 0; i < options.Count; i++)
        {
            switch (options[i])
            {
                case "YZ":
                    texts[i].text = "��Ʈ";
                    break;
                case "LS":
                    texts[i].text = "���� ��Ʈ����Ʈ";
                    break;
                case "SS":
                    texts[i].text = "���� ��Ʈ����Ʈ";
                    break;
                case "FK":
                    texts[i].text = "�� ���� ī�ε�";
                    break;
                case "FH":
                    texts[i].text = "Ǯ�Ͽ콺";
                    break;
                case "TP":
                    texts[i].text = "�����";
                    break;
                case "T":
                    texts[i].text = "Ʈ����";
                    break;
                case "P":
                    texts[i].text = "���";
                    break;
                case "O":
                    texts[i].text = "Ȧ¦";
                    break;
                case "N":
                    texts[i].text = "���̽�";
                    break;
            }
        }
    }
    private int GetRandomNum(int[] arr, int a = 0, int b = 0){
        int ret = 0;
        do {
            float rand = Random.Range(0f,100f);
            for(int i=0;i<10;i++){
                if(rand < arr[i]) {
                    ret = i + 1;
                    break;
                }
                rand -= arr[i];
            }
        } while (ret==a || ret==b);
        return ret-1;
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
