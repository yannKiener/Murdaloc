using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogActions {

    private static Dictionary<string, Action> dialogActions = new Dictionary<string, Action>();

    public static void Add(string str, Action action)
    {
        str = str.ToLower();
        dialogActions[str] = action;
    }

    public static void DoAction(string actionString)
    {
        actionString = actionString.ToLower();
        if (dialogActions.ContainsKey(actionString))
        {
            Debug.Log("Doing action " + actionString);
            dialogActions[actionString]();
        } else
        {
            Debug.Log("DIALOG ACTION '" + actionString + "' IS NOT SET !");
        }
    }
}
