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
        eyes.Sort();
        type = DiceGenerator.type;
    }

    

    public int GetEye(){
        return eyes[Random.Range(0,eyes.Count)];
    }

    public List<int> GetEyes()
    {
        return eyes;
    }

    public Type GetType()
    {
        return type;
    }
}
