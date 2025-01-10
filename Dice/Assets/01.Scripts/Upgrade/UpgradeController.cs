using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour
{

    #region Upgrade Type
    private enum UpgradeType
    {
        Dice,
        Bonus,
    }
    [SerializeField] private UpgradeType upgradeType;
    #endregion

    #region privates
    DiceGenerator generator;
    #endregion

    #region Unity LifeCycle
    private void OnEnable()
    {
        if (upgradeType == UpgradeType.Dice)
        {
            if (generator == null)
            {
                //generator = new DiceGenerator();
            }
        }
    }

    private void OnDisable()
    {

    }
    #endregion

    #region SerializedField
    [SerializeField] private Image optionImage1;
    [SerializeField] private Image optionImage2;
    [SerializeField] private Image optionImage3;
    #endregion

    #region Generate Options

    public TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    public TextMeshProUGUI text3;

    public void OnButtonDice()
    {
        GenerateDiceOption();
        GenerateDiceOption2();
        GenerateDiceOption3();
    }
    public void GenerateDiceOption()
    {
        Dice newDice = new Dice();
        newDice.Init();

        text.text = $"{newDice.eyes[0]}, {newDice.eyes[1]}, {newDice.eyes[2]}, {newDice.eyes[3]}, {newDice.eyes[4]}, {newDice.eyes[5]}";
    }

    public void GenerateDiceOption2()
    {
        Dice newDice = new Dice();
        newDice.Init();

        text2.text = $"{newDice.eyes[0]}, {newDice.eyes[1]}, {newDice.eyes[2]}, {newDice.eyes[3]}, {newDice.eyes[4]}, {newDice.eyes[5]}";
    }
    public void GenerateDiceOption3()
    {
        Dice newDice = new Dice();
        newDice.Init();

        text3.text = $"{newDice.eyes[0]}, {newDice.eyes[1]}, {newDice.eyes[2]}, {newDice.eyes[3]}, {newDice.eyes[4]}, {newDice.eyes[5]}";
    }
    #endregion



}
