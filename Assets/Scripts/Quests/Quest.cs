using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest{

	string questName;
    List<Objective> objectives;
    bool isQuestReady = false;
	
	public Quest(string name, List<Objective> objectives){
        this.objectives = objectives;
		this.questName = name;
	}
	
	public void Start(){
        DialogStatus.SetStatus(questName + "Started", true);
	}

    public string GetName()
    {
        return questName;
    }
    
    public void Update(Hostile enemy)
    {
        if (!isQuestReady)
        {
            int objectiveOverCounter = 0;
            foreach (Objective objective in objectives)
            {
                if (!objective.IsOver() && enemy != null && objective.GetName().Equals(enemy.name))
                {
                    objective.RemoveOne();
                }

                if (objective.IsOver())
                {
                    objectiveOverCounter++;
                }
            }

            if (objectives.Count == objectiveOverCounter)
            {
                SetReady();
            }
        }
    }
	
	public void SetReady(){
        DialogStatus.SetStatus(questName + "Ready", true);
        isQuestReady = true;
	}
	
	public void End(){
        Quests.EndQuest(this);
        DialogStatus.SetStatus(questName + "Over", true);
	}

}
