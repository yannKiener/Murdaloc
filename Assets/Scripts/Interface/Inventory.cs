using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, Slotable {

    public GameObject slotContainer;
    public GameObject slotPrefab;
    public int slotNumber;

    private int cash = 0;
    private GameObject copperCount;
    private GameObject silverCount;
    private GameObject goldCount;
    private GameObject copperIcon;
    private GameObject silverIcon;
    private GameObject goldIcon;

    void OnEnable()
    {
        Interface.OpenInventory();
        updateConsumablesStacks();
    }

    private void updateConsumablesStacks()
    {

        for (int i = 0; i < slotNumber; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable is Consumable)
            {
                ((Consumable)slot.GetChild(0).GetComponent<Draggable>().usable).UpdateInventoryStacks();
            }
        }

    }

    void OnDisable()
    {
        Interface.CloseInventory();
    }

    void Awake()
    {
        for (int i = 0; i < slotNumber; i++)
        {
            Instantiate(slotContainer, transform);
        }

        Transform money = transform.parent.Find("Money");
        copperCount = money.Find("CopperCount").gameObject;
        silverCount = money.Find("SilverCount").gameObject;
        goldCount = money.Find("GoldCount").gameObject;
        silverIcon = money.Find("SilverIcon").gameObject;
        goldIcon = money.Find("GoldIcon").gameObject;

        UpdateGoldGui();
    }

    public void OnDragFrom(GameObject slot)
    {
    }

    public void UpdateGoldGui()
    {
        copperCount.GetComponent<Text>().text = GetCopper().ToString();
        if(GetGold() > 0 || GetSilver() > 0)
        {
            silverCount.SetActive(true);
            silverIcon.SetActive(true);
            silverCount.GetComponent<Text>().text = GetSilver().ToString();
        } else
        {
            silverCount.SetActive(false);
            silverIcon.SetActive(false);
        }
        if (GetGold() > 0)
        {
            goldCount.SetActive(true);
            goldIcon.SetActive(true);
            goldCount.GetComponent<Text>().text = GetGold().ToString();
        } else
        {
            goldCount.SetActive(false);
            goldIcon.SetActive(false);

        }
    }

    public void SellItem(Item item)
    {
        if (RemoveItem(item))
        {
            Interface.CoinSound();
            AddCash(item.GetSellPrice());
        }

    }

    public int GetCash()
    {
        return cash;
    }

    public int GetCopper()
    {
        return InterfaceUtils.GetCopper(cash);
    }

    public int GetSilver()
    {
        return InterfaceUtils.GetSilver(cash);
    }

    public int GetGold()
    {
        return InterfaceUtils.GetGold(cash);
    }

    public bool HasEnoughCash(int price)
    {
        if (cash >= price)
        {
            return true;
        }
        else
        {
            MessageUtils.ErrorMessage("Not enough gold");
            return false;
        }
    }

    public bool RemoveCash(int price)
    {
        if(HasEnoughCash(price))
        {
            cash -= price;
            UpdateGoldGui();
            return true;
        }
        return false;
    }


    public void AddCash(int price)
    {
        cash += price;
        UpdateGoldGui();
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
        GameObject slot = GetSlotWith(item.GetName());
        if (slot == null)
        {
            MessageUtils.ErrorMessage("No item found : " + item.GetName());
            return false;
        }
        else
        {
            if(item is Consumable && ((Consumable)item).GetStacks() > 1)
            {
                ((Consumable)item).RemoveOne();
            } else
            {
                slot.GetComponent<Slot>().usable = null;
                slot.GetComponentInChildren<Draggable>().usable = null;
                clearChilds(slot.transform);
                Quests.UpdateTrackedQuests(null);
            }

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

    public bool AddItem(Item item, bool isFromDrag = false)
    {
        GameObject slot = getFirstFreeSlot();
        if (slot == null)
        {
            MessageUtils.ErrorMessage("Inventory full");
            return false;
        } else
        {
            Item tempItem = item;
            tempItem.isInInventory = true;

            if (tempItem is Equipment)
                ((Equipment)tempItem).isEquipped = false;

            bool isConsumable = tempItem is Consumable;

            if (isConsumable && GetSlotWith(tempItem.GetName()) != null && !isFromDrag)
            {
                getConsumableWithName(tempItem.GetName()).AddOne();
                return true;
            } else
            {
                InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, tempItem.GetImageAsSprite(), tempItem);
                Quests.UpdateTrackedQuests(null);
                return true;
            }

        }
    }

    public int HasItem(string itemName)
    {
        int count = 0;

        for (int i = 0; i < slotNumber; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable != null && slot.GetChild(0).GetComponent<Draggable>().usable.GetName() == itemName)
            {
                count++;
            }
        }

        return count;
    }

    public void OnDropIn(GameObject slot, PointerEventData eventData)
    {
        if (slot.transform.childCount > 0 && Draggable.currentItem == slot.transform.GetChild(0).gameObject)
        {
            eventData.Use();
        }
        else
        {
            Usable tempUsable = Draggable.currentUsable;
            if (tempUsable is Item)
            {
                if (!AddItem((Item)tempUsable, true))
                {
                    eventData.Use();
                }
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


    private Consumable getConsumableWithName(string name)
    {
        for (int i = 0; i < slotNumber; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable != null && slot.GetChild(0).GetComponent<Draggable>().usable.GetName() == name)
            {
                return (Consumable)slot.GetChild(0).GetComponent<Draggable>().usable;
            }
        }
        return null;
    }


    public GameObject GetSlotWith(string name)
    {
        for (int i = 0; i < slotNumber; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable != null && slot.GetChild(0).GetComponent<Draggable>().usable.GetName() == name)
            {
                return slot.gameObject;
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

    public List<Item> GetItems()
    {

        List<Item> items = new List<Item>();
        foreach (Transform child in transform)
        {
            Item i = (Item)child.gameObject.GetComponent<Slot>().usable;
            if (i != null)
            {
                items.Add(i);
            }
        }
        return items;
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
        slot.GetComponent<Slot>().usable = Draggable.currentUsable;
        Draggable.currentItem.transform.position = Draggable.originalPosition;
    }
}