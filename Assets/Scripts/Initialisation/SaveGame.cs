using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveGame {
    string name;
    int cash;
    int level;
    float expPercent;
    int rsrc;
    int talentPoints;
    List<string> spellList;
    List<Equipment> charSheetItems;
    List<Item> inventoryItems;
    Dictionary<string, bool> status;
    string lastScene;
    List<Quest> quests;

    Specialisation sp1;
    Specialisation sp2;
    Specialisation sp3;

    List<string> actionBarSpells;

    public SaveGame()
    {
        CharacterSheet chSheet = FindUtils.GetCharacterSheetGrid();
        Inventory inv = FindUtils.GetInventoryGrid();
        Player player = FindUtils.GetPlayer();
        TalentSheet tSheet = FindUtils.GetTalentSheetGrid();

        this.cash = inv.GetCash();
        this.name = player.GetName();
        this.level = player.GetLevel();
        this.expPercent = player.GetExp();
        this.talentPoints = player.GetTalentPoints();
        Resource rs = player.GetResourceType();
        if (rs is Rage)
        {
            rsrc = 1;
        } else if (rs is Energy)
        {
            rsrc = 2;
        } else
        {
            rsrc = 3;
        }
        this.spellList = player.GetSpells().Keys.ToList<string>();
        this.charSheetItems = chSheet.GetEquipments();
        this.inventoryItems = inv.GetItems();
        this.status = DialogStatus.GetAllStatus();
        lastScene = SceneManager.GetActiveScene().name;
        this.quests = Quests.GetQuests().Values.ToList<Quest>();
        sp1 = tSheet.GetSpec1();
        sp2 = tSheet.GetSpec2();
        sp3 = tSheet.GetSpec3();

        actionBarSpells = FindUtils.GetActionBar().GetSave();
    }

    public string GetLastScene()
    {
        return lastScene;
    }

    public int GetLevel()
    {
        return level;
    }

    public void LoadData()
    {
        Resource r;
        if(rsrc == 1)
        {
            r = new Rage();
        } else if(rsrc == 2)
        {
            r = new Energy();
        } else
        {
            r = new Mana();
        }

        List<Spell> sList = new List<Spell>();
        foreach(string spellName in spellList)
        {
            sList.Add(Spells.Get(spellName));
        }


        FindUtils.GetInventoryGrid().AddCash(cash);



        DialogStatus.SetAllStatus(status);
        foreach(KeyValuePair<string, bool> kv in status)
        {
            Debug.Log(kv.Key + " " + kv.Value);
        }
        Debug.Log(name);
        FindUtils.GetPlayer().InitializeWith(name, level, talentPoints, expPercent, r, sList);

        FindUtils.GetTalentSheetGrid().LoadSpecsSave(sp1, sp2, sp3);

        foreach (Item item in inventoryItems)
        {
            item.LoadImage();

            if (item is Consumable)
            {
                ((Consumable)item).LoadSpell();
            }
            FindUtils.GetInventoryGrid().AddItem(item);
        }

        foreach (Equipment item in charSheetItems)
        {
            item.LoadImage();
            FindUtils.GetCharacterSheetGrid().EquipEquipment(item);
        }
        FindUtils.GetPlayer().SetFullHealthAndMaxResource();

        Quests.LoadQuests(quests);
        Quests.UpdateQuestLog();

        FindUtils.GetActionBar().Load(actionBarSpells);

    }
}
