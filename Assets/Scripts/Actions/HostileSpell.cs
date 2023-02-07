using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class HostileSpell : Spell
{
	public HostileSpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown,float maxDistance, Action<Character,Character, Spell> spellEffect, string soundType, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf, bool hasGcd) 
		: base (true,name,description,resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, soundType, effectsOnTarget, effectsOnSelf, hasGcd) {}


	public HostileSpell (Spell s) : base (s){}

    public override Spell Clone()
    {
        return new HostileSpell(this);
    }

    public override void Cast (Character caster, Character target)
	{
		base.Cast (caster, target);
        if (IsCastable(caster,target,false) && caster != null)
        {
            target.AggroTarget(caster);
        }
	}

	public override bool IsCastable(Character caster, Character target, bool displayCDText = true)
    {
		return IsTargetAndCasterEnemies(caster,target) && base.IsCastable (caster,target, displayCDText) ;
	}

    private bool IsTargetAndCasterEnemies(Character caster, Character target)
    {
        return target != null && caster != null && ((caster is Player || caster is Friendly) && target is Hostile) || (caster is Hostile && (target is Player || target is Friendly));
    }
}