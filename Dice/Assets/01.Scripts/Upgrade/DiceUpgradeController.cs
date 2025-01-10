using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceUpgradeController : MonoBehaviour
{
    #region privates
    Dice[] diceOptions;
    #endregion

    #region Unity LifeCycle
    private void OnEnable()
    {
        GenerateDiceOption();
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

    private void OnDiceOptionSelected()
    {
        // 선택된 버튼에 따라 해당 dice 덱에 추가
    }
    #endregion

}
