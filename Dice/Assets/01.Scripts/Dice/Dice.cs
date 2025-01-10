using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dice",
menuName = "Scriptable Object/Dice",
order = int.MaxValue)]
public class Dice : ScriptableObject
{
    public List<int> eyes;

    public void Init(){
        eyes = new List<int>(){1,2,3,4,5,6};
    }

    public int GetEye(){
        return eyes[Random.Range(0,eyes.Count)];
    }
}
