using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
	public static GameObject currentItem;
	private Vector3 startPosition;
    Transform startParent;
	
	public void OnBeginDrag (PointerEventData eventData){
		currentItem = gameObject;
		startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;

	}

	public void OnDrag (PointerEventData eventData){
		transform.position = Input.mousePosition;

	}

	public void OnEndDrag (PointerEventData eventData){
		currentItem = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }

	}



	
}
