﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ChoiceContainer : MonoBehaviourWithMouseOverColor, IPointerClickHandler {

    Choice choice;

    // Use this for initialization
    new void Start () {
        base.Start();
        transform.parent.position += new Vector3(0, Screen.height / 20, 0);
	}

    public void Initialize(Choice choice)
    {
        this.choice = choice;
        transform.Find("Text").GetComponent<Text>().text = choice.GetText();
    }

    public void OnPointerClick(PointerEventData eventData)
    {

        if(choice.GetDialog().GetStartQuest() != null)
        {
            Quests.StartQuest(choice.GetDialog().GetStartQuest());
        }

        if (choice.GetDialog().GetEndQuest() != null)
        {
            Quests.EndQuest(choice.GetDialog().GetEndQuest());
        }

        if (choice.GetDialog().GetAction() != null)
        {
            DialogActions.DoAction(choice.GetDialog().GetAction());
        } 

        if(choice.GetDialog() != null && choice.GetDialog().GetChoices() != null)
        {
            FindUtils.GetDialogPanelComponent().Initialize(choice.GetDialog());
        } else
        {
            FindUtils.GetDialogBox().SetActive(false);
        }
        
    }
}
