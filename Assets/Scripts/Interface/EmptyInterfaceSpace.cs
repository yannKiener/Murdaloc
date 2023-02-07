using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyInterfaceSpace : MonoBehaviour, IDropHandler{
    public void OnDrop(PointerEventData eventData)
    {
        if(Draggable.currentUsable != null && Draggable.currentUsable is Item)
        {
            Interface.DrawModalDialog("Are you sure you want to delete this ?", DeleteItem(Draggable.currentItem));
            eventData.Use();
        }
    }

    private Action DeleteItem(GameObject usableGameObject)
    {
        return new Action(() => { if (usableGameObject != null) { usableGameObject.GetComponentInParent<Slot>().usable = null; Destroy(usableGameObject); } });
    }
}
