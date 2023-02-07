using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class FriendlySpell : Spell
{
	public FriendlySpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown,float maxDistance, Action<Character,Character, Spell> spellEffect, string soundType, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf) 
		: base (false,name,description,resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, soundType, effectsOnTarget, effectsOnSelf){}
	
	public FriendlySpell (Spell s) : base (s){}

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

    public override Spell Clone()
    {
        return new FriendlySpell(this);
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
