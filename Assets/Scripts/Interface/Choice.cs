using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice {
    private string choiceText;
    private Dialog dialog;
    private string condition;
    private bool inverseCondition = false;

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

    public void SetCondition(string condition)
    {
        if(condition != null)
        {
            if (condition.StartsWith("!") && condition.Length > 2)
            {
                inverseCondition = true;
                this.condition = condition.Substring(1);
            }else
            {
                this.condition = condition;
            }
        }
    }

    
    public bool GetCondition()
    {
        if (inverseCondition)
        {
            return (!DialogStatus.GetStatus(condition));
        } else
        {
            return (DialogStatus.GetStatus(condition));
        }
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
