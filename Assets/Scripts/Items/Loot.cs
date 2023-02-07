using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Loot : MonoBehaviourWithMouseOverColor {
    List<Item> itemList;

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        foreach (Item item in itemList)
        {
            item.isInInventory = false;
        }

        Destroy(this, Constants.DeleteLootAfterSeconds);
    }

    private void Update()
    {
        CloseWindowIfPlayerFar();
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

    void CloseWindowIfPlayerFar()
    {
        if (FindUtils.GetLoot().activeSelf && !IsPlayerNear())
        {
            FindUtils.GetLoot().SetActive(false);
        }
    }

    bool IsPlayerNear()
    {

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), Constants.LootMaxDistance);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Player")
            {
                return true;
            }
            i++;
        }
        return false;
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
