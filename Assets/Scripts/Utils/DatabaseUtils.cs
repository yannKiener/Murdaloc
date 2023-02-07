using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.Linq;

public static class DatabaseUtils {

    public static JSONArray GetJsonCategoryName(EquipmentType type)
    {
        return JSON.Parse(Resources.Load<TextAsset>("Data/Items/categories").text).AsObject["names"].AsObject[type.ToString()].AsArray;
    }

    public static JSONObject GetQuest(string questName)
    {
        return JSON.Parse(Resources.Load<TextAsset>("Data/Quests/" + questName).text).AsObject;
    }

    public static JSONArray GetJsonStatNames(Stat stat)
    {
        return JSON.Parse(Resources.Load<TextAsset>("Data/Items/categories").text).AsObject["statNames"].AsObject[stat.ToString()].AsArray;
    }
    
    public static Sprite[] LoadAllSpritesForType(EquipmentType type)
    {
       return Resources.LoadAll<Sprite>("Images/Items/" + type.ToString());
    }

    public static JSONArray GetJsonCategories()
    {
        return JSON.Parse(Resources.Load<TextAsset>("Data/Items/categories").text).AsObject["categories"].AsArray;
    }

    public static JSONObject GetJsonQuestEquipment(string itemName)
    {
        JSONObject result = JSON.Parse(Resources.Load<TextAsset>("Data/Items/questItems").text).AsObject[itemName].AsObject;

        if (result == null || result.Count == 0)
        {
            return null;
        }
        else
        {
            result.Add("name", itemName);
            return result;
        }
    }

    public static JSONObject GetJsonEquipment(string itemName)
    {
        JSONObject result = JSON.Parse(Resources.Load<TextAsset>("Data/Items/equipments").text).AsObject[itemName].AsObject;
        
        if (result == null || result.Count == 0)
        {
            return null;
        }else
        {
            result.Add("name", itemName);
            return result;
        }
    }

    public static JSONObject GetJsonConsumable(string consumableName)
    {
        JSONObject result = JSON.Parse(Resources.Load<TextAsset>("Data/Items/consumables").text).AsObject[consumableName].AsObject;

        if (result == null || result.Count == 0)
        {
            return null;
        }
        else
        {
            result.Add("name", consumableName);
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
            if (GetStr(jsonNode.AsObject, "condition") != null)
            {
                List<string> conditions = GetStr(jsonNode.AsObject, "condition").Split('&').ToList();
                foreach (string condition in conditions)
                {
                    c.AddCondition(condition);
                }
            }

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

    public static List<AudioClip> GetWeaponAudio(string weapType)
    {
        return GetAudioClips("Weapons/" + weapType);
    }

    private static List<AudioClip> GetAudioClips(string path)
    {
        List<AudioClip> precastSounds = Resources.LoadAll<AudioClip>("Sounds/" + path).ToList<AudioClip>();
        if(precastSounds == null || precastSounds.Count == 0)
        {
            Debug.Log("No sounds found for path '" + path + "'");
        }
        return precastSounds;
    }
    
    private static List<AudioClip> GetSpellAudio(string path)
    {
        return GetAudioClips("Spells/" + path);
    }

    public static AudioClip GetPrecastSound(string type)
    {
        List<AudioClip> precastSounds = GetSpellAudio(type + "/Precast");
        if(precastSounds != null && precastSounds.Count > 0)
        {
            return GetSpellAudio(type + "/Precast")[0];
        }
        else
        {
            return null;
        }
    }

    public static List<AudioClip> GetCastSound(string type)
    {
        return GetSpellAudio(type + "/Cast");
    }

    public static List<AudioClip> GetIImpactSound(string type)
    {
        return GetSpellAudio(type + "/Impact");
    }
}
