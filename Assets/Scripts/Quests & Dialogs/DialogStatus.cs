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
	}

    public static Dictionary<string, bool> GetAllStatus()
    {
        Debug.Log("Get All Status ! ");

        foreach (KeyValuePair<string, bool> kv in status)
        {
            Debug.Log(kv.Key + " " + kv.Value);
        }
        return status;
    }

    public static void ResetAllStatus()
    {
        status = new Dictionary<string, bool>();
    }

    public static void SetAllStatus(Dictionary<string, bool> st)
    {

        Debug.Log("Set All Status ! ");
        foreach (KeyValuePair<string, bool> kv in st)
        {
            Debug.Log(kv.Key + " " + kv.Value);
        }
        status = st;
    }
}
