using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class Spell : Usable, Castable
{
    protected string spellName;
    protected string description;
    protected int resourceCost;
    protected float castTime;
	protected int levelRequirement;
	protected float coolDown;
	protected float coolDownRemaing = 0;
    protected List<EffectOnTime> effectsOnTarget;
    protected List<EffectOnTime> effectsOnSelf;
	protected float maxDistance;
	protected Action<Character,Character, Spell> applySpellEffect;
	protected bool isHostileSpell;
	protected Image image;
    protected AudioClip preCastSound;
    protected List<AudioClip> castSounds;
    protected List<AudioClip> impactSounds;
    float NormalMultiplier = 100;
    float CritMultiplier = 100;
    Action<Character, Character> actionOnCrit;
    Dictionary<Castable, float> procs;

    public Spell(bool isHostile,string name, string description, int resourceCost, float castTime, int levelRequirement, float coolDown,float maxDistance, Action<Character,Character, Spell> spellEffect, string soundType = "Default", List<EffectOnTime> effectsOnTarget = null, List<EffectOnTime> effectsOnSelf = null)
	{
		this.spellName = name;
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
		this.image = InterfaceUtils.LoadImageForSpell (spellName);
        this.preCastSound = DatabaseUtils.GetPrecastSound(soundType);
        this.castSounds = DatabaseUtils.GetCastSound(soundType);
        this.impactSounds = DatabaseUtils.GetIImpactSound(soundType);
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
		this.image = InterfaceUtils.LoadImageForSpell (spellName);
        this.preCastSound = s.preCastSound;
        this.castSounds = s.castSounds;
        this.impactSounds = s.impactSounds;

    }

    public Sprite GetImageAsSprite()
    {
        return InterfaceUtils.LoadSpriteForSpell(this.GetName());
    }

	public void Use(Character caster){
        caster.CastSpell(this.spellName);


	}


	public virtual bool IsCastable(Character caster, Character target){
		return (caster != null && target != null && caster.GetCurrentResource () >= resourceCost && checkLevel(caster) && checkDistance(caster,target) && checkCoolDown(true) && checkFriendlyTarget(target));
	}

    protected bool checkFriendlyTarget(Character target)
    {
        bool result = !(target is Friendly);
        return result;
    }

    protected bool checkLevel(Character caster) {
        bool levelOk = caster.GetLevel() >= levelRequirement;
        if (!levelOk && caster.GetName() == FindUtils.GetPlayer().name)
        {
            MessageUtils.ErrorMessage("Level " + levelRequirement + " required to cast " + spellName +".");
        }
        return levelOk;
    }
	protected bool checkDistance(Character caster, Character target){
		bool isDistanceOK = Mathf.Abs ((caster.transform.position.x - target.transform.position.x)) < maxDistance;
		if (!isDistanceOK && caster.GetName() == FindUtils.GetPlayer().name) {
                MessageUtils.ErrorMessage(target.name+" is too far.");
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
            MessageUtils.ErrorMessage("CoolDown remaining : " + (Math.Round(coolDownRemaing, 1)));
		}
		return isCDOK;
	}


    public string GetName() {
        return spellName;
    }

    public string GetDescription() {
        if(castTime == 0)
            return string.Concat(description,"\nInstant cast.","\nLevel requirement : ", levelRequirement.ToString());
        else
            return string.Concat(description, "\nCasting time : ",castTime.ToString(),"s.", "\nLevel requirement : ", levelRequirement.ToString());
    }

    public int GetResourceCost() {
        return resourceCost;
    }

	public bool isHostile(){
		return isHostileSpell;
	}
		
	public float GetCastTime(Stats stats) {
        if(stats.Haste > Constants.MaxHaste)
        {
            stats.Haste = Constants.MaxHaste;
        }
		return castTime - castTime * stats.Haste/Constants.hasteDivider;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }

    public virtual void Cast(Character caster, Character target)
    {
		if (IsCastable (caster, target)) {
            SoundManager.PlaySound(castSounds);
			caster.RemoveResource (resourceCost);
			if (applySpellEffect != null) {
				applySpellEffect (caster, target, this);
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

    public AudioClip GetPreCastSound()
    {
        return preCastSound;
    }


    public float GetCooldown()
    {
        return coolDown;
    }

    public float GetMaxDistance()
    {
        return maxDistance;
    }

    public void SetResourceCost(int rscCost)
    {
        this.resourceCost = rscCost;
    }

    public void SetCastTime(float cstTime)
    {
        this.castTime = cstTime;
    }

    public void SetCooldown(float cd)
    {
        this.coolDown = cd;
    }

    public void SetMaxDistance(float maxDist)
    {
        this.maxDistance = maxDist;
    }



    public void SetNormalMultiplier(float newNormalMultiplier)
    {
        NormalMultiplier = newNormalMultiplier;
    }

    public void SetCritMultiplier(float newCritMultiplier)
    {
        CritMultiplier = newCritMultiplier;
    }

    public float GetNormalMultiplier()
    {
        return NormalMultiplier;
    }

    public float GetCritMultiplier()
    {
        return CritMultiplier;
    }

    public void SetActionOnCrit(Action<Character,Character> act)
    {
        actionOnCrit = act;
    }

    public void OnCrit(Character caster, Character target, int damage)
    {
        actionOnCrit(caster, target);
    }

    public void AddProc(Castable procEffect, float chancePercent)
    {
        procs[procEffect] = chancePercent;
    }

    public void RemoveProc(Castable procEffect)
    {
        procs.Remove(procEffect);
    }

    private void CheckProcs(Character caster, Character target)
    {
        foreach (KeyValuePair<Castable, float> kv in procs)
        {
            if(UnityEngine.Random.Range(0, 100) <= kv.Value)
            {
                kv.Key.ApplyTo(caster, target);
            }
        }
    }

    public void ApplyTo(Character caster, Character target)
    {
        Cast(caster, target);
    }
}
