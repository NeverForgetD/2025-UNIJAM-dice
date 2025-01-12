using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BonusButton : MonoBehaviour
{
    public UpgradeController manager;
    public bool clicked;
    public bool upgradeClicked;
    public Image image;
    public TextMeshProUGUI text;
    
    void OnEnable()
    {
        image = gameObject.GetComponent<Image>();
        clicked = false;
        upgradeClicked = false;
        image.color = new Color(1, 1, 1, 1);
        text.color = new Color(1, 1, 1, 1);
    }
    
    private void Update()
    {
        upgradeClicked = manager.GetUpgradeClicked();
        if (clicked)
        {
            image.color = new Color(1, 1, 1, 0.5f);
            text.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            image.color = new Color(1, 1, 1, 1);
            text.color = new Color(1, 1, 1, 1);
        }
    }
    
    public void OnClick()
    {
        if (!upgradeClicked)
        {
            if (clicked)
            {
                clicked = false;
                image.color = new Color(1, 1, 1, 1);
                text.color = new Color(1, 1, 1, 1);
            }
            else
            {
                clicked = true;
                image.color = new Color(1, 1, 1, 0.5f);
                text.color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            if (clicked)
            {
                clicked = false;
            }
        }
        manager.CheckUpgradeClicked();
    }
}
