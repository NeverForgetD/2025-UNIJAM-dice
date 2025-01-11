using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceButtonUpgrade : MonoBehaviour
{
    public DiceUpgradeController manager;
    public bool buttonClicked;
    public bool clicked;
    public Image image;
    public List<int> eyes;
    public GameObject infoSet;
    public DiceInfoController info;
    
    void OnEnable()
    {
        image = gameObject.GetComponent<Image>();
        clicked = false;
        buttonClicked = false;
        image.color = new Color(1, 1, 1, 1);
        infoSet.SetActive(false);
    }

    private void Update()
    {
        buttonClicked = manager.GetClicked();
        if (clicked)
        {
            image.color = new Color(1, 1, 1, 0.5f);
        }
        else
        {
            image.color = new Color(1, 1, 1, 1);
        }
    }
    
    public void OnClick()
    {
        if (!buttonClicked)
        {
            if (clicked)
            {
                clicked = false;
                image.color = new Color(1, 1, 1, 1);
            }
            else
            {
                clicked = true;
                image.color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            if (clicked)
            {
                clicked = false;
            }
        }
        manager.CheckClicked();
    }
    
    public void OnPointerEnter()
    {
        infoSet.SetActive(true);
        info.Show(eyes);
    }

    public void OnPointerExit()
    {
        infoSet.SetActive(false);
    }
}
