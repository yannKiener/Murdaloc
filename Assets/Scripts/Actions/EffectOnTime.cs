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

	private Character attachedCharacter = null;
	private int damagePerTic = 0;
	private int healPerTic = 0;
	private float timeLeft;
	private float nextTic;
	private bool toBeRemoved = false;

	public EffectOnTime(string name, string description, bool isBuff, float duration, float timePerTic, float totalDamage, float totalHeal){
		this.name = name;
		this.description = description;
		this.isBuff = isBuff;
		this.timePerTic = timePerTic;
		this.duration = duration;
		this.damagePerTic = (int)((totalDamage/duration)*timePerTic);
		this.healPerTic = (int)((totalHeal/duration)*timePerTic);


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
        
	public void Apply(Character target)
    {
		attachedCharacter = target;
		attachedCharacter.AddEffectOnTime (this);
		timeLeft = duration;
		toBeRemoved = false;
        nextTic = duration - timePerTic;
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
		Debug.Log ("Effect tic : "+damagePerTic +" damage and "+healPerTic+" heal.");
		attachedCharacter.ApplyDamage (damagePerTic);
		attachedCharacter.ApplyHeal (healPerTic);
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



