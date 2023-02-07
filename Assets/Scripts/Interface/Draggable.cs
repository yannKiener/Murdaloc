using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject currentItem;
    public static Usable currentUsable;
    public Usable usable;

    public void OnBeginDrag (PointerEventData eventData){

        currentItem = gameObject;
        currentUsable = usable;
        transform.GetComponentInParent<Slot>().OnDragFrom();
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

	public void OnDrag (PointerEventData eventData){
        transform.position = Input.mousePosition;

	}

	public void OnEndDrag (PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //Checker si le currentItem est un item avant de le delete direct.
        Destroy(currentItem);
        currentItem = null;
    }
}
