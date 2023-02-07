using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FriendlySpell : Spell
{
	public FriendlySpell(string name, string desc, int rsrcCost,  float castTime, int lvlReq, int cD, float minD, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf) : base (name,desc,rsrcCost,castTime,0,lvlReq,cD,minD,effectsOnTarget,effectsOnSelf){


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
