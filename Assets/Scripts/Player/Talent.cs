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

    public Talent(string name, string description, int maxStacks)
    {
        this.name = name;
        this.description = description;
        this.maxStacks = maxStacks;
        this.stacks = 0;
        icon = InterfaceUtils.LoadSpriteForTalent(name);
    }

    public void ActivateEffect()
    {
        Debug.Log("Activating talent effect");
    }

    public void RemoveEffect()
    {
        Debug.Log("Removing talent effect");
    }


    public void AddOne(){
        Player player = FindUtils.GetPlayer();
        if (stacks < maxStacks && player.RemoveOneTalentPoint())
        {
            stacks++;
            FindUtils.GetTalentSheetGrid().UpdateTalentsPointsRemainingText();
            ActivateEffect();
        }
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
