using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FriendlySpell : AbstractSpell
{
	public FriendlySpell() : base (){
	}


	public FriendlySpell(string name, string desc, int rsrcCost,  float castTime, int lvlReq, int cD, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf) : base (name,desc,rsrcCost,castTime,0,lvlReq,cD,effectsOnTarget,effectsOnSelf){


	}
	public override void Cast(Character caster, Character target)
	{
		if (IsCastable(caster,target))
		{
			if (target != null) {
				target = caster;
			}
			base.Cast (caster, target);
			caster.AddResource (caster.GetMaxResource() - caster.GetCurrentResource ());
			caster.ApplyHeal (modifiedSpell(caster, target, 100));
		}
		else
		{
			//Impossible de lancer le sort
		}

	}

	public override bool IsCastable(Character caster, Character target){
		return base.IsCastable (caster, target);
	}
}
