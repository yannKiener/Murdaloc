using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {
	private bool isResourceMana = false;


	private int maxLife;
	private int maxResource;
	private int force;
	private int agility;
	private int intelligence;
	private int stamina;
	private int spirit;
	private int critical;
	private int haste;
	private int power;
	private int autoAttackDamage = Constants.BaseAutoAttackDamage;
	private float autoAttackTime = Constants.BaseAutoAttackTime;
	private float maxSpeed = Constants.MaxSpeed;


	public Stats (int force, int agility, int intelligence, int stamina, int spirit, int critical, int haste, int power, bool isMana)
	{
		isResourceMana = isMana;
		this.maxLife = Constants.BaseLife + stamina*Constants.StaminaLifeMultiplier;
		if (isResourceMana) {
			this.maxResource = Constants.BaseResource + intelligence*Constants.IntelligenceManaMultiplier;
		} else {
			this.maxResource = Constants.BaseResource;
		}
		this.force = force;
		this.agility = agility;
		this.intelligence = intelligence;
		this.stamina = stamina;
		this.spirit = spirit;
		this.critical = critical;
		this.haste = haste;
		this.power = power;
	}

	public void displayStats(){
		Debug.Log (force +" " +agility +" " +intelligence +" " +stamina +" " +spirit +" " +critical +" " +haste +" " +power);
	}

	public void Add(Stats stats){
		this.Force += stats.Force;
		this.Agility += stats.Agility;
		this.Intelligence += stats.Intelligence;
		this.Stamina += stats.Stamina;
		this.Spirit += stats.Spirit;
		this.Critical += stats.Critical;
		this.Haste += stats.Haste;
		this.Power += stats.Power;
		if (this.Critical > 90) {
			this.Critical = 90;
		}
		if (this.Haste > 90) {
			this.Haste = 90;
		}
		if (this.Power > 90) {
			this.Power = 90;
		}
	}

	public float AddPercent(Stat stat, float percent){
		float result;
		switch (stat) {
		case Stat.agility:
			{
				result = (float)((int)(agility * percent / 100));
				agility += (int)result;
				return result;
			}
		case Stat.autoAttackDamage:
			{
				result = (float)((int)(autoAttackDamage * percent / 100));
				autoAttackDamage += (int)result;
				return result;

			}
		case Stat.autoAttackTime:
			{
				result = (float)((int)(autoAttackTime * percent / 100));
				autoAttackTime += (int)result;
				return result;
			}
		case Stat.critical:
			{
				critical += (int)percent;
				return percent;
			}
		case Stat.force:
			{
				result = (float)((int)(force * percent / 100));
				force += (int)result;
				return result;
			}
		case Stat.haste:
			{
				haste += (int)percent;
				return percent;
			}
		case Stat.intelligence:
			{
				result = (float)((int)(intelligence * percent / 100));
				intelligence += (int)result;
				return result;

			}
		case Stat.maxLife:
			{
				result = (float)((int)(maxLife * percent / 100));
				maxLife += (int)result;
				return result;

			}
		case Stat.maxResource:
			{
				result = (float)((int)(maxResource * percent / 100));
				maxResource += (int)result;
				return result;

			}
		case Stat.maxSpeed:
			{
				result = (maxSpeed * percent / 100);
				MaxSpeed += result;
				return result;

			}
		case Stat.power:
			{
				power += (int)percent;
				return percent;
			}
		case Stat.spirit:
			{
				result = (float)((int)(spirit * percent / 100));
				spirit += (int)result;
				return result;
			}
		case Stat.stamina:
			{
				result = (float)((int)(stamina * percent / 100));
				stamina += (int)result;
				return result;
			}
		}
		return 0f;
	}


	public void SetAutoAttack(int damage, float time){
		this.autoAttackDamage = damage;
		this.autoAttackTime = time;
	}

	public float AutoAttackTime {
		get {
			return this.autoAttackTime;
		}
		set {
			autoAttackTime = value;
		}
	}

	public float MaxSpeed {
		get {
			return this.maxSpeed;
		}
		set {
			maxSpeed = value;
		}
	}

	public int AutoAttackDamage {
		get {
			return this.autoAttackDamage;
		}
		set {
			autoAttackDamage = value;
		}
	}

	
	public int MaxLife {
		get {
			return this.maxLife;
		}
		set {
			maxLife = value;
		}
	}

	public int MaxResource {
		get {
			return this.maxResource;
		}
		set {
			maxResource = value;
		}
	}

	public int Force {
		get {
			return this.force;
		}
		set {
			force = value;
		}
	}

	public int Agility {
		get {
			return this.agility;
		}
		set {
			agility = value;
		}
	}

	public int Intelligence {
		get {
			return this.intelligence;
		}
		set {
			intelligence = value;
			updateMaxMana ();
		}
	}
	private void updateMaxMana(){
		if (isResourceMana) {
			this.maxResource = Constants.BaseResource + intelligence*Constants.IntelligenceManaMultiplier;
		}
	}

	public int Stamina {
		get {
			return this.stamina;
		}
		set {
			stamina = value;
			updateMaxLife ();
		}
	}
	private void updateMaxLife(){
			this.maxLife =	Constants.BaseLife + stamina*Constants.StaminaLifeMultiplier;
	}



	public int Spirit {
		get {
			return this.spirit;
		}
		set {
			spirit = value;
		}
	}

	public int Critical {
		get {
			return this.critical;
		}
		set {
			critical = value;
		}
	}

	public int Haste {
		get {
			return this.haste;
		}
		set {
			haste = value;
		}
	}

	public int Power {
		get {
			return this.power;
		}
		set {
			power = value;
		}
	}

}
