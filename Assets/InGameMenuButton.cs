using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InGameMenuButton : MonoBehaviourWithMouseOverColor, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        Interface.OpenCloseMenu();
    }
}
