 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Spells
{
    private static Dictionary<string, Spell> spellList = new Dictionary<string, Spell>();

    public static void Add(Spell spell)
    {
		spellList.Add(spell.GetName().ToLower(), spell);
    }
    
    public static Spell Get(string spellName)
    {
		return spellList[spellName.ToLower()];
    }
}

public interface Spell
{
    string GetName();
    string GetDescription();
    int GetResourceCost();
    float GetCastTime();
    int GetLevelRequirement();
	bool IsCastable (Character caster, Character target);
    void Cast(Character caster, Character target);

}


public abstract class AbstractSpell : Spell
{
    protected string spellName;
    protected string description;
    protected int resourceCost;
    protected float castTime;
	protected int levelRequirement;
	protected int coolDown;
    protected List<EffectOnTime> effectsOnTarget;
    protected List<EffectOnTime> effectsOnSelf;
    protected int damage;


	public AbstractSpell()
    {
        spellName = "splash";
        description = "A random magikarp splash attack.";
        resourceCost = 0;
        castTime = 1;
        levelRequirement = 1;
		coolDown = 0;
		damage = 0;
	}

	public AbstractSpell(string name, string description, int resourceCost, float castTime, int damage, int levelRequirement, int coolDown)
	{
		this.spellName = name.ToLower();
		this.description = description;
		this.resourceCost = resourceCost;
		this.castTime = castTime;
		this.levelRequirement = levelRequirement;
		this.coolDown = coolDown;
		this.damage = damage;
	}


	public bool IsCastable(Character caster, Character target){
		return (caster.GetCurrentResource () >= resourceCost && target != null); //TODO : Ajouter le level et la distance plus tard
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

    public virtual void Cast(Character caster, Character target)
    {
    }

	protected int modifiedSpell(Character caster, Character target, int number){
		return number + (int)(number * Random.Range (-30f, 30f) / 100);
	}
}
