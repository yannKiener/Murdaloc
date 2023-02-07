using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSheet : MonoBehaviour, Slotable {

    public GameObject slotContainer;
    public GameObject slotPrefab;

    public void OnDragFrom(GameObject slot)
    {
        ((Item)Draggable.currentUsable).isEquipped = false;
    }

    public void OnDropIn(GameObject slot)
    {
        Usable tempUsable = Draggable.currentUsable;
        if (tempUsable is Item)
        {
            EquipItem((Item)tempUsable);
        }
    }

    public bool RemoveItem(Item item)
    {
        GameObject slot = getSlotWithItem(item);
        return RemoveItemSlot(slot);
    }

    public bool RemoveItemSlot(GameObject slot)
    {

        if (slot != null && (Item)slot.GetComponent<Slot>().usable != null)
        {
            Item item = (Item)slot.GetComponent<Slot>().usable;
            //On retire l'objet
            item.isEquipped = false;
            clearChilds(slot.transform);
            slot.GetComponent<Slot>().usable = null;
            FindUtils.GetInventoryGrid().AddItem(item);
            FindUtils.GetPlayer().GetStats().Remove(item.GetStats());
            return true;
        }
        return false;
    }

    public void EquipItem(Item item)
    {
        if (FindUtils.GetInventoryGrid().HasAtLeastFreeSlots(1))
        {
            switch (item.GetItemSlot())
            {
                case ItemSlot.Head:
                    ClearSlot(ItemSlot.Head);
                    EquipHead(item);
                    break;
                case ItemSlot.Neck:
                    ClearSlot(ItemSlot.Neck);
                    EquipNeck(item);
                    break;
                case ItemSlot.Torso:
                    ClearSlot(ItemSlot.Torso);
                    EquipTorso(item);
                    break;
                case ItemSlot.Legs:
                    ClearSlot(ItemSlot.Legs);
                    EquipLegs(item);
                    break;
                case ItemSlot.Belt:
                    ClearSlot(ItemSlot.Belt);
                    EquipBelt(item);
                    break;
                case ItemSlot.Hands:
                    ClearSlot(ItemSlot.Hands);
                    EquipHands(item);
                    break;
                case ItemSlot.Ring:
                    ClearSlot(ItemSlot.Ring);
                    EquipRing(item);
                    break;
                case ItemSlot.Feet:
                    ClearSlot(ItemSlot.Feet);
                    EquipFeet(item);
                    break;
                case ItemSlot.TwoHanded:

                    if (FindUtils.GetInventoryGrid().HasAtLeastFreeSlots(1))
                    {
                        ClearSlot(ItemSlot.TwoHanded);
                        EquipWeapon1(item);
                    } else
                    {
                        MessageUtils.ErrorMessage("Make some space before equipping.");
                    }
                    break;
                case ItemSlot.Weapon1:
                    if (GetItemForSlot(ItemSlot.Weapon1) == null)
                    {
                        EquipWeapon1(item);
                    } else
                    {
                        if (GetItemForSlot(ItemSlot.Weapon1) != null && GetItemForSlot(ItemSlot.Weapon1).GetItemSlot() != ItemSlot.TwoHanded)
                        {
                            ClearSlot(ItemSlot.Weapon2);
                            EquipWeapon2(item);
                        } else
                        {
                            ClearSlot(ItemSlot.Weapon1);
                            EquipWeapon1(item);
                        }
                    }
                    break;
                case ItemSlot.Weapon2:
                    if(GetItemForSlot(ItemSlot.Weapon1) != null && GetItemForSlot(ItemSlot.Weapon1).GetItemSlot() == ItemSlot.TwoHanded)
                    {
                        ClearSlot(ItemSlot.Weapon1);
                        EquipWeapon2(item);
                    } else
                    {
                        ClearSlot(ItemSlot.Weapon2);
                        EquipWeapon2(item);
                    }
                    break;
            }

        }
        else
        {
            MessageUtils.ErrorMessage("Make some space before equipping.");
        }
    }

    public Item GetItemForSlot(ItemSlot itemSlot)
    {
        switch (itemSlot)
        {
            case ItemSlot.Head:
                return (Item) transform.Find("Head").GetComponent<Slot>().usable;
            case ItemSlot.Neck:
                return (Item)transform.Find("Neck").GetComponent<Slot>().usable;
            case ItemSlot.Torso:
                return (Item)transform.Find("Torso").GetComponent<Slot>().usable;
            case ItemSlot.Legs:
                return (Item)transform.Find("Legs").GetComponent<Slot>().usable;
            case ItemSlot.Belt:
                return (Item)transform.Find("Belt").GetComponent<Slot>().usable;
            case ItemSlot.Hands:
                return (Item)transform.Find("Hands").GetComponent<Slot>().usable;
            case ItemSlot.Ring:
                return (Item)transform.Find("Ring").GetComponent<Slot>().usable;
            case ItemSlot.Feet:
                return (Item)transform.Find("Feet").GetComponent<Slot>().usable;
            case ItemSlot.TwoHanded:
                return (Item)transform.Find("Weapon1").GetComponent<Slot>().usable;
            case ItemSlot.Weapon1:
                return (Item)transform.Find("Weapon1").GetComponent<Slot>().usable;
            case ItemSlot.Weapon2:
                return (Item)transform.Find("Weapon1").GetComponent<Slot>().usable;
        }
        return null;
    }

    public void ClearSlot(ItemSlot itemSlot)
    {
        switch (itemSlot)
        {
            case ItemSlot.Head:
                RemoveItemSlot(transform.Find("Head").gameObject);
                break;
            case ItemSlot.Neck:
                RemoveItemSlot(transform.Find("Neck").gameObject);
                break;
            case ItemSlot.Torso:
                RemoveItemSlot(transform.Find("Torso").gameObject);
                break;
            case ItemSlot.Legs:
                RemoveItemSlot(transform.Find("Legs").gameObject);
                break;
            case ItemSlot.Belt:
                RemoveItemSlot(transform.Find("Belt").gameObject);
                break;
            case ItemSlot.Hands:
                RemoveItemSlot(transform.Find("Hands").gameObject);
                break;
            case ItemSlot.Ring:
                RemoveItemSlot(transform.Find("Ring").gameObject);
                break;
            case ItemSlot.Feet:
                RemoveItemSlot(transform.Find("Feet").gameObject);
                break;
            case ItemSlot.TwoHanded:
                RemoveItemSlot(transform.Find("Weapon1").gameObject);
                RemoveItemSlot(transform.Find("Weapon2").gameObject);
                break;
            case ItemSlot.Weapon1:
                RemoveItemSlot(transform.Find("Weapon1").gameObject);
                break;
            case ItemSlot.Weapon2:
                RemoveItemSlot(transform.Find("Weapon2").gameObject);
                break;
        }
    }

    private void EquipHead(Item item)
    {
        Equip(transform.Find("Head").gameObject, item);
    }

    private void EquipNeck(Item item)
    {
        Equip(transform.Find("Neck").gameObject, item);
    }

    private void EquipTorso(Item item)
    {
        Equip(transform.Find("Torso").gameObject, item);
    }

    private void EquipLegs(Item item)
    {
        Equip(transform.Find("Legs").gameObject, item);
    }

    private void EquipBelt(Item item)
    {
        Equip(transform.Find("Belt").gameObject, item);
    }

    private void EquipHands(Item item)
    {
        Equip(transform.Find("Hands").gameObject, item);
    }

    private void EquipRing(Item item)
    {
        Equip(transform.Find("Ring").gameObject, item);
    }

    private void EquipFeet(Item item)
    {
        Equip(transform.Find("Feet").gameObject, item);
    }

    private void EquipWeapon1(Item item, bool isTwoHanded = false)
    {
        Equip(transform.Find("Weapon1").gameObject, item);
    }

    private void EquipWeapon2(Item item)
    {
        Equip(transform.Find("Weapon2").gameObject,item);
    }

    private void Equip(GameObject slot, Item item)
    {
        item.isEquipped = true;
        FindUtils.GetInventoryGrid().RemoveItem(item);
        FindUtils.GetPlayer().GetStats().Add(item.GetStats());
        InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, item.GetImageAsSprite(), item);
    }

    public Stats GetStuffStats()
    {
       Stats stats = new Stats(0, 0, 0, 0, 0, 0, 0, 0);
       foreach(Transform child in transform)
        {
            Item i = (Item)child.gameObject.GetComponent<Slot>().usable;
            if(i != null)
            {
                stats.Add(i.GetStats());
            }
        }

        return stats;
    }

    private void clearChilds(Transform t)
    {
        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }

    private GameObject getSlotWithItem(Item item)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slot.childCount > 0 && slot.GetChild(0).GetComponent<Draggable>().usable == item)
            {
                return slot.gameObject;
            }
        }
        return null;
    }


    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
}

