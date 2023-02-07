using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective  {

    string nameToKill;
    int killCount;
    bool isOver = false;


    public Objective(string name, int killcount)
    {
        this.nameToKill = name;
        this.killCount = killcount;
    }

    public string GetName()
    {
        return nameToKill;
    }

    public bool IsOver()
    {
        return isOver;
    }

    public void RemoveOne()
    {
        killCount -= 1;
        if(killCount <= 0)
        {
            isOver = true;
        }
    }

}
