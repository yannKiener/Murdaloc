﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
	//Global Gameplay 
	public static readonly float MaxSpeed = 8f;
	public static readonly float JumpForce = 5f;

	//Strings
	public static readonly string Mana = "Mana";


	//Interface
	public static readonly float characterBarswPercent = 22;
	public static readonly float characterBarshPercent = 4;
	public static readonly bool drawCastBar = true;
	public static readonly float castBarwPercent = 22;
	public static readonly float castBarhPercent = 4;

	//stats
	public static readonly float GlobalCooldown = 1f;
	public static readonly int BaseResource = 100;
	public static readonly int BaseLife = 100;
	public static readonly int StaminaLifeMultiplier = 10;
	public static readonly int IntelligenceManaMultiplier = 10;
	public static readonly int hasteDivider = 200;
	public static readonly int BaseAutoAttackDamage = 10;
	public static readonly float BaseAutoAttackTime = 2;
	public static readonly float MaxAutoAttackDistance = 2;
	public static readonly float RatioSpiritManaPerSecond = 1;
	public static readonly int BaseManaPerSecond = 5;
	public static readonly int RegenManaEvery = 1;
	public static readonly int RegenEnergyEvery = 2;
    public static readonly int maxHaste = 90;



	//Enemies 
	public static readonly float AggroDistance = 2;
	public static readonly float AggroOtherDistance = 2;
	
	
	//Save
	public static readonly string QuestStatusFile = "/QuestStatus.dat";




}

public enum Stat {
	force,
	agility,
	intelligence,
	stamina,
	spirit,
	critical,
	haste,
	power,
	autoAttackDamage,
	autoAttackTime,
	maxSpeed,
	maxResource,
	maxLife
}


