using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceButton : MonoBehaviour
{
    public bool clicked;
    public Image image;
    public int rollCount;
    public RollManager manager;
    public List<int> eyes;
    public Type type;
    public GameObject infoSet;
    public DiceInfoController info;
    void OnEnable()
    {
        manager = GameObject.Find("RollManager").GetComponent<RollManager>();
        image = gameObject.GetComponent<Image>();
        clicked = false;
        image.color = new Color(1, 1, 1, 1);
        infoSet.SetActive(false);
    }
    // Update is called once per frame
    private void Update()
    {
        rollCount = manager.rollCount;
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
        if (rollCount >= 1)
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
