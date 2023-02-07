using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, Slotable {

    public GameObject slotContainer;
    public GameObject slotPrefab;
    public int slotNumber;

    private int gold = 0;
    private int silver = 0;
    private int copper = 0;

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

    public bool RemoveCopper(int price)
    {
        if (copper >= price)
        {
            copper -= price;
            return true;
        } else
        {
            if (RemoveSilver(1))
            {
                copper += 100;
                copper -= price;
                return true;
            }
        }
        return false;
    }

    public int GetCopper()
    {
        return copper;
    }

    public void AddCopper(int price)
    {
        if (copper >= 100)
        {
            copper -= 100;
            AddSilver(1);
        }
        copper += price;
    }

    public bool RemoveSilver(int price)
    {
        if (silver >= price)
        {
            silver -= price;
            return true;
        } else
        {
            if (RemoveGold(1))
            {
                silver += 100;
                silver -= price;
                return true;
            }
        }
        return false;
    }

    public int GetSilver()
    {
        return silver;
    }

    public void AddSilver(int price)
    {
        if(silver >= 100)
        {
            silver -= 100;
            AddGold(1);
        }
        silver += price;
    }

    public bool RemoveGold(int price)
    {
        if(gold >= price)
        {
            gold -= price;
            return true;
        }
        MessageUtils.ErrorMessage("Not enough gold");
        return false;
    }

    public int GetGold()
    {
        return gold;
    }

    public void AddGold(int price)
    {
        gold += price;
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
            clearChilds(slot.transform);
            slot.GetComponent<Slot>().usable = null;
            Quests.UpdateTrackedQuests(null);
            return true;
        }
    }

    public bool HasAtLeastFreeSlots(int number)
    {
        int count = 0;
        for (int i = 0; i < slotNumber; i++)
        {
            if (transform.GetChild(i).childCount == 0)
            {
                count++;
            }
        }
        return count >= number;
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
            Quests.UpdateTrackedQuests(null);
            return true;
        }
    }

    public int HasItem(string itemName)
    {
        int count = 0;

        for (int i = 0; i < slotNumber; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable.GetName() == itemName)
            {
                count++;
            }
        }

        return count;
    }

    public void OnDropIn(GameObject slot)
    {
        GameObject tempGameObject = Draggable.currentItem;
        Usable tempUsable = Draggable.currentUsable;
        if (tempUsable is Item) {
            ((Item)tempUsable).isInInventory = true;
            //Si le slot a déjà un contenu, on le supprime 
            if (slot.transform.childCount > 0)
            {
                clearChilds(slot.transform);
            }
            InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, tempGameObject.GetComponent<Image>().sprite, tempUsable);
            Quests.UpdateTrackedQuests(null);
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