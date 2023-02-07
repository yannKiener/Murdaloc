using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class HostileSpell : Spell
{
	public HostileSpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown,float maxDistance,Action<Character,Character> spellEffect, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf) 
		: base (true,name,description,resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, effectsOnTarget, effectsOnSelf){}


	public HostileSpell (Spell s) : base (s){}


	public override void Cast (Character caster, Character target)
	{
		base.Cast (caster, target);
		target.AggroTarget (caster);
	}

	private void ApplyEffectsOnTarget(GameObject target)
	{
		foreach (EffectOnTime buff in effectsOnTarget)
		{
			//apply effects on target
		}
	}


	private void ApplyEffectsOnSelf()
	{
		foreach (EffectOnTime buff in effectsOnTarget)
		{

			//apply effects
		}
	}

	public override bool IsCastable(Character caster, Character target){
		return target != null && base.IsCastable (caster,target) ;
	}

}