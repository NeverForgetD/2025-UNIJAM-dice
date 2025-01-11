using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{
    #region privates
    private List<string> options;
    private bool upgradeClicked;
    private int[] probability = { 10, 8, 12, 10, 12, 13, 11, 10, 4, 10 };
    
    #endregion

    #region Unity LifeCycle

    private void Update()
    {
        
    }

    private void OnEnable()
    {
        GenerateOption();
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
    public void OnButton()
    {
        GenerateOption();
    }

    private void GenerateOption()
    {
        options = new List<string> { "", "", "" };
        int a = GetRandomNum(probability);
        int b = GetRandomNum(probability, a);
        int c = GetRandomNum(probability, a, b);
        options[0] = GetOption(a);
        options[1] = GetOption(b);
        options[2] = GetOption(c);
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
        string option = "";
        switch (choice)
        {
            case 0:
                option = "YZ";
                break;
            case 1:
                option = "LS";
                break;
            case 2:
                option = "SS";
                break;
            case 3:
                option = "FK";
                break;
            case 4:
                option = "FH";
                break;
            case 5:
                option = "TP";
                break;
            case 6:
                option = "T";
                break;
            case 7:
                option = "P";
                break;
            case 8:
                option = "O";
                break;
            case 9:
                option = "N";
                break;
        }
        return option;
    }

    public void Upgrade()
    {
        if (upgradeClicked)
        {
            for (int j = 0; j < upgradeState.Length; j++)
            {
                if (upgradeState[j].clicked)
                {
                    Debug.Log(options[j]);
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
