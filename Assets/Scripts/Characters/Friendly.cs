using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Friendly : Character
{

    public string DialogName;
    public List<SellTable> SellTable;
    private Dictionary<Item, bool> sellTable = new Dictionary<Item, bool>();


    new void Start()
    {
        base.Start();
        if (DialogName != null)
        {
            AddDialog(DialogName);
        }

        if(SellTable != null && SellTable.Count > 0) { 
            foreach (SellTable st in SellTable)
            {
                sellTable.Add(Items.GetItemFromDB(st.itemName), st.hasInfinite);
            }
        } else
        {
            sellTable = GetDefaultSellItems();
        }
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

    void OnMouseDown()
    {
        FindUtils.GetPlayer().SetTarget(this);
        //check distance
        FindUtils.GetDialogBoxComponent().Initialize(this);
    }
}
