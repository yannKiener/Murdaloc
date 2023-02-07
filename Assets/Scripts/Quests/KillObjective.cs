using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillObjective : Objective{

    string nameToKill;
    int killCount;
    bool isOver = false;


    public KillObjective(string name, int killcount)
    {
        Debug.Log("new KillObjective : kill " + killcount + " " + name);
        this.nameToKill = name;
        this.killCount = killcount;
    }

    public string GetName()
    {
        return nameToKill;
    }

    public int GetKillCount()
    {
        return killCount;
    }

    public bool IsOver()
    {
        return isOver;
    }

    public void Update(Hostile enemy)
    {
        if (enemy != null && nameToKill.Equals(enemy.GetName()))
        {
            killCount -= 1;
        }

        if (killCount <= 0)
        {
            isOver = true;
        }
    }
}
