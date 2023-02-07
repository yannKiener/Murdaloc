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
                    EquipEquipment(tempEquipment);
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

        if (slot != null && (Equipment)slot.GetComponent<Slot>().usable != null)
        {
            Equipment item = (Equipment)slot.GetComponent<Slot>().usable;
            //On retire l'objet
            clearChilds(slot.transform);
            slot.GetComponent<Slot>().usable = null;
            if (addToInventory)
            {
                item.isEquipped = false;
                FindUtils.GetInventoryGrid().AddItem(item);
            }
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

    public void EquipEquipment(Equipment item)
    {
        if (FindUtils.GetInventoryGrid().HasAtLeastFreeSlots(1))
        {
            switch (item.GetEquipmentSlot())
            {
                case EquipmentSlot.Head:
                    ClearSlot(EquipmentSlot.Head);
                    EquipHead(item);
                    break;
                case EquipmentSlot.Neck:
                    ClearSlot(EquipmentSlot.Neck);
                    EquipNeck(item);
                    break;
                case EquipmentSlot.Torso:
                    ClearSlot(EquipmentSlot.Torso);
                    EquipTorso(item);
                    break;
                case EquipmentSlot.Legs:
                    ClearSlot(EquipmentSlot.Legs);
                    EquipLegs(item);
                    break;
                case EquipmentSlot.Belt:
                    ClearSlot(EquipmentSlot.Belt);
                    EquipBelt(item);
                    break;
                case EquipmentSlot.Hands:
                    ClearSlot(EquipmentSlot.Hands);
                    EquipHands(item);
                    break;
                case EquipmentSlot.Ring:
                    ClearSlot(EquipmentSlot.Ring);
                    EquipRing(item);
                    break;
                case EquipmentSlot.Feet:
                    ClearSlot(EquipmentSlot.Feet);
                    EquipFeet(item);
                    break;
                case EquipmentSlot.TwoHanded:

                    if (FindUtils.GetInventoryGrid().HasAtLeastFreeSlots(1))
                    {
                        ClearSlot(EquipmentSlot.TwoHanded);
                        EquipWeapon1(item);
                    } else
                    {
                        MessageUtils.ErrorMessage("Make some space before equipping.");
                    }
                    break;
                case EquipmentSlot.Weapon1:
                    if (GetEquipmentForSlot(EquipmentSlot.Weapon1) == null)
                    {
                        EquipWeapon1(item);
                    } else
                    {
                        if (GetEquipmentForSlot(EquipmentSlot.Weapon1) != null && GetEquipmentForSlot(EquipmentSlot.Weapon1).GetEquipmentSlot() != EquipmentSlot.TwoHanded)
                        {
                            ClearSlot(EquipmentSlot.Weapon2);
                            EquipWeapon2(item);
                        } else
                        {
                            ClearSlot(EquipmentSlot.Weapon1);
                            EquipWeapon1(item);
                        }
                    }
                    break;
                case EquipmentSlot.Weapon2:
                    if(GetEquipmentForSlot(EquipmentSlot.Weapon1) != null && GetEquipmentForSlot(EquipmentSlot.Weapon1).GetEquipmentSlot() == EquipmentSlot.TwoHanded)
                    {
                        ClearSlot(EquipmentSlot.Weapon1);
                        EquipWeapon2(item);
                    } else
                    {
                        ClearSlot(EquipmentSlot.Weapon2);
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

    public void ClearSlot(EquipmentSlot itemSlot)
    {
        switch (itemSlot)
        {
            case EquipmentSlot.Head:
                RemoveEquipmentSlot(transform.Find("Head").gameObject);
                break;
            case EquipmentSlot.Neck:
                RemoveEquipmentSlot(transform.Find("Neck").gameObject);
                break;
            case EquipmentSlot.Torso:
                RemoveEquipmentSlot(transform.Find("Torso").gameObject);
                break;
            case EquipmentSlot.Legs:
                RemoveEquipmentSlot(transform.Find("Legs").gameObject);
                break;
            case EquipmentSlot.Belt:
                RemoveEquipmentSlot(transform.Find("Belt").gameObject);
                break;
            case EquipmentSlot.Hands:
                RemoveEquipmentSlot(transform.Find("Hands").gameObject);
                break;
            case EquipmentSlot.Ring:
                RemoveEquipmentSlot(transform.Find("Ring").gameObject);
                break;
            case EquipmentSlot.Feet:
                RemoveEquipmentSlot(transform.Find("Feet").gameObject);
                break;
            case EquipmentSlot.TwoHanded:
                FindUtils.GetPlayer().ResetAutoAttacks();
                RemoveEquipmentSlot(transform.Find("Weapon1").gameObject);
                RemoveEquipmentSlot(transform.Find("Weapon2").gameObject);
                break;
            case EquipmentSlot.Weapon1:
                FindUtils.GetPlayer().ResetAutoAttack1();
                RemoveEquipmentSlot(transform.Find("Weapon1").gameObject);
                break;
            case EquipmentSlot.Weapon2:
                FindUtils.GetPlayer().ResetAutoAttack2();
                RemoveEquipmentSlot(transform.Find("Weapon2").gameObject);
                break;
        }
    }

    private void EquipHead(Equipment item)
    {
        Equip(transform.Find("Head").gameObject, item);
    }

    private void EquipNeck(Equipment item)
    {
        Equip(transform.Find("Neck").gameObject, item);
    }

    private void EquipTorso(Equipment item)
    {
        Equip(transform.Find("Torso").gameObject, item);
    }

    private void EquipLegs(Equipment item)
    {
        Equip(transform.Find("Legs").gameObject, item);
    }

    private void EquipBelt(Equipment item)
    {
        Equip(transform.Find("Belt").gameObject, item);
    }

    private void EquipHands(Equipment item)
    {
        Equip(transform.Find("Hands").gameObject, item);
    }

    private void EquipRing(Equipment item)
    {
        Equip(transform.Find("Ring").gameObject, item);
    }

    private void EquipFeet(Equipment item)
    {
        Equip(transform.Find("Feet").gameObject, item);
    }

    private void EquipWeapon1(Equipment item, bool isTwoHanded = false)
    {
        FindUtils.GetPlayer().SetAutoAttack1(item.GetStats().AutoAttackDamage, item.GetStats().AutoAttackTime);
        Equip(transform.Find("Weapon1").gameObject, item);
    }

    private void EquipWeapon2(Equipment item)
    {
        FindUtils.GetPlayer().SetAutoAttack2(item.GetStats().AutoAttackDamage, item.GetStats().AutoAttackTime);
        Equip(transform.Find("Weapon2").gameObject,item);
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

