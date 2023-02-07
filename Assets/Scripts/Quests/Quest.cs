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
        DialogStatus.SetStatus(name + "Started", true);
	}
	
	public void SetReady(){
        DialogStatus.SetStatus(name + "Ready", true);
		this.isTracked = true;
	}
	
	public void End(){
        DialogStatus.SetStatus(name + "Over", true);
	}

}
