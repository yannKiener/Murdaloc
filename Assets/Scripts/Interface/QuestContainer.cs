using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestContainer : MonoBehaviourWithMouseOverColor, IPointerClickHandler {
    
    public void OnPointerClick(PointerEventData eventData)
    {
        transform.parent.GetComponent<QuestGrid>().ShowQuestDetails(GetComponentInChildren<Text>().text);
    }
    
	// Update is called once per frame
	void Update () {
		
	}
}
