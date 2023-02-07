using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class SpellAndEffectLoader : MonoBehaviour {


	void Awake ()
    {
        CreateEffectsOnTime();
        CreateSpells();
        CreateSpecialisations();
        Items.InitializeCategories();
        CreateDialogActions();
    }

    /*
     * number for talent tree : 
     * 1 - 2 - 3 - 4
     * 5 - 6 - 7 - 8
     * 9 - 10 - 11 - 12
     * 13 - 14 - 15 - 16
     * 17 - 18 - 19 - 20
     * 21 - 22 - 23 - 24
     * 25 - 26 - 27 - 28
     */
    private void CreateSpecialisations()
    {
        Specialisation mageFire = new Specialisation("Fire");

        mageFire.SetTalent(2, new Talent("Burning bolts", "Add 10% chance to burn your target with a Fireball. Stackable.", 5, (player, stacks) => player.GetSpells()["Fireball"].SetProc(EffectsOnTime.Get("Burning"),stacks*10), (player, stacks) => player.GetSpells()["Fireball"].RemoveProc(EffectsOnTime.Get("Burning"))));
        mageFire.SetTalent(3, new Talent("Overpowered Firebolt", "add 10% damage to your Fireball spell", 5, (player, stacks) => player.GetSpells()["Fireball"].AddToNormalMultiplier(10), (player, stacks) => player.GetSpells()["Fireball"].RemoveToNormalMultiplier(10)));
        mageFire.SetTalent(6, new Talent("Fast Firebolt", "Reduce FireBall's casting time by 0,1s.", 5, (player, stacks) => player.GetSpells()["Fireball"].RemoveCastTime(0.1f), (player, stacks) => player.GetSpells()["Fireball"].AddCastTime(0.1f)));
        mageFire.SetTalent(7, new Talent("Fire explosion", "Learn new spell : Fire explosion", 1, (player, stacks) => player.AddSpell(Spells.Get("Fire explosion")), (player, stacks) => player.RemoveSpell("Fire explosion")));
        mageFire.SetTalent(11, new Talent("Meteor storm", "Learn new spell : Meteor storm", 1, (player, stacks) => player.AddSpell(Spells.Get("Meteor storm")), (player, stacks) => player.RemoveSpell("Meteor storm")));
        mageFire.SetTalent(16, new Talent("Hasty criticals", "Add 1% crit and 1% haste. Stackable.", 5, (player, stacks) => { player.GetStats().AddStat(Stat.critical, 1); player.GetStats().AddStat(Stat.haste, 1); }, (player, stacks) => { player.GetStats().AddStat(Stat.critical, -1); player.GetStats().AddStat(Stat.haste, -1); }));

        Specialisations.Add(mageFire);

        Specialisation mageFrost = new Specialisation("Frost");
        mageFrost.SetTalent(1, new Talent("Frost nova", "Learn new spell : Frost nova", 1, (player, stacks) => player.AddSpell(Spells.Get("Frost nova")), (player, stacks) => player.RemoveSpell("Frost nova")));
        Talent icelance = new Talent("Ice Lance", "Learn new spell : IceLance", 1, (player, stacks) => player.AddSpell(Spells.Get("Icelance")), (player, stacks) => player.RemoveSpell("Icelance"));
        mageFrost.SetTalent(2, icelance);
        mageFrost.SetTalent(3, new Talent("Critical ice", "Your Ice Lance freeze your target on critical.", 1, (player, stacks) => { if (player.GetSpells().ContainsKey("Icelance")) { player.GetSpells()["Icelance"].SetActionOnCrit((pl, tar) => { EffectsOnTime.Get("Frozen").Apply(pl, tar); }); } }, (player, stacks) => { if (player.GetSpells().ContainsKey("Icelance")) { player.GetSpells()["Icelance"].RemoveActionOnCrit(); } }, icelance));
        mageFrost.SetTalent(4, new Talent("Freezing Lance", "Your Ice Lance slow target by 60%", 1, (player, stacks) => { if (player.GetSpells().ContainsKey("Icelance")) { player.GetSpells()["Icelance"].AddEffectOnTarget(EffectsOnTime.Get("Hypothermia")); } }, (player, stacks) => { if (player.GetSpells().ContainsKey("Icelance")) { player.GetSpells()["Icelance"].RemoveEffectOnTarget("Hypothermia"); } }, icelance));
        Specialisations.Add(mageFrost);



        Specialisation mageArcane = new Specialisation("Arcane");
        mageArcane.SetTalent(4, new Talent("Empowered Corruption", "add 20% damage to your Corruption spell", 5, (player, stacks) => player.GetSpells()["Corruption"].GetEffectOnTarget("Corruption").AddToNormalMultiplier(20), (player, stacks) => player.GetSpells()["Corruption"].GetEffectOnTarget("Corruption").RemoveToNormalMultiplier(20)));
        mageArcane.SetTalent(2, new Talent("Empowered Renovation", "add 20% heal to your Renovation spell", 5, (player, stacks) => player.GetSpells()["Renovation"].GetEffectOnSelf("Renovation").AddToNormalMultiplier(20), (player, stacks) => player.GetSpells()["Renovation"].GetEffectOnSelf("Renovation").RemoveToNormalMultiplier(20)));
        mageArcane.SetTalent(6, new Talent("Empowered Slam", "add 20% chance to stun your target", 5, (player, stacks) => player.GetSpells()["Slam"].SetProc(EffectsOnTime.Get("Stun"),stacks * 20), (player, stacks) => player.GetSpells()["Slam"].RemoveProc(EffectsOnTime.Get("Stun"))));

        Specialisations.Add(mageArcane);

    }

    private void CreateEffectsOnTime()
    {
        CreateEffectOnTime("Corruption", "First DoT of the game", false, 1, 6, 1f, null, newDamageOnTime(new Dictionary<Stat, float> { { Stat.intelligence, 1.6f } }, 60));
        CreateEffectOnTime("Burning", "Inflict fire damage every two seconds.", false, 3, 10, 2f, null, newDamageOnTime(new Dictionary<Stat, float> { { Stat.intelligence, 0.5f } }, 10));
        CreateEffectOnTime("Renovation", "First HoT of the game", true, 3, 10, 1, null, newHealOnTime(new Dictionary<Stat, float> { { Stat.intelligence, 2f } }, 50));
        CreateEffectOnTime("Sprint", "+60% moveSpeed", true, 1, 5, 1, new StatEffect(new Dictionary<Stat, float> { { Stat.maxSpeed, 60f } }), null);
        CreateEffectOnTime("Hypothermia", "Slower movement speed.", false, 1, 6, 1, new StatEffect(new Dictionary<Stat, float> { { Stat.maxSpeed, -60f } }), null);
        CreateEffectOnTime("Frozen", "Can't move", false, 1, 6, 1, new StatEffect(new Dictionary<Stat, float> { { Stat.maxSpeed, -100f } }), null);
        CreateEffectOnTime("Enrage", "Hit harder & faster but moves slower.", true, 1, 8, 8, new StatEffect(new Dictionary<Stat, float> { { Stat.maxSpeed, -50f }, { Stat.haste, 20f }, { Stat.power, 20f } }), null);
        CreateEffectOnTime("Webbed", "Can't move", false, 1, 3.5f, 3, new StatEffect(new Dictionary<Stat, float> { { Stat.maxSpeed, -100f } }), null);
        CreateEffectOnTime("Stun", "Stunned. Can't move or attack.", false, 1, 5, 5, new StunEffect(), null);


        //Spells for consummables
        CreateEffectOnTime("Food", "Regen health while not fighting.", true, 1, 10, 2, null, AddLifePercentOverTime(20, true));
        CreateEffectOnTime("Drink", "Regen mana while not fighting.", true, 1, 10, 2, null, AddManaPercentOverTime(20, true));
        CreateEffectOnTime("Potion of cunning", "You feel 10% smarter !", true, 1, 60 * 10, 60, new StatEffect(new Dictionary<Stat, float> { { Stat.intelligence, 10 } }), null);
        CreateEffectOnTime("Potion of might", "You feel 10% stronger !", true, 1, 60 * 10, 60, new StatEffect(new Dictionary<Stat, float> { { Stat.force, 10 } }), null);
        CreateEffectOnTime("Potion of deftness", "You feel 10% more agile !", true, 1, 60 * 10, 60, new StatEffect(new Dictionary<Stat, float> { { Stat.agility, 10 } }), null);

        //Used for Intro
        CreateEffectOnTime("Asleep", "You're emerging from a coma", false, 1, 31536000, 5, new StunEffect(), null);
    }

    private void CreateSpells()
    {
        CreateHostileSpell("Fireball", "A magic Fireball. That's it.", 10, 2, 0, 0, 5, newDamage(new Dictionary<Stat, float> { { Stat.intelligence, 1.6f } }, 30), "Fire", null, null);
        CreateHostileSpell("Slam", "Slap your target's face with your first", 20, 0, 0, 0, 2, newDamage(new Dictionary<Stat, float> { { Stat.force, 1f }, { Stat.agility, 0.5f } }, 10, 0.4f), "Default", null, null);
        CreateFriendlySpell("Renovation", "Heal over time.", 5, 0.5f, 0, 0, 5, null, "Holy", new List<EffectOnTime>(), new List<EffectOnTime> { EffectsOnTime.Get("Renovation") });
        CreateFriendlySpell("Sprint", "Gain 60% movement speed for 2 seconds.", 10, 0, 0, 15, 1, null, "Sprint", new List<EffectOnTime>(), new List<EffectOnTime> { EffectsOnTime.Get("Sprint") });
        CreateHostileSpell("Corruption", "Damages over time.", 5, 0.5f, 0, 0, 5, null, "Shadow", new List<EffectOnTime> { EffectsOnTime.Get("Corruption") }, new List<EffectOnTime>());
        CreateHostileSpell("Icelance", "Throw a magic lance on your enemy's face.", 10, 0.2f, 0, 0, 5, newDamage(new Dictionary<Stat, float> { { Stat.intelligence, 0.6f } }, 30), "Frost", null, null);
        CreateHostileSpell("Meteor storm", "A meteor fall down the sky and damages targets in area", 50, 4, 1, 8, 8, newZoneDamage(new Dictionary<Stat, float> { { Stat.intelligence, 1.6f } },60,5), "Fire", null, null);
        CreateHostileSpell("Fire explosion", "A terrible Fire explosion based on your WEAPON damage (yeah testing purpose)", 50, 0, 1, 3, 2, newZoneDamage(new Dictionary<Stat, float> { { Stat.force, 0.6f } }, 0, 2,true,1), "Fire", null, null);
        CreateHostileSpell("Frost nova", "A frost nova imported from WOW", 50, 0, 5, 12, 3, newZoneDamage(new Dictionary<Stat, float> { { Stat.intelligence, 0f } }, 10, 3, true, 1), "FrostNova", new List<EffectOnTime>() { EffectsOnTime.Get("Frozen") }, null);


        //Rogue Spells
        CreateHostileSpell("Hemorrhage", "A fast attack with your main hand + 70% of your agility.", 35, 0, 0, 0, Constants.MaxAutoAttackDistance, newDamage(new Dictionary<Stat, float> { { Stat.agility, 0.7f } }, 10, 1f), "Default", null, null);



        //Base spells
        CreateFriendlySpell("Astral Recall", "Teleports you through the twisting nether back to a safe place.", 0, 4, 0, 30, 1, new Action<Character, Character, Spell>(((Character arg1, Character arg2, Spell sp) => { if (!arg1.IsInCombat()) { arg1.transform.position = FindUtils.GetPlayer().GetInitialPosition(); } })), "Default", new List<EffectOnTime>(), new List<EffectOnTime>());

        //Spells for consummables
        CreateFriendlySpell("Food", "Eat.", 0, 0, 0, 0, 2, null, "Food", null, new List<EffectOnTime>() { EffectsOnTime.Get("Food") });
        CreateFriendlySpell("Drink", "Drink.", 0, 0, 0, 0, 2, null, "Drink", null, new List<EffectOnTime>() { EffectsOnTime.Get("Drink") });
        CreateFriendlySpell("Potion25", "Drink a small health potion", 0, 0, 0, 0, 2, AddLifePercent(25), "Potion", null, null);
        CreateFriendlySpell("Potion40", "Drink a normal health potion", 0, 0, 0, 0, 2, AddLifePercent(40), "Potion", null, null);
        CreateFriendlySpell("Potion60", "Drink a big health potion", 0, 0, 0, 0, 2, AddLifePercent(60), "Potion", null, null);
        CreateFriendlySpell("PotionIntell10", "Drink an intelligence potion", 0, 0, 0, 0, 2, null, "Potion", null, new List<EffectOnTime>() { EffectsOnTime.Get("Potion of cunning") });
        CreateFriendlySpell("PotionForce10", "Drink a force potion", 0, 0, 0, 0, 2, null, "Potion", null, new List<EffectOnTime>() { EffectsOnTime.Get("Potion of might") });
        CreateFriendlySpell("PotionAgi10", "Drink an agility potion", 0, 0, 0, 0, 2, null, "Potion", null, new List<EffectOnTime>() { EffectsOnTime.Get("Potion of deftness") });

        //Spells for Mobs
        CreateFriendlySpell("Enrage", "Hit harder & faster but move slower.", 50, 0, 5, 20, 2, null, "Bloodlust", null, new List<EffectOnTime>() { EffectsOnTime.Get("Enrage") });
        CreateHostileSpell("Web", "Send a web to your target and prevent him from moving.", 20, 1.5f, 2, 15, 4, null, "Trap", new List<EffectOnTime>() { EffectsOnTime.Get("Webbed") }, null);
    }   


    private void CreateEffectOnTime(string name, string description, bool isBuff, int maxStacks, float duration, float timePerTic, Effect applyOnce, Action<Character, Character, EffectOnTime> tic)
    {
        EffectsOnTime.Add(new EffectOnTime(name, description, isBuff, maxStacks, duration, timePerTic, applyOnce, tic));
    }
   
    private void CreateHostileSpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown, float maxDistance, Action<Character, Character,Spell> spellEffect, string soundType , List<EffectOnTime> effectsOnTarget , List<EffectOnTime> effectsOnSelf )
    {
        Spells.Add(new HostileSpell(name, description, resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, soundType, effectsOnTarget, effectsOnSelf));
    }

    private void CreateFriendlySpell(string name, string description, int resourceCost, float castTime, int levelRequirement, int coolDown, float maxDistance, Action<Character, Character,Spell> spellEffect, string soundType , List<EffectOnTime> effectsOnTarget , List<EffectOnTime> effectsOnSelf )
    {
        Spells.Add(new FriendlySpell(name, description, resourceCost, castTime, levelRequirement, coolDown, maxDistance, spellEffect, soundType, effectsOnTarget, effectsOnSelf));
    }



    private Action<Character, Character, Spell> AddLifePercent(int lifePercent)
    {
        return ((Character arg1, Character arg2, Spell sp) => { arg1.ApplyHeal(arg1.GetMaxLife() * lifePercent / 100); });
    }

    private Action<Character, Character, Spell> RemoveLifePercent(int lifePercent)
    {
        return ((Character arg1, Character arg2, Spell sp) => { arg1.ApplyDamage(arg1.GetMaxLife() * lifePercent / 100); });
    }

    private Action<Character, Character, Spell> AddManaPercent(int manaPercent)
    {
        return ((Character arg1, Character arg2, Spell sp) => { if (arg1.GetResourceType() is Mana) { arg1.AddResource(arg1.GetMaxResource() * manaPercent / 100); } });
    }

    private Action<Character, Character, Spell> RemoveManaPercent(int manaPercent)
    {
        return ((Character arg1, Character arg2, Spell sp) => { if (arg1.GetResourceType() is Mana) { arg1.RemoveResource(arg1.GetMaxResource() * manaPercent / 100); } });
    }

    private Action<Character, Character, Spell> newHeal(Dictionary<Stat, float> statWeight, int baseHeal){
		return newAction (statWeight, baseHeal, true);
	}

	private Action<Character, Character,Spell> newDamage(Dictionary<Stat, float> statWeight, int baseDamage, float autoAttackMultiplier = 0)
    {
		return newAction (statWeight, baseDamage, false, autoAttackMultiplier);
	}

    private Action<Character, Character, Spell> newZoneDamage(Dictionary<Stat, float> statWeight, int baseNumber, float zoneDistance, bool isFromSelf = false, float autoAttackMultiplier = 0)
    {
        return ((Character caster, Character target, Spell spell) =>
        {
            Collider2D[] hitColliders;
            if (isFromSelf)
            {
                hitColliders = Physics2D.OverlapCircleAll(new Vector2(caster.transform.position.x, caster.transform.position.y), zoneDistance);
            }
            else
            {
                hitColliders = Physics2D.OverlapCircleAll(new Vector2(target.transform.position.x, target.transform.position.y), zoneDistance);
            }

            foreach(Collider2D col in hitColliders)
            {
                Character targetInZone = col.GetComponent<Character>();
                if (targetInZone != null && ((caster is Player && targetInZone is Hostile) || (caster is Hostile && (targetInZone is Player || targetInZone is Friendly))))
                {
                    foreach(KeyValuePair<string, EffectOnTime> effect in spell.GetEffectsOnTarget())
                    {
                        effect.Value.Apply(caster, targetInZone);
                    }
                    newAction(statWeight, baseNumber, false, autoAttackMultiplier)(caster, targetInZone, spell);
                }
            }
        });
    }

    private Action<Character, Character, Spell> newAction (Dictionary<Stat, float> statWeight, int baseNumber, bool isHeal, float autoAttackMultiplier = 0) {
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
			return ((Character arg1, Character arg2, Spell sp) => {
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
                damage = (int)(sp.GetNormalMultiplier() * damage / 100);

                if (isCrit) { // Apply Crit
                    damage += (int)(sp.GetCritMultiplier() * damage / 100);
                    sp.OnCrit(arg1, arg2, damage);

                }
				arg2.ApplyDamage ((int)(damage + damage * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100), isCrit);
			});
		} else {
			return ((Character arg1, Character arg2, Spell sp) => {
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
                heal = (int)(sp.GetNormalMultiplier() * heal / 100);
                if (isCrit) { // Apply Crit
                    heal += (int)(sp.GetCritMultiplier() * heal / 100);
                    sp.OnCrit(arg1, arg2, heal);
                }
				arg2.ApplyHeal ((int)(heal + heal * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100), isCrit);
			});
		}
	}


    //EffectsOnTime


    private Action<Character, Character, EffectOnTime> AddLifePercentOverTime(int lifePercent, bool mustNotBeFighting = false)
    {
        return ((Character arg1, Character arg2, EffectOnTime effect) => { if (!mustNotBeFighting || !arg1.IsInCombat()) { arg1.ApplyHeal((int)(arg1.GetMaxLife() * lifePercent / 100 * effect.GetStacks())); } });
    }

    private Action<Character, Character, EffectOnTime> RemoveLifePercentOverTime(int lifePercent)
    {
        return ((Character arg1, Character arg2, EffectOnTime effect) => { arg1.ApplyDamage((int)(arg1.GetMaxLife() * lifePercent / 100 * effect.GetStacks())); });
    }

    private Action<Character, Character, EffectOnTime> AddManaPercentOverTime(int manaPercent, bool mustNotBeFighting = false)
    {
        return ((Character arg1, Character arg2, EffectOnTime effect) => { if (arg1.GetResourceType() is Mana && (!mustNotBeFighting || !arg1.IsInCombat())) { arg1.AddResource((int)(arg1.GetMaxResource() * manaPercent / 100 * effect.GetStacks())); } });
    }
    private Action<Character, Character, float, int> RemoveManaPercentOverTime(int manaPercent)
    {
        return ((Character arg1, Character arg2, float timedivider, int stacks) => { if (arg1.GetResourceType() is Mana) { arg1.RemoveResource((int)(arg1.GetMaxResource() * manaPercent / 100 * stacks)); } });
    }



    private Action<Character, Character, EffectOnTime> newHealOnTime(Dictionary<Stat, float> statWeight, int totalHeal){
		return newActionOntime (statWeight, totalHeal, true);
	}

	private Action<Character, Character, EffectOnTime> newDamageOnTime(Dictionary<Stat, float> statWeight, int totalDamage){
		return newActionOntime (statWeight, totalDamage, false);
	}

	private Action<Character, Character, EffectOnTime> newActionOntime (Dictionary<Stat, float> statWeight, int baseNumber, bool isHeal) {
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
			return ((Character arg1, Character arg2, EffectOnTime effect) => {
				int damage = baseNumber;
                damage += (int)(arg1.GetStats().Intelligence * intelligenceMultiplier);
                damage += (int)(arg1.GetStats().Force * forceMultiplier);
                damage += (int)(arg1.GetStats().Agility * agilityMultiplier);
                damage += (int)(arg1.GetStats().Stamina * staminaMultiplier);
                damage += (int)(arg1.GetStats().Spirit * spiritMultiplier);

                Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				damage = damage + (damage * casterStats.Power / 100); //Applying power 
                damage = (int)(effect.GetNormalMultiplier() * damage / 100);

                if (isCrit)
                { // Apply Crit
                    damage += (int)(effect.GetCritMultiplier() * damage / 100);
                    effect.OnCrit(arg1, arg2, damage);
                }
                arg2.ApplyDamage ((int)((damage + damage * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100)* effect.GetStacks()*effect.GetTimeDivider()), isCrit);
			});
		} else {
			return ((Character arg1, Character arg2, EffectOnTime effect) => {
				int heal = baseNumber;
                heal += (int)(arg1.GetStats().Intelligence * intelligenceMultiplier);
                heal += (int)(arg1.GetStats().Force * forceMultiplier);
                heal += (int)(arg1.GetStats().Agility * agilityMultiplier);
                heal += (int)(arg1.GetStats().Stamina * staminaMultiplier);
                heal += (int)(arg1.GetStats().Spirit * spiritMultiplier);

                Stats casterStats = arg1.GetStats ();
				bool isCrit = casterStats.Critical > UnityEngine.Random.Range (1, 101);

				heal = heal + (heal * casterStats.Power / 100); //Applying power 
                heal = (int)(effect.GetNormalMultiplier() * heal / 100);
                if (isCrit)
                { // Apply Crit
                    heal += (int)(effect.GetCritMultiplier() * heal / 100);
                    effect.OnCrit(arg1, arg2, heal);
                }
                arg2.ApplyHeal ((int)((heal + heal * UnityEngine.Random.Range(-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100) * effect.GetStacks() * effect.GetTimeDivider()), isCrit);
			});
		}
	}

    // Use this for initialization
    void Start () {
        InitializeFindUtils();
        StartCoroutine(LateStart(0.2f));
    }	
    
    IEnumerator LateStart(float time)
    {
        yield return new WaitForSeconds(time);
        Debug.Log("LateStartCalled");
        DialogSigns.UpdateSigns();
    }

    private void InitializeFindUtils()
    {
        FindUtils.GetDps();
        FindUtils.GetInterface();
        FindUtils.GetDialogPanel();
        FindUtils.GetVendorBox();
        FindUtils.GetVendorPanel();
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
        FindUtils.GetTalentSheet();
        FindUtils.GetTalentSheet().SetActive(false);
        FindUtils.GetVendorBox().SetActive(false);  
        FindUtils.GetDialogBox().SetActive(false);
        FindUtils.GetCharacterSheet().SetActive(false);
        FindUtils.GetInventory().SetActive(false);
        FindUtils.GetSpellBook().SetActive(false);
        FindUtils.GetLoot().SetActive(false);
        FindUtils.GetQuestLog().SetActive(false);
        SoundManager.StopAll();
        SoundManager.SetVolume(1);
    }

    private void CreateDialogActions()
    {
        DialogActions.Add("vendor", () =>
        {

            FindUtils.GetDialogBox().SetActive(false);
            FindUtils.GetVendorBox().SetActive(true);
            FindUtils.GetVendorPanel().Initialize(FindUtils.GetDialogBoxComponent().GetDialogOwner());
        });

        DialogActions.Add("AddMageSpells", () =>
        {
            DialogStatus.SetStatus("TalentTreeSelected", true);
            DialogStatus.SetStatus("MageTalentSelected", true);
            Player player = FindUtils.GetPlayer();
            player.SetResourceType(new Mana());
            player.SetCurrentResource(0);
            player.AddSpell(Spells.Get("Fireball"));
           
        });

        DialogActions.Add("AddWarriorSpells", () =>
        {
            DialogStatus.SetStatus("TalentTreeSelected", true);
            DialogStatus.SetStatus("WarriorTalentSelected", true);
            Player player = FindUtils.GetPlayer();
            player.SetResourceType(new Rage());
            player.SetCurrentResource(0);
            player.AddSpell(Spells.Get("Slam"));
        });

        DialogActions.Add("AddRogueSpells", () =>
        {
            DialogStatus.SetStatus("TalentTreeSelected", true);
            DialogStatus.SetStatus("RogueTalentSelected", true);
            Player player = FindUtils.GetPlayer();
            player.SetResourceType(new Energy());
            player.SetCurrentResource(0); 
            player.AddSpell(Spells.Get("Sprint"));
            player.AddSpell(Spells.Get("Hemorrhage"));
        });

        DialogActions.Add("AddMageSpec", () =>
        {
            if (FindUtils.GetTalentSheetGrid().GetSpec1() == null)
            {
                FindUtils.GetTalentSheetGrid().SetSpec1(Specialisations.Get("Fire"));
                FindUtils.GetTalentSheetGrid().SetSpec2(Specialisations.Get("Frost"));
                FindUtils.GetTalentSheetGrid().SetSpec3(Specialisations.Get("Arcane"));
            }
        });

        DialogActions.Add("AddWarriorSpec", () =>
        {
            if (FindUtils.GetTalentSheetGrid().GetSpec1() == null)
            {
                FindUtils.GetTalentSheetGrid().SetSpec1(Specialisations.Get("Fire"));
                FindUtils.GetTalentSheetGrid().SetSpec2(Specialisations.Get("Frost"));
                FindUtils.GetTalentSheetGrid().SetSpec3(Specialisations.Get("Arcane"));
            }
        });

        DialogActions.Add("AddRogueSpec", () =>
        {
            if (FindUtils.GetTalentSheetGrid().GetSpec1() == null)
            {
                FindUtils.GetTalentSheetGrid().SetSpec1(Specialisations.Get("Fire"));
                FindUtils.GetTalentSheetGrid().SetSpec2(Specialisations.Get("Frost"));
                FindUtils.GetTalentSheetGrid().SetSpec3(Specialisations.Get("Arcane"));
            }
        });

        DialogActions.Add("ActivateTeleporterInArea", () =>
        {
            List<LevelChanger> levelChangersInZone = Resources.FindObjectsOfTypeAll<LevelChanger>().ToList();

            foreach(LevelChanger lvlChanger in levelChangersInZone)
            {
                lvlChanger.gameObject.SetActive(true);
            }
        });

        DialogActions.Add("WakeUpPlayer", () =>
        {
            Player player = FindUtils.GetPlayer();
            player.RemoveEffectOnTime(EffectsOnTime.Get("Asleep"));
            player.gameObject.GetComponent<Animator>().Play("Stand");

            DialogStatus.SetStatus("PlayerWokeUp", true);

            List<LevelChanger> levelChangersInZone = Resources.FindObjectsOfTypeAll<LevelChanger>().ToList();

            foreach (LevelChanger lvlChanger in levelChangersInZone)
            {
                lvlChanger.gameObject.SetActive(true);
            }
        });




        DialogActions.Add("GiveGorkrsRing", () =>
        {
            FindUtils.GetInventoryGrid().AddItem(Items.GetQuestEquipmentFromDB("Gorkr's lucky ring"));
        });
    }
}
