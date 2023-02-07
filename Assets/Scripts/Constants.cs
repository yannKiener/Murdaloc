using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants {
	//Global Gameplay 
	public static readonly float MaxSpeed = 8f;
    public static readonly float JumpForce = 5f; 
	public static readonly float BaseExp = 30f; 

	//Strings
	public static readonly string Mana = "Mana";


	//Interface
	public static readonly float characterBarswPercent = 22;
	public static readonly float characterBarshPercent = 4;
	public static readonly bool drawCastBar = true;
	public static readonly float castBarwPercent = 22;
	public static readonly float castBarhPercent = 4;
    public static readonly float expBarHeightPercent = 1;
    public static readonly Color MouseOverColor = new Color(0.82f, 0.82f, 0.82f, 0.9f);
    public static readonly float keepDpsDamageSeconds = 5;
    public static readonly float mainMenuSizeXPercent = 20;
    public static readonly float mainMenuSizeYPercent = 30;
    public static readonly float optionMenuSizeXPercent = 40;
    public static readonly float optionMenuSizeYPercent = 30;
    public static readonly float modalDialogSizeXPercent = 20;
    public static readonly float modalDialogSizeYPercent = 10;


    //stats
    public static readonly float GlobalCooldown = 1f;
	public static readonly int BaseResource = 100;
	public static readonly int BaseLife = 100;
	public static readonly int StaminaLifeMultiplier = 10;
	public static readonly int IntelligenceManaMultiplier = 10;
	public static readonly int hasteDivider = 200;
	public static readonly int BaseAutoAttackDPS = 2;
    public static readonly int AutoAttackDPSPerLevel = 2;
    public static readonly float BaseAutoAttackSpeed = 2;
    public static readonly float MaxAutoAttackDistance = 2;
    public static readonly float MobAutoAttackMultiplier = 1.5f;
    public static readonly float PlayerAutoAttackDivider = 3;
    public static readonly float RatioSpiritManaPerSecond = 1;
	public static readonly int BaseManaPerSecond = 5;
	public static readonly int RegenManaEvery = 1;
    public static readonly float RegenManaAfter = 5f;
    public static readonly int RegenEnergyEvery = 2;
    public static readonly int MaxHaste = 90;
    public static readonly int MaxLevel = 60;
    public static readonly int RegenLifeEvery = 3;
    public static readonly int RegenLifePercent = 5;
    //EquilibrageStats
    public static readonly int ForceByLevel = 3;
    public static readonly int AgilityByLevel = 3;
    public static readonly int IntelligenceByLevel = 3;
    public static readonly int StaminaByLevel = 5;
    public static readonly int SpiritByLevel = 4;
    public static readonly float RandomDamageRange = 30f;
    public static readonly float expPerQuestPercent = 20;

    //Equipments
    public static readonly float MainStatMultiplier = 1.3f;
    public static readonly float OffStatMultiplier = 0.2f;
    public static readonly float CommonStatMultiplier = 0f;
    public static readonly float UncommonStatMultiplier = 0.7f;
    public static readonly float RareStatMultiplier = 1f;
    public static readonly float EpicStatMultiplier = 1.3f;
    public static readonly float TwoHandedStatMultiplier = 1.6f;

    //Loot
    public static readonly int DeleteLootAfterSeconds = 120;
    public static readonly float BuyPriceMultiplier = 5;

    //Enemies 
    public static readonly float AggroDistance = 2;
	public static readonly float AggroOtherDistance = 2;
    public static readonly float LootMaxDistance = 3.2f;
    public static readonly float TimeBeforeSpawnOrDespawn = 5f;
    public static readonly float TimeToFadeInOrOutSpawnOrDespawn = 3f;
    public static readonly float RespawnTimer = 500;


    //Save
    public static readonly string DialogStatusFile = "/DialogStatus.dat";




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

[System.Serializable]
public enum EquipmentType
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

public enum EquipmentSlot
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