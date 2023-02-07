using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest{

	string questName;
    List<Objective> objectives;
    bool isQuestReady = false;
	
	public Quest(string name, List<Objective> objectives)
    {
        this.objectives = objectives;
		this.questName = name;
	}

    public Quest(string name, Objective objective)
    {
        this.objectives = new List<Objective>();
        objectives.Add(objective);
        this.questName = name;
    }

    public void Start(){
        DialogStatus.SetStatus(questName + "Started", true);
	}

    public List<Objective> GetObjectives()
    {
        return objectives;
    }
    
    public string GetDescription()
    {
        string result ="\nObjectives :\n ";
        foreach(Objective o in objectives)
        {
            result += o.GetDescription() + "\n";
        }
        return result;
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
                if (!objective.IsOver() && enemy != null )
                {
                    objective.Update(enemy);
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
	
	public void SetReady()
    {
        Debug.Log("Quest Ready to turn in!");
        DialogStatus.SetStatus(questName + "Ready", true);
        isQuestReady = true;
	}
	
	public void End()
    {
        DialogStatus.SetStatus(questName + "Over", true);
	}

}
