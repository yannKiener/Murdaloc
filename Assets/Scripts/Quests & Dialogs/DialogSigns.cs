using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class DialogSigns 
{

    public static void UpdateSigns()
    {
        List<GameObject> friendliesInArea = GameObject.FindGameObjectsWithTag("Friendly").ToList();
        foreach(GameObject friendly in friendliesInArea)
        {
            friendly.GetComponent<Friendly>().GetDialog();
        }
    }


    private static bool hasDialogEndingQuest(Dialog dialog)
    {
        if (dialog.GetChoices() != null && dialog.GetChoices().Count > 0)
        {
            return false;
        } else
        {
            if(dialog.GetEndQuest() != null))
            foreach(Choice choice in dialog.GetChoices())
            {
                if (hasDialogEndingQuest(choice.GetDialog())){
                    return true;
                }
            }
        }
    }

    private static bool hasDialogStartingQuest(Dialog dialog)
    {

    }
}


