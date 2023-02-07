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
        Stats stats = GenerateStatsForType("head", maxMainStats, maxOffStats);

        Item result = null;//new Item();
        return result;
    }

    private static Stats AddStatFromEnumRandomly(List<Stat> statList, float number)
    {
        Stats stats = new Stats(0, 0, 0, 0, 0, 0, 0, 0);
        Stat stat = statList[Random.Range(0, statList.Count) +1];

        stats.AddStat(stat, number);
        return stats;
    }

    private static Stats GenerateStatsForType(string type, float maxMainStats, float maxOffStats)
    {
        int stat1Multiplier = Random.Range(30,71);
        int stat2Multiplier = Random.Range(20, 31);
        int stat3Multiplier = Random.Range(0, 100-(stat1Multiplier+stat2Multiplier));
        int offStat1Multiplier = Random.Range(30, 71);
        int offStat2Multiplier = Random.Range(20, 31);
        int offStat3Multiplier = Random.Range(0, 100-(offStat1Multiplier+offStat2Multiplier));
        List<Stat> CHANGEME = GetStatList(type);

        Stats result = AddStatFromEnumRandomly(CHANGEME, maxMainStats*stat1Multiplier);
        result.Add(AddStatFromEnumRandomly(CHANGEME, maxMainStats * stat2Multiplier));
        result.Add(AddStatFromEnumRandomly(CHANGEME, maxMainStats * stat3Multiplier));
        result.Add(AddStatFromEnumRandomly(CHANGEME, maxOffStats * offStat1Multiplier));
        result.Add(AddStatFromEnumRandomly(CHANGEME, maxOffStats * offStat2Multiplier));
        result.Add(AddStatFromEnumRandomly(CHANGEME, maxOffStats * offStat3Multiplier));

        return result;
    }

    private static List<Stat> GetStatList(string type)
    {
        return new List<Stat>();
    }
}