using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EffectOnTime
{
    void Apply();
    void Remove();
    void Tic();
    void Update();
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
        
    public void Apply()
    {
        timeLeft = duration;
        nextTic = duration - timePerTic;
    }

    public void Remove()
    {

    }

    public void Tic()
    {

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



[System.Serializable]
public class Buff : AbtractEffectOnTime
{
    public Buff(float duration, float timePerTic)
    {
        this.timePerTic = timePerTic;
        this.duration = duration;
    }
}



[System.Serializable]
public class Debuff : AbtractEffectOnTime
{
    public Debuff()
    {
    }
}
