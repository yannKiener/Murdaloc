using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Resource 
{
	int Regen(float timeElapsed, bool hasCasted, bool inCombat);
	string GetName();

}


public abstract class AbstractResource : Resource {
	protected float localTime = 0;

	public string GetName() {	
		return(this.ToString());
	}

	public virtual int Regen(float timeElapsed, bool hasCasted, bool inCombat){
		return 1;	
	}
}

[System.Serializable]
public class Mana : AbstractResource {

	public override int Regen(float timeElapsed, bool hasCasted, bool inCombat){
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

	public override int Regen(float timeElapsed, bool hasCasted, bool inCombat){
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

	public override int Regen(float timeElapsed, bool hasCasted, bool inCombat){
		localTime += timeElapsed;

		if (localTime >= 1f) {
			localTime -= 1f;
			if (inCombat) {
				return 1;
			} else {
				return -1;
			}
		}
		return 0;
	}
}
