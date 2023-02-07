using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

    Color defaultColor;
    public Color colorMouseOver;

    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.GetComponent<QuestGrid>().ShowQuestDetails(GetComponentInChildren<Text>().text);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Image>().color = colorMouseOver;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<Image>().color = defaultColor;
    }

    // Use this for initialization
    void Start () {

        this.defaultColor = GetComponent<Image>().color;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
