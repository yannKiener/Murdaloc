using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff : AbtractEffectOnTime
{
	public Buff(string name, string description, float duration, float timePerTic, int totalHeal)
	{
		this.name = name;
		this.description = description;
		this.timePerTic = timePerTic;
		this.duration = duration;
		this.totalHeal = totalHeal;
	}
}
