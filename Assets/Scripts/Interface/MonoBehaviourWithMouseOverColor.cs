using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MonoBehaviourWithMouseOverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    Color defaultColor;

    //For GUI only (So Image components)
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = Constants.MouseOverColor;
        
    }

    //For GUI only (So Image components)
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
    }


    //For collider only (So SpriteRenderer components)
    void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = Constants.MouseOverColor;
    }

    //For collider only (So SpriteRenderer components)
    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = defaultColor;
    }

    // Use this for initialization
    public void Start () {
        if(GetComponent<Image>() != null)
        {
            this.defaultColor = GetComponent<Image>().color;
        } else if (GetComponent<SpriteRenderer>() != null)
        {
            this.defaultColor = GetComponent<SpriteRenderer>().color;
        }
    }
	
}
