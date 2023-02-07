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

        swapActionBar();

        if (!item)
        {
            //L'item en train d'être déplacé prend juste la place du vide.
            Draggable.currentItem.transform.SetParent(transform);
            Draggable.currentItem.transform.localPosition = new Vector3();
        } else
        {
            //L'item actuel du slot prend le parent de celui déplacé, puis l'inverse.
            GameObject it = item;
            it.transform.SetParent(Draggable.currentItem.transform.parent);
            it.transform.localPosition = new Vector3();

            Draggable.currentItem.transform.SetParent(transform);
            Draggable.currentItem.transform.localPosition = new Vector3();
        }



    }

    int getPositionFromParent(Transform t)
    {
        Transform parent = t.parent;
        int slotsCount = parent.childCount;
        for (int i = 0; i < slotsCount; ++i)
        {
            if (parent.GetChild(i) == t)
            {
                return i;
            }
        }
        Debug.Log("Erreur de Swap sur un parent hors ActionBar");
        return 0;
    } 

    void swapActionBar()
    {
        int pos1 = getPositionFromParent(transform);
        int pos2 = getPositionFromParent(Draggable.currentItem.transform.parent);
        GameObject actionBar = GameObject.Find("ActionBar");
        actionBar.GetComponent<ActionBar>().SwapSlots(pos1, pos2);
    }
}
