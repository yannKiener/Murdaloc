﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
		Spell s = spellList [spellName.ToLower ()];
		if (s.isHostile ()) {
			return new HostileSpell(s);
		} else {
			return new FriendlySpell(s);
		}
    }
}
	


public abstract class Spell : Usable
{
    protected string spellName;
    protected string description;
    protected int resourceCost;
    protected float castTime;
	protected int levelRequirement;
	protected int coolDown;
	protected float coolDownRemaing = 0;
    protected List<EffectOnTime> effectsOnTarget;
    protected List<EffectOnTime> effectsOnSelf;
	protected float maxDistance;
	protected Action<Character,Character> applySpellEffect;
	protected bool isHostileSpell;

	public Spell(bool isHostile,string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown,float maxDistance,Action<Character,Character> spellEffect,List<EffectOnTime> effectsOnTarget = null, List<EffectOnTime> effectsOnSelf = null)
	{
		this.spellName = name.ToLower();
		this.description = description;
		this.resourceCost = resourceCost;
		this.castTime = castTime;
		this.levelRequirement = levelRequirement;
		this.coolDown = coolDown;
		this.applySpellEffect = spellEffect;
		this.effectsOnTarget = effectsOnTarget;
		this.effectsOnSelf = effectsOnSelf;
		this.maxDistance = maxDistance;
		this.isHostileSpell = isHostile;
	}


	public Spell(Spell s){
		this.spellName = s.spellName;
		this.description = s.description;
		this.resourceCost = s.resourceCost;
		this.castTime = s.castTime;
		this.levelRequirement = s.levelRequirement;
		this.coolDown = s.coolDown;
		this.applySpellEffect = s.applySpellEffect;
		this.effectsOnTarget = s.effectsOnTarget;
		this.effectsOnSelf = s.effectsOnSelf;
		this.maxDistance = s.maxDistance;
	}


	public virtual bool IsCastable(Character caster, Character target){
		return (caster.GetCurrentResource () >= resourceCost && checkDistance(caster,target) && checkCoolDown(true)); //TODO : Ajouter le level 
	}

	protected bool checkDistance(Character caster, Character target){
		bool isDistanceOK = Mathf.Abs ((caster.transform.position.x - target.transform.position.x)) < maxDistance;
		if (!isDistanceOK) {
			Debug.Log (target.name+" is too far");
		}

		return isDistanceOK;
	}

	public void UpdateCoolDown(float timeElapsed){
		if (coolDownRemaing != 0) {
			coolDownRemaing -= timeElapsed;
			if (coolDownRemaing < 0)
				coolDownRemaing = 0;
		}

	}

	public bool checkCoolDown(bool displayText = false){
		bool isCDOK = coolDownRemaing == 0;
		if (!isCDOK && displayText) {
			Debug.Log ("CD remaining : " + coolDownRemaing);
		}
		return isCDOK;
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

	public bool isHostile(){
		return isHostileSpell;
	}
		
	public float GetCastTime(Stats stats) {
		return castTime - castTime * stats.Haste/Constants.hasteDivider;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }

	protected override void Use (Character caster, Character target)
	{
		base.Use (caster,target);
		Cast (caster,target);
	}

    public virtual void Cast(Character caster, Character target)
    {
		if (IsCastable (caster, target)) {
			caster.RemoveResource (resourceCost);
			if (applySpellEffect != null) {
				applySpellEffect (caster, target);
			}
			applyEffectsOn (caster, caster, effectsOnSelf);
			applyEffectsOn (caster, target, effectsOnTarget);
			coolDownRemaing = coolDown;
			caster.addSpellOnCD (this);
		}
    }

	protected void applyEffectsOn(Character caster, Character target, List<EffectOnTime> effects){
		if(target != null && effects != null && effects.Count > 0){
			foreach (EffectOnTime effect in effects) {
				effect.Apply (caster, target);
			}
		}

	}
}
