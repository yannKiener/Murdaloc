using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public static class Specialisations
{
    private static Dictionary<string, Specialisation> specialisations = new Dictionary<string, Specialisation>();

    public static void Add(Specialisation spec)
    {
        specialisations[spec.GetName()] = spec;
    }

    public static Specialisation Get(string specName)
    {
        Specialisation s = specialisations[specName];
        return new Specialisation(s);
    }
}
