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
	float GetCastTime(Stats stats);
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
	}

	public AbstractSpell(string name, string description, int resourceCost, float castTime, int damage, int levelRequirement, int coolDown,List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf)
	{
		this.spellName = name.ToLower();
		this.description = description;
		this.resourceCost = resourceCost;
		this.castTime = castTime;
		this.levelRequirement = levelRequirement;
		this.coolDown = coolDown;
		this.damage = damage;
		this.effectsOnTarget = effectsOnTarget;
		this.effectsOnSelf = effectsOnSelf;
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


	public virtual bool IsCastable(Character caster, Character target){
		return (caster.GetCurrentResource () >= resourceCost); //TODO : Ajouter le level et la distance plus tard
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

	public float GetCastTime(Stats stats) {
		return castTime - castTime * stats.Haste/100;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }

    public virtual void Cast(Character caster, Character target)
    {
		caster.RemoveResource (resourceCost);
		applyEffectsOn (caster, effectsOnSelf);
		applyEffectsOn (target, effectsOnTarget);
    }

	protected int modifiedSpell(Character caster, Character target, int number){
		Stats casterStats = caster.GetStats ();
		//apply casterStats and targetStats here
		return number + (int)(number * Random.Range (-30f, 30f) / 100);
	}

	protected void applyEffectsOn(Character character, List<EffectOnTime> effects){
		if(character != null && effects != null && effects.Count > 0){
			foreach (EffectOnTime effect in effects) {
				effect.Apply (character);
			}
		}

	}
}
