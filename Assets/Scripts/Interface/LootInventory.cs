﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootInventory : MonoBehaviour, Slotable
{

    public GameObject slotContainer;
    public GameObject slotPrefab;
    public int slotNumber;
    public Loot loot;

    void OnEnable()
    {
        Interface.OpenInventory();
    }
    

    // Use this for initialization
    void Awake()
    {
        this.slotNumber = 9;

        if (slotContainer == null)
        {
            slotContainer = Resources.Load<GameObject>("Prefab/UI/SlotContainer");
        }

        if (slotPrefab == null)
        {
            slotPrefab = Resources.Load<GameObject>("Prefab/UI/UsableSlot");
        }

        for (int i = 0; i < slotNumber; i++)
        {
            Instantiate(slotContainer, transform);
        }

    }

    public void Initialize(List<Item> itemList, Loot loot)
    {
        if(this.loot != null)
        {
            this.loot.isOpen = false;
        }
        this.loot = null;
        clean();

        
        AddItems(itemList);
        this.loot = loot;
        loot.isOpen = true;
    }

    void Update()
    {
        if (loot != null && IsEmpty())
        {
            Close();
            Destroy(loot.gameObject);
        }
    }

    public void OnDragFrom(GameObject slot)
    {
        loot.RemoveItem((Item)Draggable.currentUsable);
    }
    

    public bool IsEmpty()
    {
        for (int i = 0; i < slotNumber; i++)
        {
            if (transform.GetChild(i).childCount != 0)
            {
                return false;
            }
        }
        return true;
    }

    public bool RemoveItem(Item item)
    {
        GameObject slot = getSlotWithItem(item);
        if (slot == null)
        {
            MessageUtils.ErrorMessage("No item found : " + item.GetName());
            return false;
        }
        else
        {
            loot.RemoveItem(item);
            clearChilds(slot.transform);
            slot.GetComponent<Slot>().usable = null;
            return true;
        }
    }

    private GameObject getSlotWithItem(Item item)
    {
        for (int i = 0; i < slotNumber; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable == item)
            {
                return slot.gameObject;
            }
        }
        return null;
    }

    public bool AddItem(Item item)
    {
        GameObject slot = getFirstFreeSlot();
        if (slot == null)
        {
            MessageUtils.ErrorMessage("Inventory full");
            return false;
        }
        else
        {
            Item tempItem = item;
            if (tempItem is Equipment)
                ((Equipment)tempItem).isEquipped = false;

            InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, tempItem.GetImageAsSprite(), tempItem);
            return true;
        }
    }

    public void AddItems(List<Item> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            Item tempItem = items[i];
            InterfaceUtils.CreateUsableSlot(slotPrefab, transform.GetChild(i), tempItem.GetImageAsSprite(), tempItem);
        }
    }

    public void OnDropIn(GameObject slot, PointerEventData eventData)
    {
        Usable tempUsable = Draggable.currentUsable;

        if (tempUsable is Item)
        {
            GameObject freeSlot = getFirstFreeSlot();
            if (freeSlot != null)
            {
                loot.AddItem((Item)tempUsable);
                AddItem((Item)tempUsable);

            } else
            {
                //L'objet est supprimé !
            }
        }
    }

    private GameObject getFirstFreeSlot()
    {
        for (int i = 0; i < slotNumber; i++)
        {
            if (transform.GetChild(i).childCount == 0)
            {
                return transform.GetChild(i).gameObject;
            }
        }
        return null;
    }

    void OnDisable()
    {
        if(loot != null)
        {
            loot.isOpen = false;
        }
        Interface.CloseInventory();
        Close();
    }

    public void Close()
    {
        clean();
        FindUtils.GetLoot().SetActive(false);
    }

    private void clean()
    {
        foreach (Transform c in transform)
        {
            clearChilds(c);
        }
    }

    private void clearChilds(Transform t)
    {

        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }

    public void ResetDrag(GameObject slot)
    {
        loot.AddItem((Item)Draggable.currentUsable);
        slot.GetComponent<Slot>().usable = Draggable.currentUsable;
        Draggable.currentItem.transform.position = Draggable.originalPosition;
    }
}