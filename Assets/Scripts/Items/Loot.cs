using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Loot : MonoBehaviourWithMouseOverColor {
    List<Item> itemList;
    public bool isOpen = false;
    bool checkStarted = false;

    // Use this for initialization
    new void Start ()
    {
        base.Start();
        if(itemList != null)
        {
            foreach (Item item in itemList)
            {
                item.isInInventory = false;
            }
        }
        Destroy(this.gameObject, Constants.DeleteLootAfterSeconds);
    }

    private void Update()
    {
        if (isOpen && !checkStarted)
        {
            InvokeRepeating("CloseWindowIfPlayerFarOrDead", 1f, 1f);
            checkStarted = true;
        }
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

    void CloseWindowIfPlayerFarOrDead()
    {
        if (FindUtils.GetLoot().activeSelf && !IsPlayerNearAndAlive())
        {
            checkStarted = false;
            CancelInvoke();
            FindUtils.GetLoot().SetActive(false);
        }
    }

    bool IsPlayerNearAndAlive()
    {
        if (!FindUtils.GetPlayer().IsDead())
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
            MessageUtils.ErrorMessage("It's too far away !");
        }

        return false;
    }

    void OnMouseDown()
    {
        if (IsPlayerNearAndAlive() && itemList != null)
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

}
