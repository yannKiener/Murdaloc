using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Loot : MonoBehaviour  {
    List<Item> itemList;

    // Use this for initialization
    void Start ()
    {
        foreach(Item item in itemList)
        {
            item.isInInventory = false;
        }

        Destroy(this, Constants.DeleteLootAfterSeconds);
    }

    public void Initialize(List<Item> itemList, Vector3 position)
    {
        transform.position = position;
        this.itemList = itemList;
        if (itemList.Count == 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void AddItem(Item item)
    {
        item.isInInventory = false;
        itemList.Add(item);
    }

    public void RemoveItem(Item item)
    {
        item.isInInventory = true;
        itemList.Remove(item);

    }
    
    void OnDestroy()
    {
        //FindUtils.GetLootGrid().GetComponent<LootInventory>().Close();
    }

    void OnMouseDown()
    {
        LootInventory lootInventory = FindUtils.GetLootGrid().GetComponent<LootInventory>();

        if (FindUtils.GetLoot().activeSelf)
        {
            lootInventory.Close();
        }

        FindUtils.GetLoot().SetActive(true);
        lootInventory.Initialize(itemList, this);

    }



}
