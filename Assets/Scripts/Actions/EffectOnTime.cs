using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EffectOnTime : Castable
{
	private float duration;
	private float timePerTic;
	private string name;
	private string description;
	private bool isBuff;
	private bool isCrit;
	private bool isStackable = false;
	private int stacks = 1;
	private int maxStacks = 1;
	private Effect applyOnce;
	private Action<Character,Character, EffectOnTime> tic;

	private Character attachedCharacter = null;
	private Character caster;
	private float timeLeft;
	private float nextTic;
	private bool toBeRemoved = false;

    float NormalMultiplier = 100;
    float CritMultiplier = 100;
    Action<Character, Character> actionOnCrit;
    Dictionary<Castable, float> procs = new Dictionary<Castable, float>();

    public EffectOnTime(string name, string description, bool isBuff, int maxStacks, float duration, float timePerTic, Effect applyOnce, Action<Character,Character, EffectOnTime> tic){
		this.name = name;
		this.description = description;
		this.isBuff = isBuff;
		this.timePerTic = timePerTic;
		this.duration = duration;
		this.nextTic = duration - timePerTic;
		if (maxStacks > 1) {
			this.isStackable = true;
		}
		this.applyOnce = applyOnce;
		this.tic = tic;
		this.maxStacks = maxStacks;
	}

	public EffectOnTime(EffectOnTime effect){
		this.timeLeft = effect.duration;
		this.name = effect.GetName();
		this.description = effect.GetDescription();
		this.isBuff = effect.IsBuff();
		this.timePerTic = effect.timePerTic;
		this.duration = effect.duration;
		this.applyOnce = effect.applyOnce;
		this.tic = effect.tic;
		this.nextTic = duration - timePerTic;
		this.attachedCharacter = effect.attachedCharacter;
		this.caster = effect.caster;
		this.isStackable = effect.isStackable;
		this.maxStacks = effect.maxStacks;
		this.timePerTic = timePerTic - timePerTic * caster.GetStats ().Haste / Constants.hasteDivider; // pour prendre en compte la hate du caster
        this.NormalMultiplier = effect.GetNormalMultiplier();
        this.CritMultiplier = effect.GetCritMultiplier();
    }
		

	public EffectOnTime Clone(){
		return new EffectOnTime(this);
	}

	public float GetTimeLeft(){
		return timeLeft;
	}

	public int GetStacks(){
		return stacks;
	}

	public void refresh(){
		this.timeLeft = this.duration;
		this.nextTic = duration - timePerTic;

	}

	public bool IsBuff(){
		return isBuff;
	}

	public string GetName(){
		return name;
	}

	public float GetDuration(){
		return duration;
	}

	public string GetDescription(){
		return description;
	}
        
	public void Apply(Character caster,Character target)
    {
		attachedCharacter = target;
		this.caster = caster;
		EffectOnTime effect = attachedCharacter.getEffect (this);

		if(effect != null){
			if(isStackable && effect.stacks < maxStacks){
				removeEffectOnce ();
				effect.stacks += 1;
				applyEffectOnce ();
			} 
			effect.refresh ();
		} else {
			attachedCharacter.AddEffectOnTime (new EffectOnTime(this));
			applyEffectOnce ();
		}
    }

	public void Remove() 
    {
		removeEffectOnce ();
		this.toBeRemoved = true;
    }

	private void applyEffectOnce(){
		if(applyOnce != null)
			applyOnce.apply (caster, attachedCharacter, attachedCharacter.getEffect (this).GetStacks());

	}

	private void removeEffectOnce(){
		if (applyOnce != null) {
			applyOnce.remove (caster, attachedCharacter,  attachedCharacter.getEffect (this).GetStacks());
		}

	}

	public bool IsToBeRemoved()
	{
		return toBeRemoved;
	}

    public float GetTimeDivider()
    {
        return timePerTic / duration;
    }


    public void Tic()
    {
		if(tic != null)
		tic (caster, attachedCharacter, this);
        CheckProcs(caster, attachedCharacter);
    }

    public void Update()
    {
        if (timeLeft < nextTic)
        {
            Tic();
            nextTic -= timePerTic;
        }

        if (timeLeft < 0 || IsEffectOver())
        {
            Remove();
        }
		timeLeft -= Time.deltaTime;
    }

    protected bool IsEffectOver()
    {
        return false;
    }

    public void AddToNormalMultiplier(float n)
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
        Debug.Log(NormalMultiplier);
        return NormalMultiplier;
    }

    public float GetCritMultiplier()
    {
        return CritMultiplier;
    }

    public void SetActionOnCrit(Action<Character, Character> act)
    {
        actionOnCrit = act;
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
            if (UnityEngine.Random.Range(0, 101) <= kv.Value)
            {
                kv.Key.ApplyTo(caster, target);
            }
        }
    }

    public void ApplyTo(Character caster, Character target)
    {
        Apply(caster, target);
    }
}



