using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

    public GameObject item    {
        get{
            if (transform.childCount > 0)
            {
                return transform.GetChild(0).gameObject;
            }
            return null;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (!item)
        {
            Draggable.currentItem.transform.SetParent(transform);
            Draggable.currentItem.transform.localPosition = new Vector3();
        } else
        {
            GameObject it = item;
            it.transform.SetParent(Draggable.currentItem.transform.parent);
            it.transform.localPosition = new Vector3();

            Draggable.currentItem.transform.SetParent(transform);
            Draggable.currentItem.transform.localPosition = new Vector3();


        }
    }
}
