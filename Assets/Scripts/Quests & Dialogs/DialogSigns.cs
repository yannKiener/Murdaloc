﻿using System.Collections;
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
            RemoveMarks(friendly);

            Friendly friend = friendly.GetComponent<Friendly>();
            int markInt = hasDialogMarkQuest(friend.GetDialog());
            if (markInt == 2)
            {
                GameObject go = Resources.Load<GameObject>("Prefab/UI/QuestionMark");
                GameObject.Instantiate(go, friendly.transform);
            }
            else if (markInt == 1)
            {
                GameObject go = Resources.Load<GameObject>("Prefab/UI/ExclamationMark");
                GameObject.Instantiate(go, friendly.transform);
            }
        }
    }

    private static void RemoveMarks(GameObject friendlyGameObject)
    {
        Transform exclamationGameObject = friendlyGameObject.transform.Find("ExclamationMark(Clone)");
        Transform questionGameObject = friendlyGameObject.transform.Find("QuestionMark(Clone)");

        if(exclamationGameObject != null)
        {
            GameObject.DestroyImmediate(exclamationGameObject.gameObject);
        }

        if (questionGameObject != null)
        {
            GameObject.DestroyImmediate(questionGameObject.gameObject);
        }

    }

    /*
     * 0 = Aucune marque
     * 1 = Starting quest, point d'exclamation (!) 
     * 2 = Ending quest, point d'interrogation (?)
     */
    private static int hasDialogMarkQuest(Dialog dialog)
    {
        if (dialog.GetEndQuest() != null)
        {
            return 2;
        }

        if (dialog.GetStartQuest() != null)
        {
            return 1;
        }

        if (dialog.GetChoices() != null)
        {
            foreach (Choice choice in dialog.GetChoices())
            {
                if (choice.GetCondition())
                {
                    int result = hasDialogMarkQuest(choice.GetDialog());
                    if(result != 0)
                    {
                        return result;
                    }
                }
            }

        }
        return 0;

    }
}


