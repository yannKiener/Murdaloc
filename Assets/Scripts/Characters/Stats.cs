using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
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
	private int autoAttackDamage = 0;
	private float autoAttackTime = 0;
	private float maxSpeed = Constants.MaxSpeed;


	public Stats (int force, int agility, int intelligence, int stamina, int spirit, int critical, int haste, int power, bool isMana = true)
	{
        AutoAttackDamage = Constants.BaseAutoAttackDamage;
        autoAttackTime = Constants.BaseAutoAttackSpeed;
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

    public Stats(int force, int agility, int intelligence, int stamina, int spirit, int critical, int haste, int power, int autoAttackDamage, float autoAttackTime)
    {
        this.AutoAttackDamage = autoAttackDamage;
        this.AutoAttackTime = autoAttackTime;
        this.force = force;
        this.agility = agility;
        this.intelligence = intelligence;
        this.stamina = stamina;
        this.spirit = spirit;
        this.critical = critical;
        this.haste = haste;
        this.power = power;
    }

	public void Add(Stats stats, bool isWeapon2 = false){
		this.Force += stats.Force;
		this.Agility += stats.Agility;
		this.Intelligence += stats.Intelligence;
		this.Stamina += stats.Stamina;
		this.Spirit += stats.Spirit;
		this.Critical += stats.Critical;
		this.Haste += stats.Haste;
		this.Power += stats.Power;
        
        if(stats.autoAttackDamage != 0)
        {
            this.AutoAttackDamage = stats.AutoAttackDamage;
        }
        if (stats.autoAttackTime != 0)
        {
            this.AutoAttackTime = stats.AutoAttackTime;
        }
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
        if (AutoAttackDamage > 0)
        {
            result += "\nDamage : " + AutoAttackDamage;
        }
        if (AutoAttackTime > 0)
        {
            result += "\nSpeed : " + AutoAttackTime;
        }
        if (Force > 0)
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

    public void MultiplyBy(float multiplier)
    {
        force = (int)(force * multiplier);
        agility = (int)(agility * multiplier);
        intelligence = (int)(intelligence * multiplier);
        stamina = (int)(stamina * multiplier);
        spirit = (int)(spirit * multiplier);
        haste = (int)(haste * multiplier);
        power = (int)(power * multiplier);
        critical = (int)(critical * multiplier);
        autoAttackDamage = (int)(autoAttackDamage * multiplier);
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


    public void AddStat(Stat stat, float number)
    {
        switch (stat)
        {
            case Stat.agility:
                {
                    Agility += (int)number;
                    break;
                }
            case Stat.autoAttackDamage:
                {
                    AutoAttackDamage += (int)number;
                    break;

                }
            case Stat.autoAttackTime:
                {
                    AutoAttackTime += number;
                    break;
                }
            case Stat.critical:
                {
                    Critical += (int)number;
                    break;
                }
            case Stat.force:
                {
                    Force += (int)number;
                    break;
                }
            case Stat.haste:
                {
                    Haste += (int)number;
                    break;
                }
            case Stat.intelligence:
                {
                    Intelligence += (int)number;
                    break;

                }
            case Stat.maxLife:
                {
                    MaxLife += (int)number;
                    break;

                }
            case Stat.maxResource:
                {
                    MaxResource += (int)number;
                    break;

                }
            case Stat.maxSpeed:
                {
                    MaxSpeed += number;
                    break;

                }
            case Stat.power:
                {
                    Power += (int)number;
                    break;
                }
            case Stat.spirit:
                {
                    Spirit += (int)number;
                    break;
                }
            case Stat.stamina:
                {
                    Stamina += (int)number;
                    break;
                }
        }
    }


    public float GetStat(Stat stat)
    {

        switch (stat)
        {
            case Stat.agility:
                {
                    return Agility;
                }
            case Stat.autoAttackDamage:
                {
                    return AutoAttackDamage;

                }
            case Stat.autoAttackTime:
                {
                    return AutoAttackTime;
                }
            case Stat.critical:
                {
                    return Critical;
                }
            case Stat.force:
                {
                    return Force;
                }
            case Stat.haste:
                {
                    return Haste;
                }
            case Stat.intelligence:
                {
                    return Intelligence;

                }
            case Stat.maxLife:
                {
                    return MaxLife;

                }
            case Stat.maxResource:
                {
                    return MaxResource;

                }
            case Stat.maxSpeed:
                {
                    return MaxSpeed;

                }
            case Stat.power:
                {
                    return Power;
                }
            case Stat.spirit:
                {
                    return Spirit;
                }
            case Stat.stamina:
                {
                    return Stamina;
                }
        }
        return 0;
    }

    public Stat GetMaxMainStat()
    {
        Dictionary<Stat, int> statHolder = new Dictionary<Stat, int>();
        statHolder.Add(Stat.force, force);
        statHolder.Add(Stat.agility, agility);
        statHolder.Add(Stat.intelligence, intelligence);
        statHolder.Add(Stat.stamina, stamina);
        statHolder.Add(Stat.spirit, spirit);
        return statHolder.FirstOrDefault(x => x.Value == statHolder.Values.Max()).Key;

    }

    public Stat GetMaxOffStat()
    {
        Dictionary<Stat, int> statHolder = new Dictionary<Stat, int>();
        statHolder.Add(Stat.critical, critical);
        statHolder.Add(Stat.haste, haste);
        statHolder.Add(Stat.power, power);

        return statHolder.FirstOrDefault(x => x.Value == statHolder.Values.Max()).Key;
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
