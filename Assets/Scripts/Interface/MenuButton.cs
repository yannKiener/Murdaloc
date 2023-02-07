using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

    public GameObject ShowHideGameobject;
    public Color ColorMouseOver;

    Color defaultColor;

    void Start()
    {
        this.defaultColor = GetComponent<Image>().color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(ShowHideGameobject != null)
        {
            ShowHideGameobject.SetActive(!ShowHideGameobject.activeSelf);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = ColorMouseOver;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
    }
}
