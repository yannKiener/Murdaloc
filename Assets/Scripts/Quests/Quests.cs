using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Quests {

    static Dictionary<string, Quest> quests = new Dictionary<string, Quest>();

    public static void StartQuest(string name) {

        if(!quests.ContainsKey(name))
        {
            JSONObject questData = DatabaseUtils.GetQuest(name);
            List<Objective> objectives = new List<Objective>();
            foreach(JSONObject objective in questData["objectives"])
            {
                JSONObject killObjective = objective["kill"].AsObject;
                if (killObjective != null && killObjective.Count >= 2)
                {
                    objectives.Add(new KillObjective(killObjective["name"],killObjective["count"].AsInt));
                }

                JSONObject lootObjective = objective["loot"].AsObject;
                if (lootObjective != null && lootObjective.Count >= 2)
                {
                    objectives.Add(new LootObjective(lootObjective["name"], lootObjective["count"].AsInt));
                }
            }

            Quest quest = new Quest(name, objectives); 
            quest.Start();
            quests.Add(name, quest);
            UpdateQuestLog();
        }
    }

    public static Dictionary<string, Quest> GetQuests()
    {
        return quests;
    }


    public static Quest GetQuest(string name)
    {
        if (quests.ContainsKey(name))
        {
            return quests[name];
        }

        return null;
    }

    public static void UpdateTrackedQuests(Hostile enemy)
    {
        foreach(Quest quest in quests.Values)
        {
            quest.Update(enemy);
        }
    }
    

    public static void EndQuest(string questName)
    {
        if (quests.ContainsKey(questName))
        {
            quests[questName].End();
            quests.Remove(questName);
            FindUtils.GetPlayer().AddExp(20);
            UpdateQuestLog();
        }
    }

    public static void UpdateQuestLog()
    {
        FindUtils.GetQuestGrid().UpdateQuestLog();
    }
    

}
