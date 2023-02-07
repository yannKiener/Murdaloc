using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public static class EffectsOnTime
{
    private static Dictionary<string, EffectOnTime> effectList = new Dictionary<string, EffectOnTime>();

    public static void Add(EffectOnTime effect)
    {
        effectList.Add(effect.GetName(), effect);
    }

    public static EffectOnTime Get(string effectName)
    {
        return effectList[effectName];
    }
}