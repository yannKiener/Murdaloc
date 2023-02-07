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
	private int manaPerSecond;
	private int autoAttackDamage = Constants.BaseAutoAttackDamage;
	private float autoAttackTime = Constants.BaseAutoAttackTime;
	private float maxSpeed = Constants.MaxSpeed;


	public Stats (int force, int agility, int intelligence, int stamina, int spirit, int critical, int haste, int power, bool isMana = true)
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
		this.manaPerSecond = (int)(Constants.BaseManaPerSecond + spirit * Constants.RatioSpiritManaPerSecond);
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
	}

    public void Remove(Stats stats)
    {
        this.Force -= stats.Force;
        this.Agility -= stats.Agility;
        this.Intelligence -= stats.Intelligence;
        this.Stamina -= stats.Stamina;
        this.Spirit -= stats.Spirit;
        this.Critical -= stats.Critical;
        this.Haste -= stats.Haste;
        this.Power -= stats.Power;
    }

    public int GetManaPerSec(){
		return manaPerSecond;
	}

    public string GetStatsDetail()
    {
        string result = "";
        if(Force > 0)
        {
            result += "\nForce : " + Force;
        }
        if (Agility > 0)
        {
            result += "\nAgility : " + Agility;
        }
        if (Intelligence > 0)
        {
            result += "\nIntelligence : " + Intelligence;
        }
        if (Stamina > 0)
        {
            result += "\nStamina : " + Stamina;
        }
        if (Spirit > 0)
        {
            result += "\nSpirit : " + Spirit;
        }
        if (Critical > 0)
        {
            result += "\nCritical : " + Critical;
        }
        if (Haste > 0)
        {
            result += "\nHaste : " + Haste;
        }
        if (Power > 0)
        {
            result += "\nPower : " + Power;
        }
        return result;
    }

	public float AddPercent(Stat stat, float percent){
		float result;
		switch (stat) {
		case Stat.agility:
			{
				result = (float)((int)(agility * percent / 100));
				Agility += (int)result;
				return result;
			}
		case Stat.autoAttackDamage:
			{
				result = (float)((int)(autoAttackDamage * percent / 100));
				AutoAttackDamage += (int)result;
				return result;

			}
		case Stat.autoAttackTime:
			{
				result = (float)((int)(autoAttackTime * percent / 100));
				AutoAttackTime += (int)result;
				return result;
			}
		case Stat.critical:
			{
				Critical += (int)percent;
				return percent;
			}
		case Stat.force:
			{
				result = (float)((int)(force * percent / 100));
				Force += (int)result;
				return result;
			}
		case Stat.haste:
			{
				Haste += (int)percent;
				return percent;
			}
		case Stat.intelligence:
			{
				result = (float)((int)(intelligence * percent / 100));
				Intelligence += (int)result;
				return result;

			}
		case Stat.maxLife:
			{
				result = (float)((int)(maxLife * percent / 100));
				MaxLife += (int)result;
				return result;

			}
		case Stat.maxResource:
			{
				result = (float)((int)(maxResource * percent / 100));
				MaxResource += (int)result;
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
				Power += (int)percent;
				return percent;
			}
		case Stat.spirit:
			{
				result = (float)((int)(spirit * percent / 100));
				Spirit += (int)result;
				return result;
			}
		case Stat.stamina:
			{
				result = (float)((int)(stamina * percent / 100));
				Stamina += (int)result;
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
			updateRegenPerSec ();
		}
	}
	private void updateRegenPerSec(){
		manaPerSecond = (int)(Constants.BaseManaPerSecond + spirit * Constants.RatioSpiritManaPerSecond);
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
