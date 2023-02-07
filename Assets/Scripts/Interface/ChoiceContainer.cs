using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    Choice choice;
    Color defaultColor;
    public Color colorMouseOver;
	// Use this for initialization
	void Start () {
        this.defaultColor = GetComponent<Image>().color;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Initialize(Choice choice)
    {
        this.choice = choice;
        transform.Find("Text").GetComponent<Text>().text = choice.GetText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = colorMouseOver;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        FindUtils.GetDialogPanelComponent().Initialize(choice.GetDialog());
    }
}
