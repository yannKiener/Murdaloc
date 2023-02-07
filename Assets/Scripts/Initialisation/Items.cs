using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System;

[System.Serializable]
public static class Items
{
    private static Dictionary<string, Item> itemList = new Dictionary<string, Item>();

    public static void Add(Item item)
    {
        itemList.Add(item.GetName(), item);
    }

    public static Item Get(string itemName)
    {
        return itemList[itemName];
    }

    public static Item GetQuestItemFromDB(string itemName)
    {

        JSONObject item = DatabaseUtils.GetJsonItem(itemName);
        if (item == null)
        {
            Debug.Log("Quest tem is not found in DB : " + itemName);
            return null;
        }
        else
        {
            JSONObject s = item["stats"].AsObject;
            Stats stats = new Stats(s["force"].AsInt, s["agility"].AsInt, s["intelligence"].AsInt, s["stamina"].AsInt, s["spirit"].AsInt, s["critical"].AsInt, s["haste"].AsInt, s["power"].AsInt);
            return new Item(GetStr(item, "name"), GetStr(item, "description"), item["levelRequirement"].AsInt, stats, ParseEnum<ItemType>(GetStr(item, "type")));
        }
    }

    public static Item GetItemFromDB(string itemName)
    {
        JSONObject item = DatabaseUtils.GetJsonItem(itemName);
        if (item == null)
        {
            Debug.Log("Item is not found in DB : " + itemName);
            return null;
        }
        else
        {
            JSONObject s = item["stats"].AsObject;
            Stats stats = new Stats(s["force"].AsInt, s["agility"].AsInt, s["intelligence"].AsInt, s["stamina"].AsInt, s["spirit"].AsInt, s["critical"].AsInt, s["haste"].AsInt, s["power"].AsInt);
            return new Item(GetStr(item,"name"), GetStr(item,"description"), item["levelRequirement"].AsInt, stats, ParseEnum<ItemType>(GetStr(item,"type")));
        }
    }



    public static void InitializeCategories()
    {
        JSONArray data = DatabaseUtils.GetJsonCategories();

        foreach(JSONObject category in data)
        {
            ItemCategories.AddCategory(createItemCategory(category)); 
        }
    }

    private static ItemCategory createItemCategory(JSONObject data)
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

        return new ItemCategory(ParseEnum<ItemType>(GetStr(data, "type")), ParseEnum<ItemSlot>(GetStr(data, "slot")), mainStatList, offStatList);
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
