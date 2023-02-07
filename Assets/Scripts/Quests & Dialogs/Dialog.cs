using System.Collections.Generic;
using UnityEngine;

public class Dialog
{
    private string text;
    private string action;
    private string startQuest;
    private string endQuest;
    private List<Choice> choices;

    public Dialog()
    {

    }

    public string GetText()
    {
        return text;
    }

    public List<Choice> GetChoices()
    {
        return choices;
    }

    public void SetChoices (List<Choice> choices)
    {
        this.choices = choices;
    }

    public void SetText(string text)
    {
        if (text != null)
        {
            this.text = text.Replace("%T", FindUtils.GetPlayer().GetName());
        }
    }

    public void SetAction(string action)
    {
        this.action = action;
    }

    public string GetStartQuest()
    {
        return startQuest;
    }

    public string GetEndQuest()
    {
        return endQuest;
    }

    public string GetAction()
    {
        return action;
    }

    public void SetStartQuest(string startQuest)
    {
        this.startQuest = startQuest;
    }

    public void SetEndQuest(string endQuest)
    {
        this.endQuest = endQuest;
    }


}
