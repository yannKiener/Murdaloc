using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest{

	string name;
	bool isReady = false;
	
	public Quest(string name){
		this.name = name;
	}
	
	public void Start(){
        QuestStatus.SetStatus(name + "Started", true);
	}
	
	public void SetReady(){
        QuestStatus.SetStatus(name + "Ready", true);
		this.isReady = true;
	}
	
	public void End(){
        QuestStatus.SetStatus(name + "Over", true);
	}

}
