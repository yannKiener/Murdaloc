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



public abstract class AbstractSpell : MonoBehaviour, Spell
{
    protected string spellName;
    protected string description;
    protected int resourceCost;
    protected float castTime;
    protected int levelRequirement;
    protected List<EffectOnTime> effectsOnTarget;
    protected List<EffectOnTime> effectsOnSelf;

    public AbstractSpell()
    {
        spellName = "Splash";
        description = "A random magikarp splash attack.";
        resourceCost = 0;
        castTime = 1;
        levelRequirement = 1;
    }

    public string GetName() {
        return spellName;
    }

    public string GetDescription() {
        if(castTime == 0)
            return string.Concat(description,"Instant.","Level requirement : ", levelRequirement.ToString());
        else
            return string.Concat(description, "Casting time : ",castTime.ToString(), "Level requirement : ", levelRequirement.ToString());
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
