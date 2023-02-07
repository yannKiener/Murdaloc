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

    private GameObject getFirstFreeSlot()
    {
        foreach (Transform c in transform)
        {
            if (c.childCount == 0)
                return c.gameObject;
        }
        return null;
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

            //Add link of consumable
        } else 
        {
            eventData.Use();
        }
    }

    // Update is called once per frame
    void Update () {

		if (Input.GetButtonDown ("ActionBar1"))
        {
            transform.GetChild(0).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar2"))
        {
            transform.GetChild(1).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar3"))
        {
            transform.GetChild(2).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar4"))
        {
            transform.GetChild(3).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar5"))
        {
            transform.GetChild(4).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar6"))
        {
            transform.GetChild(5).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar7"))
        {
            transform.GetChild(6).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar8"))
        {
            transform.GetChild(7).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar9"))
        {
            transform.GetChild(8).GetComponent<Slot>().Use();
        }
        if (Input.GetButtonDown("ActionBar10"))
        {
            transform.GetChild(9).GetComponent<Slot>().Use();
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
