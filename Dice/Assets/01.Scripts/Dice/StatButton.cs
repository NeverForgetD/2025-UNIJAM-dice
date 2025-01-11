using System;
using UnityEngine;
using UnityEngine.UI;

public class StatButton : MonoBehaviour
{
    public string stat;
    public int score;
    public bool isClicked;
    public Image image;
    public UIButtonEffect effect;
    public GameObject diceSet;
    public GameObject statSet;

    public void OnClick()
    {
        if (!isClicked)
        {
            diceSet.SetActive(true);
            statSet.SetActive(false);
            PlayerManager.Instance.playerStat.UpdateStatus(stat, score);
            PlayerManager.Instance.playerStat.PrintStatus(stat);
            isClicked = true;
        }
    }

    void OnEnable()
    {
        if (isClicked)
        {
            effect.enabled = false;
            image.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else
        {
            effect.enabled = true;
            image.color = new Color(1, 1, 1);
        }
    }
}
