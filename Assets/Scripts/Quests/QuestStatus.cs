using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

[Serializable]
public static class QuestStatus{

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
	
	public static void Save(){
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create (Application.persistentDataPath + Constants.QuestStatusFile);
		bf.Serialize(file, status);
		file.Close ();
	}
	
	public static void Load(){
		if (File.Exists(Application.persistentDataPath + Constants.QuestStatusFile)) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open (Application.persistentDataPath + Constants.QuestStatusFile, FileMode.Open);
			Dictionary<string, bool> statusSave = (Dictionary<string, bool>)bf.Deserialize (file);
			status = statusSave;
			file.Close();
		}
	}
}
