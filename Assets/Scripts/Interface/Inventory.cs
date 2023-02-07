using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, Slotable {

    public GameObject slotContainer;
    public GameObject slotPrefab;
    public int slotNumber;


    // Use this for initialization
    void Start()
    {
        for(int i=0; i < slotNumber; i++)
        {
            Instantiate(slotContainer, transform);
        }
    }

    public void OnDragFrom(GameObject slot)
    {

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
            clearChilds(slot.transform);
            slot.GetComponent<Slot>().usable = null;
            return true;
        }
    }

    public bool AddItem(Item item)
    {
        GameObject slot = getFirstFreeSlot();
        if (slot == null)
        {
            MessageUtils.ErrorMessage("Inventory full");
            return false;
        } else
        {
            Item tempItem = item;
            InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, tempItem.GetImageAsSprite(), tempItem);
            return true;
        }
    }

    public void OnDropIn(GameObject slot)
    {
        GameObject tempGameObject = Draggable.currentItem;
        Usable tempUsable = Draggable.currentUsable;
        if (tempUsable is Item) {
            //Si le slot a déjà un contenu, on le supprime 
            if (slot.transform.childCount > 0)
            {
                clearChilds(slot.transform);
            }
            InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, tempGameObject.GetComponent<Image>().sprite, tempUsable);
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

    private void clearChilds(Transform t)
    {

        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
}