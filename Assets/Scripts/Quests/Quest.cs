using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest{

	string questName;
    List<Objective> objectives;
    List<Item> rewards;
    bool isQuestReady = false;
	
	public Quest(string name, List<Objective> objectives, List<Item> rewards)
    {
        this.rewards = rewards;
        this.objectives = objectives;
		this.questName = name;
	}

    public void Start(){
        DialogStatus.SetStatus(questName + "Started", true);
	}

    public List<Objective> GetObjectives()
    {
        return objectives;
    }

    public List<Item> GetRewards()
    {
        return rewards;
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
                if (!objective.IsOver())
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
        Interface.QuestReadyToTurnIn(); 
        Debug.Log("Quest Ready to turn in!");
        DialogStatus.SetStatus(questName + "Ready", true);
        isQuestReady = true;
	}

    public bool IsReady()
    {
        return isQuestReady;
    }

    private void removeObjectiveItemsInInventory()
    {

        foreach (Objective objective in objectives)
        {
            if(objective is LootObjective)
            {
                LootObjective o = (LootObjective)objective;
                FindUtils.GetInventoryGrid().RemoveItem(Items.GetQuestEquipmentFromDB(o.GetLootName()));
            }
        }
            
           
    }
	
	public void End()
    {
        Interface.QuestDone();
        removeObjectiveItemsInInventory();
        foreach (Item i in rewards)
        {
            FindUtils.GetInventoryGrid().AddItem(i);
        }
        DialogStatus.SetStatus(questName + "Over", true);
	}

}
