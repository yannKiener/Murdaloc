﻿using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class EquipmentGenerator  {
    private static List<EquipmentType> weaponTypes = new List<EquipmentType>() { EquipmentType.Axe, EquipmentType.TwoHandedAxe, EquipmentType.Sword, EquipmentType.TwoHandedSword, EquipmentType.Mace, EquipmentType.TwoHandedMace, EquipmentType.Dagger, EquipmentType.Staff };

    private static List<EquipmentType> twoHandedWeaponTypes = new List<EquipmentType>() {EquipmentType.TwoHandedAxe,EquipmentType.TwoHandedSword, EquipmentType.TwoHandedMace, EquipmentType.Staff};

    private static List<EquipmentType> heldInOffhand = new List<EquipmentType>() {EquipmentType.Shield, EquipmentType.OffHand};

    private static Dictionary<Stat, JSONArray> statNames = null;
    private static Dictionary<EquipmentType, JSONArray> typeNames = null;
    private static Dictionary<EquipmentType, Sprite[]> typeSprites = null;


    public static Equipment GenerateEquipment(int maxLevel)
    {
        float maxMainStats = Constants.MainStatMultiplier * maxLevel;
        float maxOffStats = Constants.OffStatMultiplier * maxLevel;
        EquipmentType itemType = GetRandomEquipmentType();
        EquipmentQuality quality = GetRandomQualityForLevel(maxLevel);
        float damagePerSecondOnWeapon = GetRandomAutoAttackDPSForLevel(maxLevel);
        Stats stats = GenerateStatsForType(itemType, maxMainStats, maxOffStats, damagePerSecondOnWeapon, quality);
        string name = GetRandomNameForTypeAndStats(itemType, stats);
        Sprite sprite = GetRandomSpriteForType(itemType);

        Equipment result = new Equipment(name,"",maxLevel,stats,itemType, quality);
        result.SetImage(sprite);
        return result;
    }

    private static float GetRandomAutoAttackDPSForLevel(int level)
    {
        float randomMultiplier = UnityEngine.Random.Range(1-Constants.WeapDamageRandomiser, 1 + Constants.WeapDamageRandomiser);
        return (Constants.BaseAutoAttackDPS + Constants.AutoAttackDPSPerLevel * level * randomMultiplier);
    }

    private static EquipmentQuality GetRandomQualityForLevel(int level)
    {
        int levelMaxPercent = (Constants.MaxLevel - level) /Constants.DropReducer +1;

        int random = UnityEngine.Random.Range(0,101) * levelMaxPercent;

        if (random < Constants.EpicDropChancePercent)
        {
            return EquipmentQuality.Epic;
        }
        if (random < Constants.RareDropChancePercent)
        {
            return EquipmentQuality.Rare;
        }
        if (random < Constants.UncommonDropChancePercent)
        {
            return EquipmentQuality.Uncommon;
        }

        return EquipmentQuality.Common;

    }


    private static Sprite[] GetSpritesForType(EquipmentType equipType)
    {
        if(typeSprites == null)
        {
            typeSprites = new Dictionary<EquipmentType, Sprite[]>();
            foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
            {
                typeSprites.Add(type, DatabaseUtils.LoadAllSpritesForType(type));
            }
        }
        if (!typeSprites.ContainsKey(equipType))
        {
            Debug.Log("NO SPRITES FOR ITEM TYPE : " + equipType);
            return null;
        } else
        {
            return typeSprites[equipType];
        }
    }


    private static Sprite GetRandomSpriteForType(EquipmentType type)
    {
        Sprite[] sprites = GetSpritesForType(type);
        if (sprites != null && sprites.Length > 0){
            return sprites[UnityEngine.Random.Range(0, sprites.Length-1)];
        } else {
           return InterfaceUtils.LoadSpriteForItem("Default");
        }
    }


    private static Dictionary<EquipmentType, JSONArray> GetTypeNames()
    {
        if(typeNames == null)
        {
            typeNames = new Dictionary<EquipmentType, JSONArray>();
            JSONObject jsonObjectCategoryNames = DatabaseUtils.GetJsonCategoryNames();
            foreach (EquipmentType type in Enum.GetValues(typeof(EquipmentType)))
            {
                typeNames.Add(type, jsonObjectCategoryNames.AsObject[type.ToString()].AsArray);
            }
        }
        return typeNames;
    }

    private static string GetRandomTypeName(EquipmentType type)
    {
        JSONArray names = GetTypeNames()[type];
        return names[UnityEngine.Random.Range(0, names.Count)];
    }


    private static Dictionary<Stat, JSONArray> GetStatNames()
    {
        if (statNames == null)
        {
            statNames = new Dictionary<Stat, JSONArray>();
            JSONObject jsonObjectStatNames = DatabaseUtils.GetJsonStatNames();
            foreach (Stat stat in Enum.GetValues(typeof(Stat)))
            {
                statNames.Add(stat, jsonObjectStatNames.AsObject[stat.ToString()].AsArray);
            }
        }
        return statNames;
    }

    private static string GetRandomStatName(Stat stat)
    {
        JSONArray names = GetStatNames()[stat];
        string result = names[UnityEngine.Random.Range(0, names.Count)];
        if(result == null)
        {
            Debug.Log("No names found for stat : " + stat);
            return "";
        } else
        {
            return result;
        }
    }


    private static string GetRandomNameForTypeAndStats (EquipmentType type, Stats stats)
    {
        Stat mainStat = stats.GetMaxMainStat();
        Stat offStat = stats.GetMaxOffStat();

        string mainStatName = "";
        if(stats.GetStat(mainStat) > 1)
        {
            mainStatName = GetRandomStatName(mainStat);
        }

        string offStatName = "";
        if (stats.GetStat(offStat) > 1)
        {
            offStatName = GetRandomStatName(offStat);
        }

        string name = GetRandomTypeName(type);

        string result = "";
        if(offStatName.Length > 1)
        {
            result += offStatName + " ";
        }


        if (mainStatName.Length > 1)
        {
            bool mainStatNameFirst = UnityEngine.Random.Range(0,3) >= 1;
            if (mainStatNameFirst)
            {
                result += mainStatName + "'s ";
                result += name;
            } else
            {
                result += name;
                result += " of the " + mainStatName;
            }
        } else
        {
            result += name;

        }
        return result;
    }
        

    private static EquipmentType GetRandomEquipmentType()
    {
        Array values = Enum.GetValues(typeof(EquipmentType));
        
        return (EquipmentType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    private static Stats AddStatFromEnumRandomly(List<Stat> statList, float number)
    {
        Stats stats = new Stats(0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        Stat stat = statList[UnityEngine.Random.Range(0, statList.Count)];
        stats.AddStat(stat, number);
        return stats;
    }

    private static Stats GenerateStatsForType(EquipmentType type, float maxMainStats, float maxOffStats, float attackDamage, EquipmentQuality quality)
    {
        if(quality == EquipmentQuality.Common)
        {
            maxMainStats = maxMainStats * Constants.CommonStatMultiplier;
            maxOffStats = maxOffStats * Constants.CommonStatMultiplier;
            attackDamage = attackDamage *( 1 +  Constants.CommonStatMultiplier / Constants.RarityWeapDpsReducer);

        } else if (quality == EquipmentQuality.Uncommon)
        {
            maxMainStats = maxMainStats * Constants.UncommonStatMultiplier;
            maxOffStats = maxOffStats * Constants.UncommonStatMultiplier;
            attackDamage = attackDamage * (1 + Constants.UncommonStatMultiplier / Constants.RarityWeapDpsReducer);
        }
        else if (quality == EquipmentQuality.Rare)
        {
            maxMainStats = maxMainStats * Constants.RareStatMultiplier;
            maxOffStats = maxOffStats * Constants.RareStatMultiplier;
            attackDamage = attackDamage * (1 + Constants.RareStatMultiplier / Constants.RarityWeapDpsReducer);
        }
        else if (quality == EquipmentQuality.Epic)
        {
            maxMainStats = maxMainStats * Constants.EpicStatMultiplier;
            maxOffStats = maxOffStats * Constants.EpicStatMultiplier;
            attackDamage = attackDamage * (1 + Constants.EpicStatMultiplier / Constants.RarityWeapDpsReducer);
        }

        if (IsOffHand(type))
        {   
            maxMainStats = maxMainStats * Constants.OffHandStatMultiplier;
            maxOffStats = maxOffStats * Constants.OffHandStatMultiplier;
        }
        if (IsTwoHanded(type))
        {
            maxMainStats = maxMainStats * Constants.TwoHandedStatMultiplier;
            maxOffStats = maxOffStats * Constants.TwoHandedStatMultiplier;
        }

        float stat1Multiplier = UnityEngine.Random.Range(30,51) ;
        float stat2Multiplier = UnityEngine.Random.Range(30,51) ;
        float stat3Multiplier = UnityEngine.Random.Range(0, 101-(stat1Multiplier+stat2Multiplier)) ;
        float offStat1Multiplier = UnityEngine.Random.Range(30, 71) ;
        float offStat2Multiplier = UnityEngine.Random.Range(20, 31) ;
        float offStat3Multiplier = UnityEngine.Random.Range(0, 101-(offStat1Multiplier+offStat2Multiplier)) ;
        List<Stat> mainStatList = GetMainStatList(type);
        List<Stat> offStatList = GetOffStatList(type);

        Stats result = AddStatFromEnumRandomly(mainStatList, maxMainStats * stat1Multiplier / 100);
        result.Add(AddStatFromEnumRandomly(mainStatList, maxMainStats * stat2Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(mainStatList, maxMainStats * stat3Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat1Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat2Multiplier / 100));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat3Multiplier / 100));

        if (IsWeapon(type))
        {
            float attackSpeed = (float)Math.Round(GetAttackSpeedForType(type), 1);
            float mediumAttackDamage = attackDamage * attackSpeed;
            if (IsTwoHanded(type))
            {
                mediumAttackDamage = mediumAttackDamage * Constants.TwoHandedAutoAttackMultiplier;
                if(type == EquipmentType.Staff)
                {
                    mediumAttackDamage = mediumAttackDamage / Constants.StaffAutoAttackDivider;
                }
            }
            result.AddStat(Stat.autoAttackDamage, mediumAttackDamage);
            result.AddStat(Stat.autoAttackTime, attackSpeed);
        }

        return result;
    }

    private static float GetAttackSpeedForType(EquipmentType t)
    {
        switch (t)
        {
            case EquipmentType.Sword:
                return UnityEngine.Random.Range(1.5f,2.5f);
            case EquipmentType.TwoHandedSword:
                return UnityEngine.Random.Range(2.5f, 4f);
            case EquipmentType.Axe:
                return UnityEngine.Random.Range(1.5f, 2.5f);
            case EquipmentType.TwoHandedAxe:
                return UnityEngine.Random.Range(2.5f, 4f);
            case EquipmentType.Mace:
                return UnityEngine.Random.Range(1.5f, 2.5f);
            case EquipmentType.TwoHandedMace:
                return UnityEngine.Random.Range(2.5f, 4f);
            case EquipmentType.Dagger:
                return UnityEngine.Random.Range(0.8f, 2f);
            case EquipmentType.Staff:
                return UnityEngine.Random.Range(2.5f, 4f);
        }
        return 5;
    }

    private static bool IsTwoHanded(EquipmentType itemType)
    {
        return IsEqTypeInList(twoHandedWeaponTypes, itemType);
    }
    
    private static bool IsOffHand(EquipmentType itemType)
    {
        return IsEqTypeInList(heldInOffhand, itemType);
    }

    private static bool IsWeapon(EquipmentType itemType)
    {
        return IsEqTypeInList(weaponTypes,itemType);
    }

    private static bool IsEqTypeInList(List<EquipmentType> listOfTypes, EquipmentType itemType)
    {
        return listOfTypes.Contains(itemType);
    }

    private static List<Stat> GetMainStatList(EquipmentType type)
    {
        return EquipmentCategories.GetCategory(type).GetPossibleMainStats();
    }

    private static List<Stat> GetOffStatList(EquipmentType type)
    {
        return EquipmentCategories.GetCategory(type).GetPossibleOffStats();
    }
}