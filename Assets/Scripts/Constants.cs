using System.Collections;
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
    public static readonly int MaxHaste = 90;
    public static readonly int MaxLevel = 60;

    //Items
    public static readonly float MainStatMultiplier = 1.3f;
    public static readonly float OffStatMultiplier = 0.2f;
    public static readonly float CommonStatMultiplier = 0f;
    public static readonly float UncommonStatMultiplier = 0.7f;
    public static readonly float RareStatMultiplier = 1f;
    public static readonly float EpicStatMultiplier = 1.3f;

    //Loot
    public static readonly int DeleteLootAfterSeconds = 120;

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

public enum ItemType
{
    PlateHead,
    PlateTorso,
    PlateLegs,
    PlateBelt,
    PlateHands,
    PlateFeet,
    LeatherHead,
    LeatherTorso,
    LeatherLegs,
    LeatherBelt,
    LeatherHands,
    LeatherFeet,
    ClothHead,
    ClothTorso,
    ClothLegs,
    ClothBelt,
    ClothHands,
    ClothFeet,
    Neck,
    Ring,
    Axe,
    TwoHandedAxe,
    Sword,
    TwoHandedSword,
    Dagger,
    Mace,
    TwoHandedMace,
    Staff,
    OffHand,
    Shield
}

public enum ItemSlot
{
    Head,
    Neck,
    Torso,
    Legs,
    Belt,
    Hands,
    Ring,
    Feet,
    TwoHanded,
    Weapon1,
    Weapon2
}