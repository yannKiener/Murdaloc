using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice {
    private string choiceText;
    private Dialog dialog;
    private bool condition = true;

    public Choice()
    {

    }
    
    public void SetChoiceText(string choiceText)
    {
        if(choiceText != null)
        {
            this.choiceText = choiceText.Replace("%T", FindUtils.GetPlayer().GetName());
        }
    }

    public void SetCondition(bool condition)
    {
        this.condition = condition;
    }

    
    public bool GetCondition()
    {
        return condition;
    }

    public void SetDialog(Dialog dialog)
    {
        this.dialog = dialog;
    }

    public Dialog GetDialog()
    {
        return dialog;
    }

    public string GetText()
    {
        return choiceText;
    }


}
