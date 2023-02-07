using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Slotable {
    void OnDragFrom(GameObject slot);
    void OnDropIn(GameObject slot);
}
