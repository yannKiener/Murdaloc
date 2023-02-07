using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
public static class DialogStatus{

	static Dictionary<string, bool> status = new Dictionary<string, bool>();
	
	public static bool GetStatus(string name){

        if(name == null)
        {
            return true;
        }
		if (!status.ContainsKey(name))
        {
			status.Add(name, false);
		}
		
		return status[name];
	}
	
	public static void SetStatus(string name, bool boolean){
		status[name] = boolean;
        DialogSigns.UpdateSigns();
    }

    public static Dictionary<string, bool> GetAllStatus()
    {   
        return status;
    }

    public static void ResetAllStatus()
    {
        status = new Dictionary<string, bool>();
    }

    public static void SetAllStatus(Dictionary<string, bool> st)
    {
        status = st;
    }
}
