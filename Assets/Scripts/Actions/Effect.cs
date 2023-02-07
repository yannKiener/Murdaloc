using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect {

	void apply(Character caster, Character target, int stacks);
	void remove(Character caster, Character target, int stacks);

}

public class StatEffect : Effect{

	private Dictionary<Stat, float> statWeight;
	private float forceAdded = 0;
	private float agilityAdded = 0;
	private float intelligenceAdded = 0;
	private float staminaAdded = 0;
	private float spiritAdded = 0;
	private float criticalAdded = 0;
	private float hasteAdded = 0;
	private float powerAdded = 0;
	private float autoAttackDamageAdded = 0;
	private float autoAttackTimeAdded = 0;
	private float maxSpeedAdded = 0;
	private float maxResourceAdded = 0;
	private float maxLifeAdded = 0;

	public StatEffect(Dictionary<Stat, float> statWeight){
		this.statWeight = statWeight;
	}

	public void apply(Character caster, Character target, int stacks){
		foreach (KeyValuePair<Stat,float> p in statWeight) {
			if (p.Key == Stat.force) {
				forceAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.agility) {
				agilityAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.intelligence) {
				intelligenceAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.stamina) {
				staminaAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.spirit) {
				spiritAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.critical) {
				criticalAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.haste) {
				hasteAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.power) {
				powerAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.autoAttackTime) {
				autoAttackTimeAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.autoAttackDamage) {
				autoAttackDamageAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.maxSpeed) {
				maxSpeedAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.maxResource) {
				maxResourceAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
			if (p.Key == Stat.maxLife) {
				maxLifeAdded = target.GetStats ().AddPercent (p.Key, p.Value * stacks);
			}
		}
	}

	public void remove (Character caster, Character target, int stacks){
		foreach (KeyValuePair<Stat,float> p in statWeight) {
			if (p.Key == Stat.force) {
				target.GetStats ().Force -= (int)forceAdded;
			}
			if (p.Key == Stat.agility) {
				target.GetStats ().Agility -= (int)agilityAdded ;
			}
			if (p.Key == Stat.intelligence) {
				target.GetStats ().Intelligence -= (int)intelligenceAdded;
			}
			if (p.Key == Stat.stamina) {
				target.GetStats ().Stamina -= (int)staminaAdded;
			}
			if (p.Key == Stat.spirit) {
				target.GetStats ().Spirit -= (int)spiritAdded;
			}
			if (p.Key == Stat.critical) {
				target.GetStats ().Critical -= (int)criticalAdded;
			}
			if (p.Key == Stat.haste) {
				target.GetStats ().Haste -= (int)hasteAdded;
			}
			if (p.Key == Stat.power) {
				target.GetStats ().Power -= (int)powerAdded;
			}
			if (p.Key == Stat.autoAttackTime) {
				target.GetStats ().AutoAttackTime -= autoAttackTimeAdded;
			}
			if (p.Key == Stat.autoAttackDamage) {
				target.GetStats ().AutoAttackDamage -= (int)autoAttackDamageAdded;
			}
			if (p.Key == Stat.maxSpeed) {
				target.GetStats ().MaxSpeed -= maxSpeedAdded;
			}
			if (p.Key == Stat.maxResource) {
				target.GetStats ().MaxResource -= (int)maxResourceAdded;
			}
			if (p.Key == Stat.maxLife) {
				target.GetStats ().MaxLife -= (int)maxLifeAdded;
			}
		}
	}

	private float getValueIfContains(Dictionary<Stat, float> statWeight, Stat s){
		if(statWeight.ContainsKey(s)){
			return statWeight [s];
		}
		return 0;
	}
}