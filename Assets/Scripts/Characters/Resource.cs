using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Resource 
{
	int Regen();
	string GetName();

}

[System.Serializable]
public class Mana : Resource {

	public string GetName() {
		return("Mana");
	}

	public int Regen(){
		return 1;

	}
}


[System.Serializable]
public class Energy : Resource {
	
	public string GetName() {
		return("Energy");
	}

	public int Regen(){
		return 1;
	}
}


[System.Serializable]
public class Rage : Resource {

	public string GetName() {
		return("Rage");
	}

	public int Regen(){
		return 1;
	}
}
