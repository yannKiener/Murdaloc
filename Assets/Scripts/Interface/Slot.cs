using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IDropHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler{

    public Usable usable;

    void Start()
    {
        GetComponent<Image>().color = new Color(1,1,1,0.6f);

    }
    
    public void Use()
    {
        if(usable != null)
        {
            usable.Use(FindUtils.GetPlayer());
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(FindUtils.GetVendorBox().activeSelf )
        {
            if (usable is Item)
            {
                Item item = (Item)usable;
                FindUtils.GetInventoryGrid().SellItem(item);
                FindUtils.GetVendorPanel().GetVendor().AddItemToSellTable(item, false);

            }
        } else
        {
            Use();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (usable != null)
            if(usable is Item)
            {
                Interface.DrawToolTip(usable.GetName(), usable.GetDescription(), ((Item)usable).GetSellPrice());
            }
            else
            {
                Interface.DrawToolTip(usable.GetName(), usable.GetDescription());
            }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Interface.RemoveToolTip();
    }

    void OnDisable()
    {
        Interface.RemoveToolTip();
    }

    public void ResetDrag()
    {
        transform.GetComponentInParent<Slotable>().ResetDrag(gameObject);
    }


    public void OnDragFrom()
    {
        usable = null;
        transform.GetComponentInParent<Slotable>().OnDragFrom(gameObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Slotable tempSlotable = transform.GetComponentInParent<Slotable>();
        tempSlotable.OnDropIn(gameObject, eventData);
    }
}
