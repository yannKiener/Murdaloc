using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
	//private int totalDamage;
	//private int totalHeal;
	private string name;
	private string description;
	private bool isBuff;
	private bool isCrit;
	private bool isStackable = false;
	private int stacks = 1;

	private Character attachedCharacter = null;
	private Character caster;
	private int damagePerTic = 0;
	private int healPerTic = 0;
	private float timeLeft;
	private float nextTic;
	private bool toBeRemoved = false;

	public EffectOnTime(string name, string description, bool isBuff, bool stackable, float duration, float timePerTic, float totalDamage, float totalHeal){
		this.name = name;
		this.description = description;
		this.isBuff = isBuff;
		this.timePerTic = timePerTic;
		this.duration = duration;
		this.damagePerTic = (int)((totalDamage/duration)*timePerTic);
		this.healPerTic = (int)((totalHeal/duration)*timePerTic);
		this.nextTic = duration - timePerTic;
		this.isStackable = stackable;
	}

	public EffectOnTime(EffectOnTime effect){
		this.timeLeft = effect.duration;
		this.name = effect.GetName();
		this.description = effect.GetDescription();
		this.isBuff = effect.IsBuff();
		this.timePerTic = effect.timePerTic;
		this.duration = effect.duration;
		this.damagePerTic = effect.damagePerTic;
		this.healPerTic = effect.healPerTic;
		this.nextTic = duration - timePerTic;
		this.attachedCharacter = effect.attachedCharacter;
		this.caster = effect.caster;
		this.isStackable = effect.isStackable;
	}
		

	public EffectOnTime Clone(){
		return new EffectOnTime(this);
	}

	public void refresh(){
		Debug.Log(name + " refreshed on " +  attachedCharacter.GetName());
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
			if(isStackable){
				effect.stacks += 1;
			}	
			effect.refresh ();
		} else {
			Debug.Log ("Doesn't have " + this.name);
			attachedCharacter.AddEffectOnTime (new EffectOnTime(this));
		}
    }

	public void Remove()
    {
		this.toBeRemoved = true;
    }

	public bool IsToBeRemoved()
	{
		return toBeRemoved;
	}


    public void Tic()
    {
		attachedCharacter.ApplyDamage (modifiedTic(damagePerTic), isCrit);
		attachedCharacter.ApplyHeal (modifiedTic(healPerTic), isCrit);
    }


	private int modifiedTic (int number)
	{
		Stats casterStats = caster.GetStats ();
		this.isCrit = casterStats.Critical > Random.Range (1, 101);

		number = number + (number * casterStats.Power / 100); //Applying power 
		if (this.isCrit) { // Apply Crit
			number = number * 2;
		}
		return (int)(number + number * Random.Range (-30f, 30f) / 100) * stacks;
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



