using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Spell
{
    string GetName();
    string GetDescription();
    int GetResourceCost();
    float GetCastTime();
    int GetLevelRequirement();
    void Cast(GameObject target);

}



public abstract class AbstractSpell : Spell
{
    protected string name;
    protected string description;
    protected int resourceCost;
    protected float castTime;
    protected int levelRequirement;
    protected List<EffectOnTime> effectsOnTarget;
    protected List<EffectOnTime> effectsOnSelf;

    public AbstractSpell()
    {
        name = "Splash";
        description = "Magikarp magikarp !!";
        resourceCost = 0;
        castTime = 1;
        levelRequirement = 1;
    }

    public string GetName() {
        return name;
    }

    public string GetDescription() {
        return description;
    }

    public int GetResourceCost() {
        return resourceCost;
    }

    public float GetCastTime() {
        return castTime;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }

    public virtual void Cast(GameObject target)
    {
    }
}



[System.Serializable]
public class HostileSpell : AbstractSpell
{
    public override void Cast(GameObject target)
    {
        if (CheckCondition())
        {
            //Cast Spell
        }
        else
        {
            //Impossible de lancer le sort
        }

    }

    private void ApplyEffectsOnTarget(GameObject target)
    {
        foreach (EffectOnTime buff in effectsOnTarget)
        {
            //apply effects on target
        }
    }


    private void ApplyEffectsOnSelf()
    {
        foreach (EffectOnTime buff in effectsOnTarget)
        {
            //apply effects
        }
    }

    private bool CheckCondition()
    {
        return true;
    }
}



[System.Serializable]
public class FriendlySpell : AbstractSpell
{
    public override void Cast(GameObject target)
    {
        if (CheckCondition())
        {
            //Cast Spell
        }
        else
        {
            //Impossible de lancer le sort
        }

    }

    private bool CheckCondition()
    {
        return true;
    }
}
