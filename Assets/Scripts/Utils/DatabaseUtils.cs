using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public static class DatabaseUtils {
    public static string GetJsonItems()
    {
        return Resources.Load<TextAsset>("Data/Items/items").text;
    }

    public static JSONArray GetJsonCategoryName(ItemType type)
    {
        return JSON.Parse(Resources.Load<TextAsset>("Data/Items/categories").text).AsObject["names"].AsObject[type.ToString()].AsArray;
    }


    public static JSONArray GetJsonStatNames(Stat stat)
    {
        return JSON.Parse(Resources.Load<TextAsset>("Data/Items/categories").text).AsObject["statNames"].AsObject[stat.ToString()].AsArray;
    }
    

    public static Sprite[] LoadAllSpritesForType(ItemType type)
    {
       return Resources.LoadAll<Sprite>("Images/Items/" + type.ToString());
    }


    public static JSONArray GetJsonCategories()
    {
        return JSON.Parse(Resources.Load<TextAsset>("Data/Items/categories").text).AsObject["categories"].AsArray;
    }


    public static JSONObject GetJsonItem(string itemName)
    {
        JSONObject result = JSON.Parse(Resources.Load<TextAsset>("Data/Items/items").text).AsObject[itemName].AsObject;
        if (result == null)
        {
            return null;
        }else
        {
            result.Add("name", itemName);
            return result;
        }
    }
	
	
	public static Dialog GetDialog(string dialogName){
        JSONObject data = JSON.Parse(Resources.Load<TextAsset>("Data/Dialogs/"+dialogName).text).AsObject;
		return createDialog(data);
	}
	
	private static List<Choice> createChoices(JSONArray data){
        List<Choice> result = new List<Choice>();
        foreach (JSONNode jsonNode in data)
        {
            Choice c = new Choice();
            c.SetChoiceText( GetStr(jsonNode.AsObject, "choice"));
            c.SetCondition(jsonNode["condition"].AsBool);
            c.SetDialog(createDialog(jsonNode["dialog"].AsObject));
            result.Add(c);
        }
        return result;
		
	}
	
	private static Dialog createDialog(JSONObject data){
        Dialog result = new Dialog();
        result.SetText(GetStr(data, "text"));
        result.SetAction(GetStr(data, "action"));
        result.SetStartQuest(GetStr(data, "startQuest"));
        result.SetEndQuest(GetStr(data, "endQuest"));

        if (data["choices"].AsArray.Count != 0){
			result.SetChoices(createChoices(data["choices"].AsArray));
		}
		return result;
	}
	
	
    private static string GetStr(JSONObject jsonO, string s)
    {
        if (jsonO != null && s != null && jsonO[s] != null)
        {
            return jsonO[s].ToString().Replace("\"", "");
        }
        else
        {
            return null;
        }
    }
}
