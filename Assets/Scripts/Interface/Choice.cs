using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice {
    private string choiceText;
    private Dialog dialog;
    private Dictionary<string, bool> conditions = new Dictionary<string, bool>();

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

    public void AddCondition(string condition)
    {
        if(condition != null)
        {
            if (condition.StartsWith("!") && condition.Length > 2)
            {
                this.conditions.Add(condition.Substring(1),true);
            }else
            {
                this.conditions.Add(condition,false);
            }
        }
    }

    
    public bool GetCondition()
    {
        bool result;
        foreach(KeyValuePair<string, bool> kv in conditions)
        {
            if (kv.Value)
            {
                result=!DialogStatus.GetStatus(kv.Key);
            }
            else
            {
                result=DialogStatus.GetStatus(kv.Key);
            }

            if (!result)
            {
                return false;
            }
        }
        return true;
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
