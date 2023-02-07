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
	


public abstract class Spell
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
	protected bool isCrit = false;
	protected float maxDistance;

	public Spell(string name, string description, int resourceCost, float castTime, int damage, int levelRequirement, int coolDown,float maxDistance,List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf)
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
		this.maxDistance = maxDistance;
	}

	public Spell(string name, string description, int resourceCost, float castTime, int damage, int levelRequirement, int coolDown,float maxDistance)
	{
		this.spellName = name.ToLower();
		this.description = description;
		this.resourceCost = resourceCost;
		this.castTime = castTime;
		this.levelRequirement = levelRequirement;
		this.coolDown = coolDown;
		this.damage = damage;
		this.maxDistance = maxDistance;
	}


	public virtual bool IsCastable(Character caster, Character target){
		return (caster.GetCurrentResource () >= resourceCost && checkDistance(caster,target)); //TODO : Ajouter le level 
	}

	protected bool checkDistance(Character caster, Character target){
		bool isDistanceOK = Mathf.Abs ((caster.transform.position.x - target.transform.position.x)) < maxDistance;
		if (!isDistanceOK) {
			Debug.Log (target.name+" is too far");
		}

		return isDistanceOK;

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

	public bool IsCrit() {
        return this.isCrit;
    }


	public float GetCastTime(Stats stats) {
		return castTime - castTime * stats.Haste/Constants.hasteDivider;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }

    public virtual void Cast(Character caster, Character target)
    {
		caster.RemoveResource (resourceCost);
		applyEffectsOn (caster, caster, effectsOnSelf);
		applyEffectsOn (caster, target, effectsOnTarget);
    }

	protected int modifiedSpell (Character caster, Character target, int number)
	{
		Stats casterStats = caster.GetStats ();
		this.isCrit = casterStats.Critical > Random.Range (1, 101);

		number = number + (number * casterStats.Power / 100); //Applying power 
		if (this.isCrit) { // Apply Crit
			number = number * 2;
		}
		return (int)(number + number * Random.Range (-30f, 30f) / 100);
	}

	protected void applyEffectsOn(Character caster, Character target, List<EffectOnTime> effects){
		if(target != null && effects != null && effects.Count > 0){
			foreach (EffectOnTime effect in effects) {
				effect.Apply (caster, target);
			}
		}

	}
}
