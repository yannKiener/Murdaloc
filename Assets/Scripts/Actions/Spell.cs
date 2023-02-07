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
    protected Dictionary<string, EffectOnTime> effectsOnTarget = new Dictionary<string, EffectOnTime>();
    protected Dictionary<string, EffectOnTime> effectsOnSelf = new Dictionary<string, EffectOnTime>();
	protected float maxDistance;
	protected Action<Character,Character, Spell> applySpellEffect;
    protected Func<Character, Character, Spell, bool> spellCondition;
    bool hasGcd;
	protected bool isHostileSpell;
	protected Image image;
    protected AudioClip preCastSound;
    protected List<AudioClip> castSounds;
    protected List<AudioClip> impactSounds;

    float NormalMultiplier = 100;
    float CritMultiplier = 100;
    Action<Character, Character> actionOnCrit;
    Dictionary<Castable, float> procs = new Dictionary<Castable, float>();

    public Spell(bool isHostile, string name, string description, int resourceCost, float castTime, int levelRequirement, float coolDown, float maxDistance, Action<Character, Character, Spell> spellEffect, string soundType = "Default", List<EffectOnTime> effectsOnTarget = null, List<EffectOnTime> effectsOnSelf = null, bool hasGcd = true)
	{
		this.spellName = name;
		this.description = description;
		this.resourceCost = resourceCost;
		this.castTime = castTime;
		this.levelRequirement = levelRequirement;
		this.coolDown = coolDown;
		this.applySpellEffect = spellEffect;
        this.hasGcd = hasGcd;

        if (effectsOnTarget != null)
        {
            foreach (EffectOnTime effect in effectsOnTarget)
            {
                this.effectsOnTarget[effect.GetName()] = effect;
            }
        }

        if(effectsOnSelf != null)
        {
            foreach (EffectOnTime effect in effectsOnSelf)
            {
                this.effectsOnSelf[effect.GetName()] = effect;
            }
        }

		this.maxDistance = maxDistance;
		this.isHostileSpell = isHostile;
		this.image = InterfaceUtils.LoadImageForSpell (spellName);
        this.preCastSound = DatabaseUtils.GetPrecastSound(soundType);
        this.castSounds = DatabaseUtils.GetCastSound(soundType);
        this.impactSounds = DatabaseUtils.GetIImpactSound(soundType);
    }

    public void SetSpellCondition(Func<Character, Character, Spell, bool> spCondition)
    {
        spellCondition = spCondition;
    }

    public Func<Character, Character, Spell, bool> GetSpellCondition()
    {
        return spellCondition;
    }
    public void RemoveSpellCondition()
    {
        spellCondition = null;
    }

    public bool HasGcd()
    {
        return hasGcd;
    }
    public void SetHasGcd(bool hasGcd)
    {
        this.hasGcd = hasGcd;
    }
		
	public Spell(Spell s){
        this.isHostileSpell = s.isHostile();
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
        this.spellCondition = s.GetSpellCondition();
        this.hasGcd = s.HasGcd();
    }

    public Sprite GetImageAsSprite()
    {
        return InterfaceUtils.LoadSpriteForSpell(this.GetName());
    }

	public void Use(Character caster){
        caster.CastSpell(this.spellName);


	}

    public virtual Spell Clone()
    {
        return null;
    }


	public virtual bool IsCastable(Character caster, Character target, bool displayCDText = true){
        bool specialCondition = true;

        if (spellCondition != null) {
            specialCondition = spellCondition(caster, target, this);
        } 
		return (caster != null && target != null && caster.GetCurrentResource () >= resourceCost && specialCondition && checkLevel(caster) && checkDistance(caster,target) && checkCoolDown(displayCDText));
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
        if (castTime == 0)
            return string.Concat(description, "\nInstant cast.", "\nLevel requirement : ", levelRequirement.ToString());
        else
            return string.Concat(description, "\nCasting time : ", GetCastTime(FindUtils.GetPlayer().GetStats()).ToString(),"s.", "\nLevel requirement : ", levelRequirement.ToString());
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

    public void AddCooldownRemaining(Character caster)
    {
        caster.addSpellOnCD(this);
        coolDownRemaing = coolDown;
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
            CheckProcs(caster, target);
            AddCooldownRemaining(caster);

        }
    }

    public EffectOnTime GetEffectOnTarget(string name)
    {
        if (effectsOnTarget.ContainsKey(name))
        {
            return effectsOnTarget[name];
        }
        return null;
    }

    public Dictionary<string, EffectOnTime> GetEffectsOnTarget()
    {
        return effectsOnTarget;
    }

    public void RemoveEffectOnTarget(string name)
    {
        effectsOnTarget.Remove(name);
    }

    public void AddEffectOnTarget(EffectOnTime effect)
    {
        effectsOnTarget[effect.GetName()] = effect;
    }

    public EffectOnTime GetEffectOnSelf(string name)
    {
        return effectsOnSelf[name];
    }

    public void RemoveEffectOnSelf(string name)
    {
        effectsOnSelf.Remove(name);
    }

    public void AddEffectOnSelf(EffectOnTime effect)
    {
        effectsOnSelf[effect.GetName()] = effect;
    }

    protected void applyEffectsOn(Character caster, Character target, Dictionary<string, EffectOnTime> effects){
		if(target != null && effects != null && effects.Count > 0){
			foreach (KeyValuePair<string,EffectOnTime> effect in effects) {
				effect.Value.Apply (caster, target);
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

    public void AddCastTime(float cstTime)
    {
        this.castTime += cstTime;
    }

    public void RemoveCastTime(float cstTime)
    {
        this.castTime -= cstTime;
    }

    public void SetCastTime(float cstTime)
    {
        this.castTime = cstTime;
    }

    public void SetCooldown(float cd)
    {
        this.coolDown = cd;
    }

    public void AddCooldown(float cd)
    {
        this.coolDown += cd;
    }

    public void RemoveCooldown(float cd)
    {
        this.coolDown -= cd;
    }

    public void SetMaxDistance(float maxDist)
    {
        this.maxDistance = maxDist;
    }

    public void AddMaxDistance(float maxDist)
    {
        this.maxDistance += maxDist;
    }

    public void RemoveMaxDistance(float maxDist)
    {
        this.maxDistance -= maxDist;
    }



    public void AddToNormalMultiplier (float n)
    {
        NormalMultiplier += n;
    }

    public void RemoveToNormalMultiplier(float n)
    {
        NormalMultiplier -= n;
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

    public void RemoveActionOnCrit()
    {
        actionOnCrit = null;
    }

    public void OnCrit(Character caster, Character target, int damageOrHeal)
    {
        if (actionOnCrit != null)
        {
            actionOnCrit(caster, target);
        }
    }

    public void SetProc(Castable procEffect, float chancePercent)
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
            if(UnityEngine.Random.Range(0, 101) <= kv.Value)
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
