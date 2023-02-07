using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Quests {

    static Dictionary<string, Quest> quests = new Dictionary<string, Quest>();

    public static void StartQuest(string name) {

        if(!quests.ContainsKey(name))
        {
            Quest quest = new Quest(name, new Objective("Goretusk", 2)); // TODO : CREER LES OBJECTIVES DEPUIS LA DB ?
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

    public static void UpdateTrackedQuests(Hostile enemy)
    {
        Debug.Log("recieved kill : " + enemy.GetName());
        foreach(Quest quest in quests.Values)
        {
            quest.Update(enemy);
        }
    }
    

    public static void EndQuest(string questName)
    {
        Debug.Log("Ending quest");
        if (quests.ContainsKey(questName))
        {
            quests[questName].End();
            quests.Remove(questName);
        }
    }
    

}
