using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

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

    public static Item GetFromDB(string itemName)
    {
        JSONObject item = DatabaseUtils.GetJsonItem(itemName);
        if (item == null)
        {
            Debug.Log("item is not found in DB");
            return null;
        }
        else
        {
            JSONObject s = item["stats"].AsObject;
            Stats stats = new Stats(s["force"].AsInt, s["agility"].AsInt, s["intelligence"].AsInt, s["stamina"].AsInt, s["spirit"].AsInt, s["critical"].AsInt, s["haste"].AsInt, s["power"].AsInt);
            return new Item(GetStr(item,"name"), GetStr(item,"description"), item["levelRequirement"].AsInt, stats, GetStr(item,"type"));
        }
    }

    private static string GetStr(JSONObject jsonO, string s)
    {
        return jsonO[s].ToString().Replace("\"","");
    }
}
