using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton Init
    public static PlayerManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }
    #endregion


    #region Properites
    
    public List<Dice> dices;
    public Dictionary<string, int> handsLevel;

    public void Init(){
        dices = new List<Dice>();
        for(int i=0;i<5;i++) {
            dices.Add(new Dice());
            dices[i].Init();
        }

        handsLevel = new Dictionary<string, int>(){{"YZ", 1}, 
        {"LS", 1}, {"SS", 1}, {"FK", 1}, 
        {"FH", 1}, {"TP", 1}, {"T", 1}, 
        {"P", 1}, {"O", 1}, {"E", 1}, };
    }


    #endregion
}
