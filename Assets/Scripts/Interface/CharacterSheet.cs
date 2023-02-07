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

    }
    
    public void OnDropIn(GameObject slot)
    {
        GameObject tempGameObject = Draggable.currentItem;
        Usable tempUsable = Draggable.currentUsable;
        if (tempUsable is Item)
        {
            EquipItem((Item)tempUsable);
        }
    }

    public bool RemoveItem(Item item)
    {
        GameObject slot = getSlotWithItem(item);
        if(slot != null)
        {
            item.isEquipped = false;
            clearChilds(slot.transform);
            slot.GetComponent<Slot>().usable = null;
            return true;
        }
        return false;
    }

    public void EquipItem(Item item)
    {
        switch (item.GetItemType())
        {
            case "Head":
                EquipHead(item);
                break;
            case "Neck":
                EquipNeck(item);
                break;
            case "Torso":
                EquipTorso(item);
                break;
            case "Legs":
                EquipLegs(item);
                break;
            case "Belt":
                EquipBelt(item);
                break;
            case "Hands":
                EquipHands(item);
                break;
            case "Ring":
                EquipRing(item);
                break;
            case "Feet":
                EquipFeet(item);
                break;
            case "TwoHanded":
                EquipWeapon1(item);
                //TODO remove weapon 2
                break;
            case "Weapon1":
                EquipWeapon1(item);
                break;
            case "Weapon2":
                EquipWeapon2(item);
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
        //Si le slot a déjà un contenu, on le supprime 
        if (slot.transform.childCount > 0)
        {
            clearChilds(slot.transform);
        }
        InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, item.GetImageAsSprite(), item);
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
}

