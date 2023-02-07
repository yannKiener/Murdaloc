using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour, Slotable {

	public GameObject slotPrefab;
        

    public void OnDragFrom(GameObject slot)
    {

    }

    public List<string> GetSave()
    {
        List<string> shortCuts = new List<string>();

        foreach (Transform c in transform)
        {
            Slot s = c.GetComponent<Slot>();
            if (c.childCount != 0 && s != null && s.usable != null)
            {
                shortCuts.Add(s.usable.GetName());
            }
        }

        return shortCuts;
    }


    public void Load(List<string> shortCuts) {

        foreach(string spellName in shortCuts)
        {
            Dictionary<string, Spell> playerSpells = FindUtils.GetPlayer().GetSpells();
            if (playerSpells.ContainsKey(spellName))
            {
                Add(playerSpells[spellName]);
            } else
            {
                Add(Spells.Get(spellName));
            }
        }
    }

    private GameObject getFirstFreeSlot()
    {
        foreach (Transform c in transform)
        {
            if (c.childCount == 0)
                return c.gameObject;
        }
        return null;
    }

    public void Remove(string usable)
    {
        foreach (Transform t in transform)
        {
            if (t.GetComponent<Slot>().usable != null && t.GetComponent<Slot>().usable.GetName().Equals(usable))
            {
                t.GetComponent<Slot>().usable = null;
                t.GetComponentInChildren<Draggable>().usable = null;
                clearChilds(t);
            }
        }
    }

    public void Add(Usable usable)
    {
        GameObject slot = getFirstFreeSlot();

        if (slot != null)
        {
            InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, usable.GetImageAsSprite(), usable);
        }
    }

    public void OnDropIn(GameObject slot, PointerEventData eventData)
    {
        if(Draggable.currentItem != null && !(Draggable.currentUsable is Item))
        {
            GameObject tempGameObject = Draggable.currentItem;
            Usable tempUsable = Draggable.currentUsable;
            //Si le slot a déjà un contenu, on le supprime 
            if (slot.transform.childCount > 0)
            {
                clearChilds(slot.transform);
            }
            InterfaceUtils.CreateUsableSlot(slotPrefab, slot.transform, tempGameObject.GetComponent<Image>().sprite, tempUsable);
        }else if(Draggable.currentUsable is Consumable)
        {

            eventData.Use();
            //Add link of consumable ou le spell du consumable!
        } else 
        {
            eventData.Use();
        }
    }

    // Update is called once per frame
    void Update () {

		if (InputManager.IsButtonDown("ActionBar1"))
        {
            transform.GetChild(0).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar2"))
        {
            transform.GetChild(1).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar3"))
        {
            transform.GetChild(2).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar4"))
        {
            transform.GetChild(3).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar5"))
        {
            transform.GetChild(4).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar6"))
        {
            transform.GetChild(5).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar7"))
        {
            transform.GetChild(6).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar8"))
        {
            transform.GetChild(7).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar9"))
        {
            transform.GetChild(8).GetComponent<Slot>().Use();
        }
        if (InputManager.IsButtonDown("ActionBar10"))
        {
            transform.GetChild(9).GetComponent<Slot>().Use();
        }

        CheckSlotsConditions();
    }

    private void CheckSlotsConditions()
    {
        foreach(Transform t in transform)
        {
            if(t.childCount > 0)
            {
                HideSlotIfCantCast(t.GetChild(0).gameObject);
            }
        }
    }

    private void HideSlotIfCantCast(GameObject slotGameObject)
    {
        Player p = FindUtils.GetPlayer();
        Spell spell = (Spell)slotGameObject.GetComponent<Draggable>().usable;
        if(spell != null)
        {
            if (spell.IsCastable(p, p.GetTarget(), false))
            {
                slotGameObject.GetComponent<Image>().color = Color.white;
            }
            else
            {
                slotGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
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
        slot.GetComponent<Slot>().usable = Draggable.currentUsable;
        Draggable.currentItem.transform.position = Draggable.originalPosition;

    }
}
