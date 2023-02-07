using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestBackButton : MonoBehaviourWithMouseOverColor, IPointerClickHandler {
    
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<QuestGrid>().UpdateQuestLog();
    }
}
