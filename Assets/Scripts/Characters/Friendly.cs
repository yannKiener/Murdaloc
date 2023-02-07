using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Friendly : Character
{

    public string Title;
    public string DialogName;
    public List<SellTable> SellTable;
    private Dictionary<Item, bool> sellTable = new Dictionary<Item, bool>();

    public List<AudioClip> talkSounds;


    new void Start()
    {
        base.Start();
        this.tag = "Friendly";
        if (DialogName != null)
        {
            AddDialog(DialogName);
        }

        if (SellTable != null && SellTable.Count > 0) {
            foreach (SellTable st in SellTable)
            {
                sellTable.Add(Items.GetItemFromDB(st.itemName), st.hasInfinite);
            }
        } else
        {
            sellTable = GetDefaultSellItems();
        }

        DisplayNameAndTitle();
    }

    public override void OnUpdate() { 
        if (!IsDead())
        {
            UpdateRegen();
            UpdateEffects();
        }
    }

    private void DisplayNameAndTitle()
    {
        GameObject textOverFriendlyPrefab =Resources.Load<GameObject>("Prefab/UI/TextOverFriendly");
        if (textOverFriendlyPrefab != null)
        {
            GameObject textOverFriendly = Instantiate(textOverFriendlyPrefab, this.transform);
            string nameAndTitle = (GetTitle() != null && GetTitle().Length > 3) ? GetName() + "\n" + GetTitle() : GetName();
            textOverFriendly.GetComponentInChildren<Text>().text = nameAndTitle;
        } else
        {
            Debug.Log("GameObject for Friendly names is null ! ");
        }

    }

    public void SetTitle(string title)
    {
        this.Title = title;
    }

    public string GetTitle()
    {
        return "<" + Title + ">";
    }

    public List<AudioClip> GetTalkSounds()
    {
        return talkSounds;
    }

    private Dictionary<Item, bool> GetDefaultSellItems()
    {
        Dictionary<Item, bool> itemList = new Dictionary<Item, bool>();
        itemList.Add(Items.GetConsumableFromDB("Haunch of Meat"), true);
        itemList.Add(Items.GetConsumableFromDB("Ice Cold Milk"), true);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()), false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()), false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()), false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()), false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()), false);

        return itemList;
    }

    public Dictionary<Item, bool> GetSellTable()
    {
        return sellTable;
    }

    public void AddItemToSellTable(Item item, bool hasInfinite)
    {
        if (!sellTable.ContainsKey(item))
        {
            sellTable.Add(item, hasInfinite);
        }
        FindUtils.GetVendorPanel().RefreshSelf();
    }

    public void RemoveItemFromSellTable(Item item)
    {
        sellTable.Remove(item);
        FindUtils.GetVendorPanel().RefreshSelf();
    }


    public override void OnClickPlayerCloseEnough() {

        if (!FindUtils.GetPlayer().IsDead())
        {
            FindUtils.GetDialogBoxComponent().Initialize(this);
        }
    }

    public override void OnPlayerFarOrDead() {
        FindUtils.GetDialogBox().SetActive(false);
    }

}
