using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Quests {

    static Dictionary<string, Quest> quests = new Dictionary<string, Quest>();
    static Dictionary<string, Quest> trackedQuests = new Dictionary<string, Quest>();

    public static void StartQuest(string name) {

        if(!quests.ContainsKey(name))
        {
            Quest quest = new Quest(name);
            quest.Start();
            quests.Add(name, quest);
        }
    }


    public static Quest GetQuest(string name)
    {
        if (quests.ContainsKey(name))
        {
            return quests[name];
        }

        return null;
    }

    public static void UpdateTrackedQuests()
    {
        
    }
    

}
