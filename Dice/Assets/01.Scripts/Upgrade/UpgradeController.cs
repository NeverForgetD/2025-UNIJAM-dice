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

    #endregion

    #region Unity LifeCycle
    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    #region SerializedField
    [SerializeField] private Image optionImage1;
    [SerializeField] private Image optionImage2;
    [SerializeField] private Image optionImage3;
    #endregion




    #endregion



}
