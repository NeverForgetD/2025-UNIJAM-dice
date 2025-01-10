using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

enum Type{
    D111111,D222,D33,D24,D15,D123,D6
}
public class DiceGenerator
{
    private static readonly int[] D222 = {25,25,25,10,8,7};

    private static readonly int[] D33_1 = {5,5,10,35,25,20};
    private static readonly int[] D33_2 = {20,25,35,10,5,5};

    private static readonly int[] D24_1 = {5,10,15,20,30,20};
    private static readonly int[] D24_2 = {20,20,30,15,10,5};

    private static readonly int[] D15_1 = {5,10,15,20,20,30};
    private static readonly int[] D15_2 = {30,20,20,15,10,5};

    private static readonly int[] D123_1 = {5,5,5,20,25,40};
    private static readonly int[] D123_2 = {5,15,20,30,20,10};
    private static readonly int[] D123_3 = {30,20,20,15,10,5};

    private static readonly int[] D6 = {30,20,20,15,10,5};

    public static List<int> eyes;

    public static List<int> Generate(int round = 0){
        Type type = (Type)Random.Range(0, 7);
        switch(type){
            case Type.D111111: InitD111111(); break;
            case Type.D222: InitD222(); break;
            case Type.D33: InitD33(); break;
            case Type.D24: InitD24(); break;
            case Type.D15: InitD15(); break;
            case Type.D123: InitD123(); break;
            case Type.D6: InitD6(); break;
        }

        return eyes;
    }

    private static void InitD111111(){
        eyes = new List<int>(){1,2,3,4,5,6};
    }

    private static void InitD222(){
        int a = GetRandomEye(D222);
        int b = GetRandomEye(D222, a);
        int c = GetRandomEye(D222, a, b);

        eyes = new List<int>(){a,a,b,b,c,c};
    }

    private static void InitD33(){
        int a = GetRandomEye(D33_1);
        int b = GetRandomEye(D33_2, a);

        eyes = new List<int>(){a,a,a,b,b,b};
    }

    private static void InitD24(){
        int a = GetRandomEye(D24_1);
        int b = GetRandomEye(D24_2, a);

        eyes = new List<int>(){a,a,a,b,b,b};
    }

    private static void InitD15(){
        int a = GetRandomEye(D15_1);
        int b = GetRandomEye(D15_2, a);

        eyes = new List<int>(){a,b,b,b,b,b};
    }

    private static void InitD123(){
        int a = GetRandomEye(D123_1);
        int b = GetRandomEye(D123_2, a);
        int c = GetRandomEye(D123_3, a, b);

        eyes = new List<int>(){a,b,b,c,c,c};
    }

    private static void InitD6(){
        int a = GetRandomEye(D6);

        eyes = new List<int>(){a,a,a,a,a,a};
    }

    private static int GetRandomEye(int[] arr, int a = 0, int b = 0){
        int ret = 0;
        do {
            float rand = Random.Range(0f,100f);
            for(int i=0;i<6;i++){
                if(rand < arr[i]) {
                    ret = i + 1;
                    break;
                }
                rand -= arr[i];
            }
        } while (ret==a || ret==b);
        return ret;
    }
}