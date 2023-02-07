using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject currentItem;
    public static Usable currentUsable;
    public static Vector3 originalPosition;
    public static Transform originalParent;
    public Usable usable;


    public void OnBeginDrag (PointerEventData eventData){
        originalPosition = transform.position;
        originalParent = transform.parent;
        currentItem = gameObject;
        currentUsable = usable;
        transform.GetComponentInParent<Slot>().OnDragFrom();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

	public void OnDrag (PointerEventData eventData){
        currentItem.transform.position = Input.mousePosition;
	}

    public void ResetDrag()
    {
        transform.GetComponentInParent<Slot>().ResetDrag();
    }

	public void OnEndDrag (PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //We use that to see if the user is trying to delete => Drop out of slot
        if (eventData.used)
        {
            ResetDrag();
        } else
        {
            Destroy(currentItem);
        }

        currentItem = null;
    }
}
