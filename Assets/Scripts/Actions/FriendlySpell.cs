using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class FriendlySpell : Spell
{
	public FriendlySpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown,float maxDistance,Action<Character,Character> spellEffect, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf) : base (name,description,resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, effectsOnTarget, effectsOnSelf){


	}

	public override void Cast(Character caster, Character target)
	{
		if (targetIsNullOrEnemy(target)) {
			target = caster;
		}

		if (IsCastable(caster,target))
		{
			base.Cast (caster, target);
		}
		else
		{
			//Impossible de lancer le sort
		}

	}

	public override bool IsCastable(Character caster, Character target){
		if (targetIsNullOrEnemy(target)) {
			target = caster;
		}

		return base.IsCastable (caster, target);
	}

	private bool targetIsNullOrEnemy(Character target){
		return (target == null || target.tag == "Enemy");
	}
}
