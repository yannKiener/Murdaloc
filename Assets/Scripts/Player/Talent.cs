using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Talent
{
    private string name;
    private string description;
    private int stacks;
    private int maxStacks;
    private Sprite icon;
    Action<Character, int> activateTalentOnPlayer;
    Action<Character, int> deactivateTalentOnPlayer;

    public Talent(string name, string description, int maxStacks, Action<Character, int> activationEffect, Action<Character, int> deactivationEffect)
    {
        this.name = name;
        this.description = description;
        this.maxStacks = maxStacks;
        this.stacks = 0;
        icon = InterfaceUtils.LoadSpriteForTalent(name);
        activateTalentOnPlayer = activationEffect;
        deactivateTalentOnPlayer = deactivationEffect;
    }

    public void ActivateEffect()
    {
        activateTalentOnPlayer(FindUtils.GetPlayer(), stacks);
    }

    public void RemoveEffect()
    {
        deactivateTalentOnPlayer(FindUtils.GetPlayer(), stacks);
    }


    public bool AddOne(){
        
        if (stacks < maxStacks && FindUtils.GetPlayer().RemoveOneTalentPoint())
        {
            stacks++;
            FindUtils.GetTalentSheetGrid().UpdateTalentsPointsRemainingText();
            ActivateEffect();
            return true;
        }

        return false;
    }
    

    public void Reset()
    {
        while (stacks > 0)
        {
            RemoveEffect();
            stacks--;
        }
    }


    public Sprite GetImage()
    {
        return icon;
    }

    public string GetDescription()
    {
        return description;
    }

    public int GetStacks()
    {
        return stacks;
    }

    public int GetMaxStacks()
    {
        return maxStacks;
    }

    public string GetName()
    {
        return name;
    }

}
