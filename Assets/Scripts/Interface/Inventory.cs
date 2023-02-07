using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Inventory : MonoBehaviour, Slotable {

    public GameObject slotContainer;
    public GameObject slotPrefab;
    public int slotNumber;

    private int gold = 0;
    private int silver = 0;
    private int copper = 0;
    private GameObject copperCount;
    private GameObject silverCount;
    private GameObject goldCount;
    private GameObject copperIcon;
    private GameObject silverIcon;
    private GameObject goldIcon;

    void OnEnable()
    {
        FindUtils.GetInterface().OpenInventory();
    }

    void OnDisable()
    {
        FindUtils.GetInterface().CloseInventory();
    }

    // Use this for initialization
    void Start()
    {
        for(int i=0; i < slotNumber; i++)
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
        copperCount.GetComponent<Text>().text = copper.ToString();
        if(gold > 0 || silver > 0)
        {
            silverCount.SetActive(true);
            silverIcon.SetActive(true);
            silverCount.GetComponent<Text>().text = silver.ToString();
        } else
        {
            silverCount.SetActive(false);
            silverIcon.SetActive(false);
        }
        if (gold > 0)
        {
            goldCount.SetActive(true);
            goldIcon.SetActive(true);
            goldCount.GetComponent<Text>().text = gold.ToString();
        } else
        {
            goldCount.SetActive(false);
            goldIcon.SetActive(false);

        }
    }

    public bool RemoveCopper(int price)
    {
        if (copper >= price)
        {
            copper -= price;
            UpdateGoldGui();
            return true;
        } else
        {
            if (RemoveSilver(1))
            {
                copper += 100;
                copper -= price;
                UpdateGoldGui();
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
        UpdateGoldGui();
    }

    public bool RemoveSilver(int price)
    {
        if (silver >= price)
        {
            silver -= price;
            UpdateGoldGui();
            return true;
        } else
        {
            if (RemoveGold(1))
            {
                silver += 100;
                silver -= price;
                UpdateGoldGui();
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
        UpdateGoldGui();
    }

    public bool RemoveGold(int price)
    {
        if(gold >= price)
        {
            gold -= price;
            UpdateGoldGui();
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
        UpdateGoldGui();
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
        GameObject slot = GetSlotWith(item.GetName());
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
            tempItem.isInInventory = true;

            if (tempItem is Equipment)
                ((Equipment)tempItem).isEquipped = false;

            if (tempItem is Consumable && GetSlotWith(tempItem.GetName()) != null)
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
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable.GetName() == itemName)
            {
                count++;
            }
        }

        return count;
    }

    public void OnDropIn(GameObject slot, PointerEventData eventData)
    {
        Usable tempUsable = Draggable.currentUsable;
        if (tempUsable is Item) {
            AddItem((Item)tempUsable);
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
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable.GetName() == name)
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
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable.GetName() == name)
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