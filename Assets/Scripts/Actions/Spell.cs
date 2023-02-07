using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Spells
{
    private Dictionnary<string,Spell> spellList;

    public static void Initialize()
    {
         spellList = new Dictionnary<string,Spell>();
    }
    
    public static void AddSpell(Spell spell)
    {
         spellList.add(spell.GetName(),spell);
    }
    
    public static Spell GetSpell(string spellName)
    {
         return spellList[spellName];
    }
    
    
}

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
	protected int coolDown;
    protected List<EffectOnTime> effectsOnTarget;
    protected List<EffectOnTime> effectsOnSelf;


	public AbstractSpell()
    {
        spellName = "Splash";
        description = "A random magikarp splash attack.";
        resourceCost = 0;
        castTime = 1;
        levelRequirement = 1;
		coolDown = 0;
	}
	public AbstractSpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown)
	{
		this.spellName = name;
		this.description = description;
		this.resourceCost = resourceCost;
		this.castTime = castTime;
		this.levelRequirement = levelRequirement;
		this.coolDown = coolDown;
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
	public HostileSpell(string name, string desc, int rsrcCost, float castTime, int lvlReq, int cD) : base (name,desc,rsrcCost,castTime,lvlReq,cD){
		

	}

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
