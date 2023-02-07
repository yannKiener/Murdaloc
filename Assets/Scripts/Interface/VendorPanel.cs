using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorPanel : MonoBehaviour
{

    public GameObject sellSlotContainer;

    private Dictionary<Item, bool> GetDefaultItems()
    {
        Dictionary<Item, bool> itemList = new Dictionary<Item, bool>();
        itemList.Add(Items.GetConsumableFromDB("Haunch of Meat"),true);
        itemList.Add(Items.GetConsumableFromDB("Ice Cold Milk"),true);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);
        itemList.Add(EquipmentGenerator.GenerateEquipment(FindUtils.GetPlayer().GetLevel()),false);

        return itemList;
    }

    public void Initialize(Dictionary<Item, bool> itemList)
    {
        clearChilds(transform);
        if(itemList == null || itemList.Count == 0)
        {
            itemList = GetDefaultItems();
        }

        foreach (KeyValuePair<Item,bool> item in itemList)
        {
            GameObject sellContainer = Instantiate(sellSlotContainer,transform);
            sellContainer.GetComponentInChildren<SellSlot>().Initialize(item.Key, item.Value);
        }
    }

    void OnEnable()
    {
        FindUtils.GetInterface().OpenVendor();
    }

    void OnDisable()
    {
        FindUtils.GetInterface().CloseVendor();
    }

    private void clearChilds(Transform t)
    {
        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
}