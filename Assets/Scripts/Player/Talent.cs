using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Talent
{
    private string name;
    private string description;
    private int stacks;
    private int maxStacks;
    Action<Character, int> activateTalentOnPlayer;
    Action<Character, int> deactivateTalentOnPlayer;
    Talent linkedTalent;

    public Talent(string name, string description, int maxStacks, Action<Character, int> activationEffect, Action<Character, int> deactivationEffect, Talent linkedTalent = null)
    {
        this.name = name;
        this.description = description;
        this.maxStacks = maxStacks;
        this.stacks = 0;
        activateTalentOnPlayer = activationEffect;
        deactivateTalentOnPlayer = deactivationEffect;
        this.linkedTalent = linkedTalent;
    }

    public void LoadTalent()
    {
        for(int i = 0; i < stacks; i++)
        {
            ActivateEffect();
        }
    }

    public Talent GetLinkedTalent()
    {
        return linkedTalent;
    }

    public bool IsMaxed()
    {
        return stacks == maxStacks;
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
            FindUtils.GetPlayer().AddTalentPoint();
            RemoveEffect();
            stacks--;
        }
    }


    public Sprite GetImage()
    {
        return InterfaceUtils.LoadSpriteForTalent(name);
    }

    public string GetDescription()
    {
        if(linkedTalent == null)
        {
            return description;
        } else
        {
            return description + "\n Talent required : " + linkedTalent.GetName();
        }
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
