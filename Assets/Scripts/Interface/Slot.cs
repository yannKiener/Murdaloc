using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public Usable usable;
    
    public void Use()
    {
        if(usable != null)
        {
            usable.Use(FindUtils.GetPlayer());
        }
    }

    public void OnDragFrom()
    {
        usable = null;
        transform.GetComponentInParent<Slotable>().OnDragFrom(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        usable = Draggable.currentUsable;
        transform.GetComponentInParent<Slotable>().OnDropIn(gameObject);
        
    }
}
