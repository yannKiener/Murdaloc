using SimpleJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemGenerator  {

    public static Item GenerateItem(int maxLevel)
    {
        //Quality multiplier : 
        //Green = 0.7
        //Blue = 1
        //Epic = 1.3

        //Add quality at multiplier
        //Make a random type
        float maxMainStats = Constants.MainStatMultiplier * maxLevel;
        float maxOffStats = Constants.OffStatMultiplier * maxLevel;
        ItemType itemType = GetRandomItemType();


        Stats stats = GenerateStatsForType(itemType, maxMainStats, maxOffStats);
        string name = GetRandomNameForType(itemType);

        Item result = new Item(name,"",maxLevel,stats,itemType);//new Item();
        return result;
    }


    private static string GetRandomNameForType (ItemType type)
    {
        JSONArray names = DatabaseUtils.GetJsonCategoryName(type);
        return names[UnityEngine.Random.Range(0, names.Count)];
    }

    private static ItemType GetRandomItemType()
    {
        Array values = Enum.GetValues(typeof(ItemType));
        System.Random random = new System.Random();
        return (ItemType)values.GetValue(random.Next(values.Length));
    }

    private static Stats AddStatFromEnumRandomly(List<Stat> statList, float number)
    {
        Stats stats = new Stats(0, 0, 0, 0, 0, 0, 0, 0);
        Stat stat = statList[UnityEngine.Random.Range(0, statList.Count)];

        stats.AddStat(stat, number);
        return stats;
    }

    private static Stats GenerateStatsForType(ItemType type, float maxMainStats, float maxOffStats)
    {
        int stat1Multiplier = UnityEngine.Random.Range(30,71);
        int stat2Multiplier = UnityEngine.Random.Range(20, 31);
        int stat3Multiplier = UnityEngine.Random.Range(0, 100-(stat1Multiplier+stat2Multiplier));
        int offStat1Multiplier = UnityEngine.Random.Range(30, 71);
        int offStat2Multiplier = UnityEngine.Random.Range(20, 31);
        int offStat3Multiplier = UnityEngine.Random.Range(0, 100-(offStat1Multiplier+offStat2Multiplier));
        List<Stat> mainStatList = GetMainStatList(type);
        List<Stat> offStatList = GetOffStatList(type);

        Stats result = AddStatFromEnumRandomly(mainStatList, maxMainStats*stat1Multiplier);
        result.Add(AddStatFromEnumRandomly(mainStatList, maxMainStats * stat2Multiplier));
        result.Add(AddStatFromEnumRandomly(mainStatList, maxMainStats * stat3Multiplier));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat1Multiplier));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat2Multiplier));
        result.Add(AddStatFromEnumRandomly(offStatList, maxOffStats * offStat3Multiplier));

        return result;
    }

    private static List<Stat> GetMainStatList(ItemType type)
    {
        return ItemCategories.GetCategory(type).GetPossibleMainStats();
    }

    private static List<Stat> GetOffStatList(ItemType type)
    {
        return ItemCategories.GetCategory(type).GetPossibleMainStats();
    }
}