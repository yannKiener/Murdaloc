using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Resource 
{
	int Regen(float timeElapsed, bool hasCasted);
	string GetName();

}


public abstract class AbstractResource : Resource {
	protected float localTime = 0;

	public string GetName() {	
		return(this.ToString());
	}

	public virtual int Regen(float timeElapsed, bool hasCasted){
		return 1;	
	}
}

[System.Serializable]
public class Mana : AbstractResource {

	public override int Regen(float timeElapsed, bool hasCasted){
		if (hasCasted) {
			localTime = Time.time;
		} else if(Time.time >= localTime + 5f){ //regen après 5s.
			return 1;
		}


		return 0;
	}
}
		

[System.Serializable]
public class Energy : AbstractResource {

	public override int Regen(float timeElapsed, bool hasCasted){
		localTime += timeElapsed;

		if (localTime >= 2f) {
			localTime -= 2f;
			return 20;
		}
		return 0;
	}
}


[System.Serializable]
public class Rage : AbstractResource {

	public override int Regen(float timeElapsed, bool hasCasted){
		localTime += timeElapsed;

		if (localTime >= 1f) {
			localTime -= 1f;
			return 1;
		}
		return 0;
	}
}
