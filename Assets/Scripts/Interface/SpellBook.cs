﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour, Slotable {

	public GameObject usablePrefab;
    
    private bool doUpdate = false;
	static int childCount = 40;

    // Use this for initialization
    void Start () {
        UpdateSpellBook();
	}

    //On copie le composant pour qu'il reste dans le "SpellBook"
    public void OnDragFrom(GameObject slot)
    {
        InterfaceUtils.CreateUsableSlot(usablePrefab, slot.transform, Draggable.currentItem.GetComponent<Image>().sprite, Draggable.currentUsable);
    }

    public void OnDropIn(GameObject slot, PointerEventData eventData)
    {
        eventData.Use();
    }

    void OnDisable()
    {
        Interface.CloseSpellbook();
        doUpdate = true;
    }

    void OnEnable()
    {

        if (FindUtils.GetTalentSheet().activeSelf)
        {
            InterfaceUtils.ShowHideTalentSheet();
        }
        Interface.OpenSpellbook();
        if (doUpdate)
        {
            UpdateSpellBook();
            doUpdate = false;
        }
    }

	public void UpdateSpellBook(){
        Dictionary<string, Spell> spellList = FindUtils.GetPlayer().GetSpells();

        Spell[] spells = new Spell[childCount];
		spellList.Values.CopyTo (spells, 0);
        UpdateSpellBookWith(spells);
    }

    private void UpdateSpellBookWith(Spell[] slots)
    {
        for (int i = 0; i < childCount; i++)
        {
            Transform slot = transform.GetChild(i);
            clearChilds(slot);
            slot.GetComponent<Slot>().usable = null;
            if (slots[i] != null)
            {
                InterfaceUtils.CreateUsableSlot(usablePrefab, slot, InterfaceUtils.LoadSpriteForSpell(slots[i].GetName()), slots[i]);
            }
        }
    }

    private void clearChilds(Transform t){
		foreach (Transform c in t) {
			GameObject.Destroy (c.gameObject);
		}
	}

    public void ResetDrag(GameObject slot)
    {
        Destroy(Draggable.currentItem);
    }
}