using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class HostileSpell : Spell
{
	public HostileSpell(string name, string desc, int rsrcCost,  float castTime, int damage, int lvlReq, int cD, float minD) : base (name,desc,rsrcCost,castTime,damage,lvlReq,cD,minD){


	}

	public HostileSpell(string name, string desc, int rsrcCost,  float castTime, int damage, int lvlReq, int cD, float minD, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf) : base (name,desc,rsrcCost,castTime,damage,lvlReq,cD,minD,effectsOnTarget,effectsOnSelf){


	}

	public override void Cast(Character caster, Character target)
	{
		if (IsCastable(caster, target))
		{
			base.Cast (caster, target);
			target.ApplyDamage (modifiedSpell(caster, target, damage), isCrit);
		}
		else
		{
			//Impossible de lancer le sort
		}

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