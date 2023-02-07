using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public static class EffectsOnTime
{
	private static Dictionary<string, EffectOnTime> effectList = new Dictionary<string, EffectOnTime>();

	public static void Add(EffectOnTime effect)
	{
		effectList.Add(effect.GetName().ToLower(), effect);
	}

	public static EffectOnTime Get(string effectName)
	{
		return effectList[effectName.ToLower()];
	}
}

public class EffectOnTime
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
	private Action<Character,Character, float, int> tic;

	private Character attachedCharacter = null;
	private Character caster;
	private float timeLeft;
	private float nextTic;
	private bool toBeRemoved = false;

	public EffectOnTime(string name, string description, bool isBuff, int maxStacks, float duration, float timePerTic, Effect applyOnce, Action<Character,Character, float, int> tic){
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
				effect.stacks += 1;
				if(applyOnce != null)
				applyOnce.apply (caster, attachedCharacter);
			}	
			effect.refresh ();
		} else {
			attachedCharacter.AddEffectOnTime (new EffectOnTime(this));
			if(applyOnce != null)
            applyOnce.apply (caster, attachedCharacter);
		}
    }

	public void Remove() 
    {
        if(applyOnce != null)
		applyOnce.remove (caster, attachedCharacter);
		this.toBeRemoved = true;
    }

	public bool IsToBeRemoved()
	{
		return toBeRemoved;
	}


    public void Tic()
    {
		tic (caster, attachedCharacter,timePerTic/duration,stacks);
    }

    public void Update()
    {
        if (timeLeft < 0 || IsEffectOver())
        {
            Remove();
        }
        else
        {
            if (timeLeft < nextTic)
			{
                Tic();
                nextTic -= timePerTic;
            }
        }

		timeLeft -= Time.deltaTime;

    }

    protected bool IsEffectOver()
    {
        return false;
    }        
}



