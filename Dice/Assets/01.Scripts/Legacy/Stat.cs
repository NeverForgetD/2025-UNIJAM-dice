using UnityEngine;

public class Stat
{
    public int maxhp;
    public int hp;
    public int ad;
    public int def;
    public int pot;
    public int bonus = 0;
    public int Atk{
        get{
            int ret = ad + bonus;
            bonus = 0;
            return ret;
        }
    }

    public void Init(){
        maxhp = 20;
        hp = maxhp;
        ad = 5;
        def = 3;
        pot = 5;
    }
}
