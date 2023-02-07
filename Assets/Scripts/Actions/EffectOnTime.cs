using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EffectOnTime
{
	void Apply(Character target);
    void Remove();
    void Tic();
    void Update();
	string GetName();
	string GetDescription();

}



public abstract class AbtractEffectOnTime : EffectOnTime
{
    protected float duration;
    protected float timeLeft;
    protected float timePerTic;
    protected float nextTic;
    protected int totalDamage;
    protected int totalHeal;
    protected string name;
    protected string description;
	protected Character attachedCharacter;
	protected int damagePerTic;
	protected int healPerTic;

	public string GetName(){
		return name;
	}

	public string GetDescription(){
		return description;
	}
        
	public void Apply(Character target)
    {
		attachedCharacter = target;
        timeLeft = duration;
        nextTic = duration - timePerTic;
    }

    public void Remove()
    {

    }

    public void Tic()
    {
		attachedCharacter.ApplyDamage (damagePerTic);
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
                duration -= Time.deltaTime;
            }
        }

    }

    protected bool IsEffectOver()
    {
        return false;
    }        
}



