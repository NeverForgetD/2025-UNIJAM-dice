using System;
using TMPro;
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
    public TextMeshProUGUI scoreText;

    public void OnClick()
    {
        if (!isClicked)
        {
            diceSet.SetActive(true);
            statSet.SetActive(false);
            if (stat == "hp")
            {
                StatusManager.Instance.playerStatus.ModifyStatus(stat, score);
            }
            else
            {
                StatusManager.Instance.playerStatus.ChangeStatus(stat, score);
            }
            isClicked = true;
            scoreText.text = "";
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

        transform.localScale = new Vector3(1, 1, 1);
    }
}
