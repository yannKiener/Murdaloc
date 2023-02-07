using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

interface Slotable {
    void OnDragFrom(GameObject slot);
    void OnDropIn(GameObject slot, PointerEventData eventData);
    void ResetDrag(GameObject slot);
}
