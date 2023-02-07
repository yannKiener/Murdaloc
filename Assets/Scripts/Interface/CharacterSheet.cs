using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSheet : MonoBehaviour, Slotable {

    public GameObject slotPrefab;

    void OnEnable()
    {
        Interface.OpenCharsheet();
    }

    void OnDisable()
    {
        Interface.CloseCharsheet();
    }

    public void OnDragFrom(GameObject slot)
    {
        Equipment draggedEquipment = ((Equipment)Draggable.currentUsable);
        FindUtils.GetPlayer().GetStats().Remove(draggedEquipment.GetStats());
    }

    public void OnDropIn(GameObject slot, PointerEventData eventData)
    {
        Usable tempUsable = Draggable.currentUsable;
        if (tempUsable is Equipment)
        {
            Equipment tempEquipment = (Equipment)tempUsable;
            if (tempEquipment.IsUsable())
            {
                if(tempEquipment.isEquipped && tempEquipment.GetEquipmentSlot() == EquipmentSlot.Weapon1)
                {
                    if(getSlotWithEquipment(tempEquipment) == transform.Find("Weapon2").gameObject && slot == transform.Find("Weapon1").gameObject && GetEquipmentForSlot(EquipmentSlot.Weapon1) != null)
                    {
                        SwapWeapon1With(tempEquipment);
                    } else if (getSlotWithEquipment(tempEquipment) == transform.Find("Weapon1").gameObject && slot == transform.Find("Weapon2").gameObject && GetEquipmentForSlot(EquipmentSlot.Weapon2) != null)
                    {
                        SwapWeapon2With(tempEquipment);
                    }
                    else
                    {
                        EquipEquipment(tempEquipment);
                    }
                }
                else
                {
                    if (!EquipEquipment(tempEquipment))
                    {
                        eventData.Use();
                    }
                    
                }
            } else
            {
                MessageUtils.ErrorMessage("Can't equip that yet.");
                eventData.Use();
            }
        }
    }

    public bool RemoveEquipment(Equipment item)
    {
        GameObject slot = getSlotWithEquipment(item);
        return RemoveEquipmentSlot(slot);
    }

    public bool RemoveEquipmentSlot(GameObject slot, bool addToInventory = true)
    {
        if (slot != null)
        {
            if ((Equipment)slot.GetComponent<Slot>().usable == null)
            {
                return true;
            }

            Equipment item = (Equipment)slot.GetComponent<Slot>().usable;
            //On retire l'objet
            if (addToInventory)
            {
                if (!FindUtils.GetInventoryGrid().AddItem(item))
                {
                    return false;
                }   
            }
            clearChilds(slot.transform);
            slot.GetComponent<Slot>().usable = null;
            FindUtils.GetPlayer().GetStats().Remove(item.GetStats());
            if (slot.name.Equals("Weapon1")) 
            {
                FindUtils.GetPlayer().ResetAutoAttack1();
            }
            if (slot.name.Equals("Weapon2"))
            {
                FindUtils.GetPlayer().ResetAutoAttack2();
            }
            return true;
        }
        return false;
    }

    public bool EquipEquipment(Equipment item)
    {
        switch (item.GetEquipmentSlot())
        {
            case EquipmentSlot.Head:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Head");
            case EquipmentSlot.Neck:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Neck");
            case EquipmentSlot.Torso:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Torso");
            case EquipmentSlot.Legs:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Legs");
            case EquipmentSlot.Belt:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Belt");
            case EquipmentSlot.Hands:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Hands");
            case EquipmentSlot.Ring:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Ring");
            case EquipmentSlot.Feet:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Feet");
            case EquipmentSlot.TwoHanded:
                return EquipItemToSlot(item, item.GetEquipmentSlot(), "Weapon1");
            case EquipmentSlot.Weapon1:
                if (GetEquipmentForSlot(EquipmentSlot.Weapon1) == null)
                {
                    return EquipItemToSlot(item, item.GetEquipmentSlot(), "Weapon1");
                } else
                {
                    if (GetEquipmentForSlot(EquipmentSlot.Weapon1) != null && GetEquipmentForSlot(EquipmentSlot.Weapon1).GetEquipmentSlot() != EquipmentSlot.TwoHanded)
                    {
                        return EquipItemToSlot(item, EquipmentSlot.Weapon2, "Weapon2");
                    } else
                    {
                        return EquipItemToSlot(item, item.GetEquipmentSlot(), "Weapon1");
                    }
                }
            case EquipmentSlot.Weapon2:
                if(GetEquipmentForSlot(EquipmentSlot.Weapon1) != null && GetEquipmentForSlot(EquipmentSlot.Weapon1).GetEquipmentSlot() == EquipmentSlot.TwoHanded)
                {
                    if (ClearSlot(EquipmentSlot.Weapon1))
                    {
                        return EquipItemToSlot(item, EquipmentSlot.Weapon2, "Weapon2");
                    } else
                    {
                        return false;
                    }
                } else
                {
                    return EquipItemToSlot(item, EquipmentSlot.Weapon2, "Weapon2");
                }
        }

        return ErrorUnknownEquimentSlot(item.GetEquipmentSlot());
    }

    public Equipment GetEquipmentForSlot(EquipmentSlot itemSlot)
    {
        switch (itemSlot)
        {
            case EquipmentSlot.Head:
                return (Equipment) transform.Find("Head").GetComponent<Slot>().usable;
            case EquipmentSlot.Neck:
                return (Equipment)transform.Find("Neck").GetComponent<Slot>().usable;
            case EquipmentSlot.Torso:
                return (Equipment)transform.Find("Torso").GetComponent<Slot>().usable;
            case EquipmentSlot.Legs:
                return (Equipment)transform.Find("Legs").GetComponent<Slot>().usable;
            case EquipmentSlot.Belt:
                return (Equipment)transform.Find("Belt").GetComponent<Slot>().usable;
            case EquipmentSlot.Hands:
                return (Equipment)transform.Find("Hands").GetComponent<Slot>().usable;
            case EquipmentSlot.Ring:
                return (Equipment)transform.Find("Ring").GetComponent<Slot>().usable;
            case EquipmentSlot.Feet:
                return (Equipment)transform.Find("Feet").GetComponent<Slot>().usable;
            case EquipmentSlot.TwoHanded:
                return (Equipment)transform.Find("Weapon1").GetComponent<Slot>().usable;
            case EquipmentSlot.Weapon1:
                return (Equipment)transform.Find("Weapon1").GetComponent<Slot>().usable;
            case EquipmentSlot.Weapon2:
                return (Equipment)transform.Find("Weapon2").GetComponent<Slot>().usable;
        }
        return null;
    }

    public bool ClearSlot(EquipmentSlot itemSlot)
    {
        switch (itemSlot)
        {
            case EquipmentSlot.Head:
                return RemoveEquipmentSlot(transform.Find("Head").gameObject);
            case EquipmentSlot.Neck:
                return RemoveEquipmentSlot(transform.Find("Neck").gameObject);
            case EquipmentSlot.Torso:
                return RemoveEquipmentSlot(transform.Find("Torso").gameObject);
            case EquipmentSlot.Legs:
                return RemoveEquipmentSlot(transform.Find("Legs").gameObject);
            case EquipmentSlot.Belt:
                return RemoveEquipmentSlot(transform.Find("Belt").gameObject);
            case EquipmentSlot.Hands:
                return RemoveEquipmentSlot(transform.Find("Hands").gameObject);
            case EquipmentSlot.Ring:
                return RemoveEquipmentSlot(transform.Find("Ring").gameObject);
            case EquipmentSlot.Feet:
                return RemoveEquipmentSlot(transform.Find("Feet").gameObject);
            case EquipmentSlot.TwoHanded:
                bool hasRemovedBothWeap = RemoveEquipmentSlot(transform.Find("Weapon1").gameObject) && RemoveEquipmentSlot(transform.Find("Weapon2").gameObject);
                if (hasRemovedBothWeap)
                {
                    FindUtils.GetPlayer().ResetAutoAttacks();
                }
                return hasRemovedBothWeap;
            case EquipmentSlot.Weapon1:
                bool hasRemovedWeap1 = RemoveEquipmentSlot(transform.Find("Weapon1").gameObject);
                if (hasRemovedWeap1)
                {
                    FindUtils.GetPlayer().ResetAutoAttack1();
                }
                return hasRemovedWeap1;
            case EquipmentSlot.Weapon2:
                bool hasRemovedWeap2 = RemoveEquipmentSlot(transform.Find("Weapon2").gameObject);
                if (hasRemovedWeap2)
                {
                    FindUtils.GetPlayer().ResetAutoAttack2();
                }
                return hasRemovedWeap2;
        }
        return ErrorUnknownEquimentSlot(itemSlot);
    }

    private bool ErrorUnknownEquimentSlot(EquipmentSlot equiSlot)
    {
        Debug.LogError("Unknown slot : " + equiSlot);
        return false;
    }

    private bool EquipItemToSlot(Equipment item, EquipmentSlot equipmentSlot, string slotGameObjName)
    {
        bool isSlotCleared = ClearSlot(equipmentSlot);
        if (isSlotCleared)
        {
            Equip(transform.Find(slotGameObjName).gameObject, item);
        }
        return isSlotCleared;
    }

    

    private void Equip(GameObject slot, Equipment item)
    {
        if (!item.isEquipped)
        {
            if (item.isInInventory)
            {
               FindUtils.GetInventoryGrid().RemoveItem(item);
            }
            else
            {
                FindUtils.GetLootGrid().GetComponent<LootInventory>().RemoveItem(item);
            }

            item.isEquipped = true;
        }
        if (slot.name.Equals("Weapon1"))
        {
            FindUtils.GetPlayer().SetAutoAttack1(item.GetStats().AutoAttackDamage, item.GetStats().AutoAttackTime);
        }
        if (slot.name.Equals("Weapon2"))
        {
            FindUtils.GetPlayer().SetAutoAttack2(item.GetStats().AutoAttackDamage, item.GetStats().AutoAttackTime);
        }

        FindUtils.GetPlayer().GetStats().Add(item.GetStats());
        InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, item.GetImageAsSprite(), item);
    }

    private void SwapWeapon1With(Equipment weaponToEquip)
    {
        Equipment weap1 = GetEquipmentForSlot(EquipmentSlot.Weapon1);
        RemoveEquipmentSlot(getSlotWithEquipment(weap1), false);
        EquipEquipment(weaponToEquip);
        EquipEquipment(weap1);
    }

    private void SwapWeapon2With(Equipment weaponToEquip)
    {
        Equipment weap2 = GetEquipmentForSlot(EquipmentSlot.Weapon2);
        RemoveEquipmentSlot(getSlotWithEquipment(weap2), false);
        EquipEquipment(weap2);
        EquipEquipment(weaponToEquip);
    }

    public List<Equipment> GetEquipments()
    {
        List<Equipment> stuff = new List<Equipment>();
        foreach (Transform child in transform)
        {
            Equipment eq = (Equipment)child.gameObject.GetComponent<Slot>().usable;
            if (eq != null)
            {
                stuff.Add(eq);
            }
        }
        return stuff;
    }

    public Stats GetStuffStats()
    {
       Stats stats = new Stats(0, 0, 0, 0, 0, 0, 0, 0);
       foreach(Transform child in transform)
        {
            Equipment i = (Equipment)child.gameObject.GetComponent<Slot>().usable;
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

    private GameObject getSlotWithEquipment(Equipment item)
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

    public void ResetDrag(GameObject slot)
    {
        Equipment draggedEquipment = ((Equipment)Draggable.currentUsable);
        draggedEquipment.isEquipped = true;
        FindUtils.GetPlayer().GetStats().Add(draggedEquipment.GetStats());
        slot.GetComponent<Slot>().usable = Draggable.currentUsable;
        Draggable.currentItem.transform.position = Draggable.originalPosition;
    }
}

