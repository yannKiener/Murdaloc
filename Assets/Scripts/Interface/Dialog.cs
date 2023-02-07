using System.Collections.Generic;

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

    public void SetStartQuest(string startQuest)
    {
        this.startQuest = startQuest;
    }

    public void SetEndQuest(string endQuest)
    {
        this.endQuest = endQuest;
    }

    /*
    public string ShowTree()
    {
        string result = "";
        result += text + startQuest + endQuest +  action+ "\r\n";

        if(choices != null && choices.Count != 0) { 
            foreach(Choice c in choices)
            {
                result += c.choiceText + " => " + c.dialog.ShowTree() + "\r\n";
            }
        }

        return result;
    }*/

}
