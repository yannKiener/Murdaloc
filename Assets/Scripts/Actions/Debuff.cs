using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Debuff : AbtractEffectOnTime
{
	public Debuff(string name, string description, float duration, float timePerTic, int totalDamage)
	{
		this.name = name;
		this.description = description;
		this.timePerTic = timePerTic;
		this.duration = duration;
		this.totalDamage = totalDamage;
		this.damagePerTic = (int)((totalDamage / duration) * timePerTic);
	}
}
