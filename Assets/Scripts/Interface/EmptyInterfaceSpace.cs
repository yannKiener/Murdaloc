using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmptyInterfaceSpace : MonoBehaviour, IDropHandler{
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Trying to delete ");
        eventData.Use();
    }
}
