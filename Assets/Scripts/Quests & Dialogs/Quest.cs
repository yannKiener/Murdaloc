using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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
        int objectiveOverCounter = 0;
        foreach (Objective objective in objectives)
        {
            objective.Update(enemy);

            if (objective.IsOver())
            {
                objectiveOverCounter++;
            }
        }

        if (objectives.Count == objectiveOverCounter)
        {
            SetReady();
        } else
        {
            UnsetReady();
        }
    }

    public void UnsetReady()
    {
    
        DialogStatus.SetStatus(questName + "Ready", false);
        isQuestReady = false;
    }
	
	public void SetReady()
    {
        if (!isQuestReady)
        {
            Interface.QuestReadyToTurnIn();
            DialogStatus.SetStatus(questName + "Ready", true);
            isQuestReady = true;
        }
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
	
	public bool End()
    {
        if (FindUtils.GetInventoryGrid().HasAtLeastFreeSlots(rewards.Count))
        {
            Interface.QuestDone();
            removeObjectiveItemsInInventory();
            foreach (Item i in rewards)
            {
                FindUtils.GetInventoryGrid().AddItem(i);
            }
            DialogStatus.SetStatus(questName + "Over", true);
            return true;
        } else
        {
            MessageUtils.ErrorMessage("Make space in inventory first.");
            return false;
        }
	}

}
