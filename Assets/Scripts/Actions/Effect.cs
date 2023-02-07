using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect {

	void apply(Character caster, Character target);
	void remove(Character caster, Character target);

}

public class StatModifier : Effect{

	private float forceMultiplier;
	private float agilityMultiplier;
	private float intelligenceMultiplier;
	private float staminaMultiplier;
	private float spiritMultiplier;
	private float criticalMultiplier;
	private float hasteMultiplier;
	private float powerMultiplier;
	private float autoAttackDamageMultiplier;
	private float autoAttackTimeMultiplier;
	private float maxSpeedMultiplier;
	private float maxResourceMultiplier;
	private float maxLifeMultiplier;

	public StatModifier(Dictionary<Stat, float> statWeight){
		forceMultiplier = getValueIfContains(statWeight, Stat.force);
		agilityMultiplier = getValueIfContains(statWeight, Stat.agility);
		intelligenceMultiplier = getValueIfContains(statWeight, Stat.intelligence);
		staminaMultiplier = getValueIfContains(statWeight, Stat.stamina);
		spiritMultiplier = getValueIfContains(statWeight, Stat.spirit);
		criticalMultiplier = getValueIfContains(statWeight, Stat.critical);
		hasteMultiplier = getValueIfContains(statWeight, Stat.haste);
		powerMultiplier = getValueIfContains(statWeight, Stat.power);
		autoAttackDamageMultiplier = getValueIfContains(statWeight, Stat.autoAttackDamage);
		autoAttackTimeMultiplier= getValueIfContains(statWeight, Stat.autoAttackTime);
		maxSpeedMultiplier = getValueIfContains(statWeight, Stat.maxSpeed);
		maxResourceMultiplier = getValueIfContains(statWeight, Stat.maxResource);
		maxLifeMultiplier = getValueIfContains(statWeight, Stat.maxLife);
	}

	public void apply(Character caster, Character target){
		target.GetStats ().MultiplyBy (maxLifeMultiplier, maxResourceMultiplier, forceMultiplier, agilityMultiplier, intelligenceMultiplier, staminaMultiplier, spiritMultiplier, criticalMultiplier, hasteMultiplier, powerMultiplier, autoAttackDamageMultiplier, autoAttackTimeMultiplier, maxSpeedMultiplier);
	}

	public void remove (Character caster, Character target){
		target.GetStats ().DivideBy (maxLifeMultiplier, maxResourceMultiplier, forceMultiplier, agilityMultiplier, intelligenceMultiplier, staminaMultiplier, spiritMultiplier, criticalMultiplier, hasteMultiplier, powerMultiplier, autoAttackDamageMultiplier, autoAttackTimeMultiplier, maxSpeedMultiplier);
	}

	private float getValueIfContains(Dictionary<Stat, float> statWeight, Stat s){
		if(statWeight.ContainsKey(s)){
			return statWeight [s];
		}
		return 0;
	}
}