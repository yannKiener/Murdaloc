using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviourWithMouseOverColor, IPointerClickHandler {

    public GameObject ShowHideGameobject;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if(ShowHideGameobject != null)
        {
            ShowHideGameobject.SetActive(!ShowHideGameobject.activeSelf);
        }
    }
}
