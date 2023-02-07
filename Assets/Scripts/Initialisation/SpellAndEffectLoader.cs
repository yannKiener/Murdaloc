using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellAndEffectLoader : MonoBehaviour {


	void Awake (){
		EffectsOnTime.Add (new EffectOnTime ("Corruption","First DoT of the game",false,1,6,0.5f,null,newDamageOnTime(new Dictionary<Stat,float>{{Stat.intelligence,1.6f}},60)));
		EffectsOnTime.Add (new EffectOnTime ("Renovation","First HoT of the game",true,3,10,1,null,newHealOnTime(new Dictionary<Stat,float>{{Stat.intelligence,1.6f}},50)));
		EffectsOnTime.Add (new EffectOnTime ("Sprint","+60% moveSpeed",true,1,5,1,new StatEffect(new Dictionary<Stat,float>{{Stat.maxSpeed,60f}}),null));

		Spells.Add (new HostileSpell ("Fireball","A magic Fireball. That's it.", 10,2,0,0,5,newDamage(new Dictionary<Stat,float>{{Stat.intelligence,1.6f}},30),null,null));
		Spells.Add (new HostileSpell ("Slam","Slap your target's face with your first", 20,0,0,0,2,newDamage(new Dictionary<Stat,float>{{Stat.force,1f},{Stat.agility,0.5f}},30),null,null));
		Spells.Add (new FriendlySpell ("Renovation","FIRST HOTSPELL", 5,0.5f,0,0,5,null,new List<EffectOnTime>(),new List<EffectOnTime>{ EffectsOnTime.Get("Renovation") }));
		Spells.Add (new FriendlySpell ("Sprint","Gain 60% movement speed for 2 seconds.", 10,0,0,15,1,null,new List<EffectOnTime>(),new List<EffectOnTime>{ EffectsOnTime.Get("Sprint") }));
		Spells.Add (new HostileSpell ("Corruption","FIRST DOT SPELL", 5,0.5f,0,0,5,null,new List<EffectOnTime> { EffectsOnTime.Get("Corruption") },new List<EffectOnTime>()));
        Spells.Add(new HostileSpell("Icelance", "Throw a magic lance on your enemy's face.", 10, 0.2f, 0, 0, 5, newDamage(new Dictionary<Stat, float> { { Stat.intelligence, 1.6f } }, 30), null, null));
    }



	private Action<Character, Character> newHeal(Dictionary<Stat, float> statWeight, int baseHeal){
		return newAction (statWeight, baseHeal, true);
	}

	private Action<Character, Character> newDamage(Dictionary<Stat, float> statWeight, int baseDamage){
		return newAction (statWeight, baseDamage, false);
	}

	private Action<Character, Character> newAction (Dictionary<Stat, float> statWeight, int baseNumber, bool isHeal) {
		float forceMultiplier = 0;
		float agilityMultiplier = 0;
		float intelligenceMultiplier = 0;
		float staminaMultiplier = 0;
		float spiritMultiplier = 0;

		//TODO pas très joli, trouver plus propre.
		foreach (KeyValuePair<Stat,float> p in statWeight) {
			if(p.Key == Stat.force){
				forceMultiplier = p.Value;
			}
			if(p.Key == Stat.agility){
				agilityMultiplier = p.Value;
			}
			if(p.Key == Stat.intelligence){
				intelligenceMultiplier = p.Value;
			}
			if(p.Key == Stat.stamina){
				staminaMultiplier = p.Value;
			}
			if(p.Key == Stat.spirit){
				spiritMultiplier = p.Value;
			}
		}
		if (!isHeal) {
			return ((Character arg1, Character arg2) => {
				int damage = baseNumber;

				Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				damage = damage + (damage * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					damage = damage * 2;
				}
				arg2.ApplyDamage ((int)(damage + damage * UnityEngine.Random.Range (-30f, 30f) / 100), isCrit);
			});
		} else {
			return ((Character arg1, Character arg2) => {
				int heal = baseNumber;

				Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				heal = heal + (heal * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					heal = heal * 2;
				}
				arg2.ApplyHeal ((int)(heal + heal * UnityEngine.Random.Range (-30f, 30f) / 100), isCrit);
			});
		}
	}


	private Action<Character, Character, float, int> newHealOnTime(Dictionary<Stat, float> statWeight, int baseHeal){
		return newActionOntime (statWeight, baseHeal, true);
	}

	private Action<Character, Character, float, int> newDamageOnTime(Dictionary<Stat, float> statWeight, int baseDamage){
		return newActionOntime (statWeight, baseDamage, false);
	}

	private Action<Character, Character, float, int> newActionOntime (Dictionary<Stat, float> statWeight, int baseNumber, bool isHeal) {
		float forceMultiplier = 0;
		float agilityMultiplier = 0;
		float intelligenceMultiplier = 0;
		float staminaMultiplier = 0;
		float spiritMultiplier = 0;

		foreach (KeyValuePair<Stat,float> p in statWeight) {
			if(p.Key == Stat.force){
				forceMultiplier = p.Value;
			}
			if(p.Key == Stat.agility){
				agilityMultiplier = p.Value;
			}
			if(p.Key == Stat.intelligence){
				intelligenceMultiplier = p.Value;
			}
			if(p.Key == Stat.stamina){
				staminaMultiplier = p.Value;
			}
			if(p.Key == Stat.spirit){
				spiritMultiplier = p.Value;
			}
		}
		if (!isHeal) {
			return ((Character arg1, Character arg2, float timedivider, int stacks) => {
				int damage = baseNumber;

				Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				damage = damage + (damage * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					damage = damage * 2;
				}
				arg2.ApplyDamage ((int)((damage + damage * UnityEngine.Random.Range (-30f, 30f) / 100)*stacks*timedivider), isCrit);
			});
		} else {
			return ((Character arg1, Character arg2, float timedivider, int stacks) => {
				int heal = baseNumber;

				Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				heal = heal + (heal * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					heal = heal * 2;
				}
				arg2.ApplyHeal ((int)((heal + heal * UnityEngine.Random.Range (-30f, 30f) / 100)*stacks*timedivider), isCrit);
			});
		}
	}

	// Use this for initialization
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
