using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest{

	string name;
	bool isTracked = false;
	
	public Quest(string name){
		this.name = name;
	}
	
	public void Start(){
        Debug.Log("quest started : " + name);
        QuestStatus.SetStatus(name + "Started", true);
	}
	
	public void SetReady(){
        QuestStatus.SetStatus(name + "Ready", true);
		this.isTracked = true;
	}
	
	public void End(){
        QuestStatus.SetStatus(name + "Over", true);
	}

}
