using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class FriendlySpell : Spell
{
	public FriendlySpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown,float maxDistance, Action<Character,Character, Spell> spellEffect, string soundType, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf, bool hasGcd) 
		: base (false,name,description,resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, soundType, effectsOnTarget, effectsOnSelf, hasGcd) {}
	
	public FriendlySpell (Spell s) : base (s){}

	public override void Cast(Character caster, Character target)
	{
		if (IsTargetNullOrEnemy(target)) {
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
        Spell spell = new FriendlySpell(this);
        spell.SetStopRegen(this.stopRegen);
        spell.SetActionOnCrit(this.actionOnCrit);
        spell.SetProcs(this.procs);
        spell.SetSpellCondition(this.GetSpellCondition());
        return spell;
    }

    public override bool IsCastable(Character caster, Character target, bool displayCDText = true)
    {
		if (IsTargetNullOrEnemy(target)) {
			target = caster;
		}

		return base.IsCastable (caster, target, displayCDText);
	}

	private bool IsTargetNullOrEnemy(Character target){
		return (target == null || target is Hostile);
	}
}
