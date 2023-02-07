using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {
	private int force;
	private int agility;
	private int inteligence;
	private int stamina;
	private int spirit;
	private int critical;
	private int haste;
	private int power;

	public Stats (int force, int agility, int inteligence, int stamina, int spirit, int critical, int haste, int power)
	{
		this.force = force;
		this.agility = agility;
		this.inteligence = inteligence;
		this.stamina = stamina;
		this.spirit = spirit;
		this.critical = critical;
		this.haste = haste;
		this.power = power;
	}
	

	public int Force {
		get {
			return this.force;
		}
	}

	public int Agility {
		get {
			return this.agility;
		}
	}

	public int Inteligence {
		get {
			return this.inteligence;
		}
	}

	public int Stamina {
		get {
			return this.stamina;
		}
	}

	public int Spirit {
		get {
			return this.spirit;
		}
	}

	public int Critical {
		get {
			return this.critical;
		}
	}

	public int Haste {
		get {
			return this.haste;
		}
	}

	public int Power {
		get {
			return this.power;
		}
	}


}
