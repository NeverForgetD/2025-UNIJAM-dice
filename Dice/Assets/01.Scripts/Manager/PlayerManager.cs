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
        
        handsLevel = new Dictionary<string, int>(){{"YZ", 10}, 
        {"LS", 7}, {"SS", 4}, {"FK", 5}, 
        {"FH", 4}, {"TP", 3}, {"T", 3}, 
        {"P", 2}, {"O", 2}, {"N", 1}};
    }

    #endregion
}