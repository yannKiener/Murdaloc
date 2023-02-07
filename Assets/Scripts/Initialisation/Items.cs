using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;
using System.Linq;

[System.Serializable]
public static class Items
{

    public static Item GetItemFromDB(string itemName)
    {
        if (itemName.ToLower().Equals("random"))
        {
            return EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel());
        }

        Item result = GetEquipmentFromDB(itemName);
        if (result == null)
        {
            result = GetUselessFromDB(itemName);
        }
        if (result == null)
        {
            result = GetConsumableFromDB(itemName);
        }
        return result;
    }


    public static Equipment GetQuestEquipmentFromDB(string EquipmentName)
    {

        JSONObject Equipment = DatabaseUtils.GetJsonQuestEquipment(EquipmentName);
        if (Equipment == null)
        {
            Debug.Log("Quest Equipment is not found in DB : " + EquipmentName);
            return null;
        }
        else
        {
            JSONObject s = Equipment["stats"].AsObject;
            Stats stats = new Stats(s["force"].AsInt, s["agility"].AsInt, s["intelligence"].AsInt, s["stamina"].AsInt, s["spirit"].AsInt, s["critical"].AsInt, s["haste"].AsInt, s["power"].AsInt, s["autoAttackDamage"].AsInt, s["autoAttackTime"].AsFloat);
            return new Equipment(GetStr(Equipment, "name"), GetStr(Equipment, "description"), Equipment["levelRequirement"].AsInt, stats, ParseEnum<EquipmentType>(GetStr(Equipment, "type")));
        }
    }

    public static Equipment GetEquipmentFromDB(string equipmentName)
    {
        JSONObject equipment = DatabaseUtils.GetJsonEquipment(equipmentName);
        if (equipment == null)
        {
            return null;
        }
        else
        {
            JSONObject s = equipment["stats"].AsObject;
            Stats stats = new Stats(s["force"].AsInt, s["agility"].AsInt, s["intelligence"].AsInt, s["stamina"].AsInt, s["spirit"].AsInt, s["critical"].AsInt, s["haste"].AsInt, s["power"].AsInt, s["autoAttackDamage"].AsInt, s["autoAttackTime"].AsFloat);
            EquipmentQuality quality = EquipmentQuality.Common;
            string qualityString = GetStr(equipment, "quality");
            if (qualityString != null && Enum.GetNames(typeof(EquipmentQuality)).ToList<string>().Contains(qualityString))
            {
                quality = (EquipmentQuality) Enum.Parse(typeof(EquipmentQuality), qualityString, true);
            }
            return new Equipment(GetStr(equipment, "name"), GetStr(equipment, "description"), equipment["levelRequirement"].AsInt, stats, ParseEnum<EquipmentType>(GetStr(equipment, "type")), quality);
        }
    }

    public static Consumable GetConsumableFromDB(string consumableName)
    {
        JSONObject consumable = DatabaseUtils.GetJsonConsumable(consumableName);
        if (consumable == null)
        {
            Debug.Log("Consumable is not found in DB : " + consumableName);
            return null;
        } else
        {
            return new Consumable(GetStr(consumable, "name"), GetStr(consumable, "description"), Spells.Get(GetStr(consumable, "spell")));
        }
    }

    public static Useless GetUselessFromDB(string uselessName)
    {
        JSONObject useless = DatabaseUtils.GetJsonUseless(uselessName);
        if (useless == null)
        {
            return null;
        }
        else
        {
            return new Useless(GetStr(useless, "name"), GetStr(useless, "description"));
        }
    }



    public static void InitializeCategories()
    {
        JSONArray data = DatabaseUtils.GetJsonCategories();

        foreach(JSONObject category in data)
        {
            EquipmentCategories.AddCategory(createEquipmentCategory(category)); 
        }
    }

    private static EquipmentCategory createEquipmentCategory(JSONObject data)
    {
        List<Stat> mainStatList = new List<Stat>();
        foreach(JSONNode stat in data["possibleMainStats"])
        {
            mainStatList.Add(ParseEnum<Stat>(stat));
        }

        List<Stat> offStatList = new List<Stat>();

        foreach (JSONNode stat in data["possibleOffStats"])
        {
            offStatList.Add(ParseEnum<Stat>(stat));
        }

        return new EquipmentCategory(ParseEnum<EquipmentType>(GetStr(data, "type")), ParseEnum<EquipmentSlot>(GetStr(data, "slot")), mainStatList, offStatList);
    }

    private static string GetStr(JSONObject jsonO, string s)
    {
        return jsonO[s].ToString().Replace("\"", "");
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}
