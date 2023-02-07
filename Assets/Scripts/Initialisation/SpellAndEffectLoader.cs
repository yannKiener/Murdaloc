using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpellAndEffectLoader : MonoBehaviour {


	void Awake (){
        CreateEffectOnTime("Corruption","First DoT of the game",false,1,6,0.5f,null,newDamageOnTime(new Dictionary<Stat,float>{{Stat.intelligence,1.6f}},60));
		CreateEffectOnTime("Renovation","First HoT of the game",true,3,10,1,null,newHealOnTime(new Dictionary<Stat,float>{{Stat.intelligence,1.6f}},50));
		CreateEffectOnTime("Sprint","+60% moveSpeed",true,1,5,1,new StatEffect(new Dictionary<Stat,float>{{Stat.maxSpeed,60f}}),null);

        CreateHostileSpell("Fireball","A magic Fireball. That's it.", 10,2,0,0,5,newDamage(new Dictionary<Stat,float>{{Stat.intelligence,1.6f}},30),null,null);
        CreateHostileSpell("Slam","Slap your target's face with your first", 20,0,0,0,2,newDamage(new Dictionary<Stat,float>{{Stat.force,1f},{Stat.agility,0.5f}},10,0.8f),null,null);
        CreateFriendlySpell("Renovation","FIRST HOTSPELL", 5,0.5f,0,0,5,null,new List<EffectOnTime>(),new List<EffectOnTime>{ EffectsOnTime.Get("Renovation") });
        CreateFriendlySpell("Sprint","Gain 60% movement speed for 2 seconds.", 10,0,0,15,1,null,new List<EffectOnTime>(),new List<EffectOnTime>{ EffectsOnTime.Get("Sprint") });
        CreateHostileSpell("Corruption","FIRST DOT SPELL", 5,0.5f,0,0,5,null,new List<EffectOnTime> { EffectsOnTime.Get("Corruption") },new List<EffectOnTime>());
        CreateHostileSpell("Icelance", "Throw a magic lance on your enemy's face.", 10, 0.2f, 0, 0, 5, newDamage(new Dictionary<Stat, float> { { Stat.intelligence, 1.6f } }, 30), null, null);

        //Spells for consummables
        CreateEffectOnTime("Food", "Regen health while not fighting.", true, 1, 10, 2, null, AddLifePercentOverTime(20,true));
        CreateEffectOnTime("Bevrage", "Regen mana while not fighting.", true, 1, 10, 2, null, AddManaPercentOverTime(20, true));
        CreateFriendlySpell("Food", "Eat.", 0, 0, 0, 0, 2, null, null, new List<EffectOnTime>() { EffectsOnTime.Get("Food") });
        CreateFriendlySpell("Bevrage", "Drink.", 0, 0, 0, 0, 2, null, null, new List<EffectOnTime>() { EffectsOnTime.Get("Bevrage") });
        CreateFriendlySpell("Potion25", "Drink a small health potion", 0, 0, 0, 0, 2, AddLifePercent(25), null, null);
        CreateFriendlySpell("Potion40", "Drink a normal health potion", 0, 0, 0, 0, 2, AddLifePercent(40), null, null);
        CreateFriendlySpell("Potion60", "Drink a big health potion", 0, 0, 0, 0, 2, AddLifePercent(60), null, null);
        CreateEffectOnTime("Potion of cunning", "You feel 10% smarter !", true, 1, 60 * 10, 60, new StatEffect(new Dictionary<Stat, float>{ { Stat.intelligence, 10 } }) , null);
        CreateFriendlySpell("PotionIntell10", "Drink an intelligence potion", 0, 0, 0, 0, 2, null, null, new List<EffectOnTime>() { EffectsOnTime.Get("Potion of cunning") });
        CreateEffectOnTime("Potion of might", "You feel 10% stronger !", true, 1, 60 * 10, 60, new StatEffect(new Dictionary<Stat, float> { { Stat.force, 10 } }), null);
        CreateFriendlySpell("PotionForce10", "Drink a force potion", 0, 0, 0, 0, 2, null, null, new List<EffectOnTime>() { EffectsOnTime.Get("Potion of might") });
        CreateEffectOnTime("Potion of deftness", "You feel 10% more agile !", true, 1, 60 * 10, 60, new StatEffect(new Dictionary<Stat, float> { { Stat.agility, 10 } }), null);
        CreateFriendlySpell("PotionAgi10", "Drink an agility potion", 0, 0, 0, 0, 2, null, null, new List<EffectOnTime>() { EffectsOnTime.Get("Potion of deftness") });

    }


    private void CreateEffectOnTime(string name, string description, bool isBuff, int maxStacks, float duration, float timePerTic, Effect applyOnce, Action<Character, Character, float, int> tic)
    {
        EffectsOnTime.Add(new EffectOnTime(name, description, isBuff, maxStacks, duration, timePerTic, applyOnce, tic));
    }
   
    private void CreateHostileSpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown, float maxDistance, Action<Character, Character> spellEffect, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf)
    {
        Spells.Add(new HostileSpell(name, description, resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, effectsOnTarget, effectsOnSelf));
    }

    private void CreateFriendlySpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown, float maxDistance, Action<Character, Character> spellEffect, List<EffectOnTime> effectsOnTarget, List<EffectOnTime> effectsOnSelf)
    {
        Spells.Add(new FriendlySpell(name, description, resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, effectsOnTarget, effectsOnSelf));
    }



    private Action<Character, Character> AddLifePercent(int lifePercent)
    {
        return ((Character arg1, Character arg2) => { arg1.ApplyHeal(arg1.GetMaxLife() * lifePercent / 100); });
    }

    private Action<Character, Character> RemoveLifePercent(int lifePercent)
    {
        return ((Character arg1, Character arg2) => { arg1.ApplyDamage(arg1.GetMaxLife() * lifePercent / 100); });
    }

    private Action<Character, Character> AddManaPercent(int manaPercent)
    {
        return ((Character arg1, Character arg2) => { if (arg1.GetResourceType() is Mana) { arg1.AddResource(arg1.GetMaxResource() * manaPercent / 100); } });
    }

    private Action<Character, Character> RemoveManaPercent(int manaPercent)
    {
        return ((Character arg1, Character arg2) => { if (arg1.GetResourceType() is Mana) { arg1.RemoveResource(arg1.GetMaxResource() * manaPercent / 100); } });
    }

    private Action<Character, Character> newHeal(Dictionary<Stat, float> statWeight, int baseHeal){
		return newAction (statWeight, baseHeal, true);
	}

	private Action<Character, Character> newDamage(Dictionary<Stat, float> statWeight, int baseDamage, float autoAttackMultiplier = 0)
    {
		return newAction (statWeight, baseDamage, false, autoAttackMultiplier);
	}

	private Action<Character, Character> newAction (Dictionary<Stat, float> statWeight, int baseNumber, bool isHeal, float autoAttackMultiplier = 0) {
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
                damage += (int)(arg1.GetAutoAttack1Damage() * autoAttackMultiplier);
                damage += (int)(arg1.GetStats().Intelligence * intelligenceMultiplier);
                damage += (int)(arg1.GetStats().Force * forceMultiplier);
                damage += (int)(arg1.GetStats().Agility * agilityMultiplier);
                damage += (int)(arg1.GetStats().Stamina * staminaMultiplier);
                damage += (int)(arg1.GetStats().Spirit * spiritMultiplier);

                Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				damage = damage + (damage * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					damage = damage * 2;
				}
				arg2.ApplyDamage ((int)(damage + damage * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100), isCrit);
			});
		} else {
			return ((Character arg1, Character arg2) => {
				int heal = baseNumber;
                heal += (int)(arg1.GetAutoAttack1Damage() * autoAttackMultiplier);
                heal += (int)(arg1.GetStats().Intelligence * intelligenceMultiplier);
                heal += (int)(arg1.GetStats().Force * forceMultiplier);
                heal += (int)(arg1.GetStats().Agility * agilityMultiplier);
                heal += (int)(arg1.GetStats().Stamina * staminaMultiplier);
                heal += (int)(arg1.GetStats().Spirit * spiritMultiplier);

                Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				heal = heal + (heal * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					heal = heal * 2;
				}
				arg2.ApplyHeal ((int)(heal + heal * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100), isCrit);
			});
		}
	}


    //EffectsOnTime


    private Action<Character, Character, float, int> AddLifePercentOverTime(int lifePercent, bool mustNotBeFighting = false)
    {
        return ((Character arg1, Character arg2, float timedivider, int stacks) => { if (!mustNotBeFighting || !arg1.IsInCombat()) { arg1.ApplyHeal((int)(arg1.GetMaxLife() * lifePercent / 100 * stacks)); } });
    }

    private Action<Character, Character, float, int> RemoveLifePercentOverTime(int lifePercent)
    {
        return ((Character arg1, Character arg2, float timedivider, int stacks) => { arg1.ApplyDamage((int)(arg1.GetMaxLife() * lifePercent / 100 * stacks)); });
    }

    private Action<Character, Character, float, int> AddManaPercentOverTime(int manaPercent, bool mustNotBeFighting = false)
    {
        return ((Character arg1, Character arg2, float timedivider, int stacks) => { if (arg1.GetResourceType() is Mana && (!mustNotBeFighting || !arg1.IsInCombat())) { arg1.AddResource((int)(arg1.GetMaxResource() * manaPercent / 100 * stacks)); } });
    }
    private Action<Character, Character, float, int> RemoveManaPercentOverTime(int manaPercent)
    {
        return ((Character arg1, Character arg2, float timedivider, int stacks) => { if (arg1.GetResourceType() is Mana) { arg1.RemoveResource((int)(arg1.GetMaxResource() * manaPercent / 100 * stacks)); } });
    }



    private Action<Character, Character, float, int> newHealOnTime(Dictionary<Stat, float> statWeight, int totalHeal){
		return newActionOntime (statWeight, totalHeal, true);
	}

	private Action<Character, Character, float, int> newDamageOnTime(Dictionary<Stat, float> statWeight, int totalDamage){
		return newActionOntime (statWeight, totalDamage, false);
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
                damage += (int)(arg1.GetStats().Intelligence * intelligenceMultiplier);
                damage += (int)(arg1.GetStats().Force * forceMultiplier);
                damage += (int)(arg1.GetStats().Agility * agilityMultiplier);
                damage += (int)(arg1.GetStats().Stamina * staminaMultiplier);
                damage += (int)(arg1.GetStats().Spirit * spiritMultiplier);

                Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				damage = damage + (damage * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					damage = damage * 2;
				}
				arg2.ApplyDamage ((int)((damage + damage * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100)*stacks*timedivider), isCrit);
			});
		} else {
			return ((Character arg1, Character arg2, float timedivider, int stacks) => {
				int heal = baseNumber;
                heal += (int)(arg1.GetStats().Intelligence * intelligenceMultiplier);
                heal += (int)(arg1.GetStats().Force * forceMultiplier);
                heal += (int)(arg1.GetStats().Agility * agilityMultiplier);
                heal += (int)(arg1.GetStats().Stamina * staminaMultiplier);
                heal += (int)(arg1.GetStats().Spirit * spiritMultiplier);

                Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				heal = heal + (heal * casterStats.Power / 100); //Applying power 
				if (isCrit) { // Apply Crit
					heal = heal * 2;
				}
				arg2.ApplyHeal ((int)((heal + heal * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100)*stacks*timedivider), isCrit);
			});
		}
	}

	// Use this for initialization
	void Start () {
        InitializeFindUtils();
        Items.InitializeCategories();
    }
	

    private void InitializeFindUtils()
    {
        FindUtils.GetDialogPanel();
        FindUtils.GetDialogPanelComponent();
        FindUtils.GetCharacterSheetGrid();
        FindUtils.GetCharacterSheetText();
        FindUtils.GetLootGrid();
        FindUtils.GetSpellBookGrid();
        FindUtils.GetPlayer();
        FindUtils.GetInventoryGrid();
        FindUtils.GetQuestGridGameObject();
        FindUtils.GetQuestGrid();
        FindUtils.GetDialogBoxComponent();
        FindUtils.GetDialogBox().SetActive(false);
        FindUtils.GetCharacterSheet().SetActive(false);
        FindUtils.GetInventory().SetActive(false);
        FindUtils.GetSpellBook().SetActive(false);
        FindUtils.GetLoot().SetActive(false);
        FindUtils.GetQuestLog().SetActive(false);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
