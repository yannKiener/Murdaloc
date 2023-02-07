using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect {

	void apply(Character caster, Character target);
	void remove(Character caster, Character target);

}

public class StatEffect : Effect{

	private Dictionary<Stat, float> statWeight;
	private int forceAdded;
	private int agilityAdded;
	private int intelligenceAdded;
	private int staminaAdded;
	private int spiritAdded;
	private int criticalAdded;
	private int hasteAdded;
	private int powerAdded;
	private int autoAttackDamageAdded;
	private float autoAttackTimeAdded;
	private float maxSpeedAdded;
	private int maxResourceAdded;
	private int maxLifeAdded;

	public StatEffect(Dictionary<Stat, float> statWeight){
		this.statWeight = statWeight;
	}

	public void apply(Character caster, Character target){
		/*
		foreach (KeyValuePair<Stat,float> p in statWeight) {
			if (p.Key == Stat.force) {
				forceAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.agility) {
				agilityAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.intelligence) {
				intelligenceAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.stamina) {
				staminaAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.spirit) {
				spiritAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.critical) {
				criticalAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.haste) {
				hasteAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.power) {
				powerAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.autoAttackTime) {
				autoAttackTimeAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.autoAttackDamage) {
				autoAttackDamageAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.maxSpeed) {
				maxSpeedAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.maxResource) {
				maxResourceAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
			if (p.Key == Stat.maxLife) {
				maxLifeAdded = target.GetStats ().AddPercent (p.Key, p.Value);
			}
		}*/
	}

	public void remove (Character caster, Character target){
	}

	private float getValueIfContains(Dictionary<Stat, float> statWeight, Stat s){
		if(statWeight.ContainsKey(s)){
			return statWeight [s];
		}
		return 1;
	}
}