using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public static class DatabaseUtils {
    public static string GetJsonItems()
    {
        return Resources.Load<TextAsset>("items").text; 
    }


    public static JSONObject GetJsonItem(string itemName)
    {
        JSONObject result = JSON.Parse(Resources.Load<TextAsset>("items").text).AsObject[itemName].AsObject;
        if (result == null)
        {
            return null;
        }else
        {
            result.Add("name", itemName);
            return result;
        }
    }
}
