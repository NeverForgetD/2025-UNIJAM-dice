using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;



public class Dice
{
    

    public List<int> eyes;
    public Type type;

    public void Init(int round = 0){
        eyes = DiceGenerator.Generate();
        type = DiceGenerator.type;

        Debug.Log(eyes[0].ToString()+
        eyes[1].ToString()+
        eyes[2].ToString()+
        eyes[3].ToString()+
        eyes[4].ToString()+
        eyes[5].ToString());
    }

    

    public int GetEye(){
        return eyes[Random.Range(0,eyes.Count)];
    }
}
