using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{

    public Usable usable;
    
    public void Use()
    {
        if(usable != null)
        {
            usable.Use(FindUtils.GetPlayer());
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Use();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (usable != null)
            Interface.DrawToolTip(usable.GetName(), usable.GetDescription());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Interface.RemoveToolTip();
    }

    void OnDisable()
    {
        Interface.RemoveToolTip();
    }


    public void OnDragFrom()
    {
        usable = null;
        transform.GetComponentInParent<Slotable>().OnDragFrom(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Slotable tempSlotable = transform.GetComponentInParent<Slotable>();
        tempSlotable.OnDropIn(gameObject);
        
    }
}
