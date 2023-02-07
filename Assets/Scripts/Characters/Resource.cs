using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Resource  {
	protected float localTime = 0;

	public string GetName() {	
		return(this.ToString());
	}

	public virtual int Regen(float timeElapsed, bool hasCasted, bool inCombat, Character caster){
		return 1;	
	}
}

[System.Serializable]
public class Mana : Resource {
	float counter = 0;

	public override int Regen(float timeElapsed, bool hasCasted, bool inCombat, Character caster){
		if (hasCasted) {
			localTime = Time.time;
			counter = 0;
		} else if(Time.time >= localTime + Constants.RegenManaAfter){ //regen après x secondes.
			counter += timeElapsed;
			if (counter >= Constants.RegenManaEvery) {
				counter -= Constants.RegenManaEvery;
				return caster.GetStats().GetManaPerSec() * Constants.RegenManaEvery;
			}
			return 0;

		}


		return 0;
	}
}
		

[System.Serializable]
public class Energy : Resource {

	public override int Regen(float timeElapsed, bool hasCasted, bool inCombat, Character caster){
		localTime += timeElapsed;

		if (localTime >= Constants.RegenEnergyEvery) {
			localTime -= Constants.RegenEnergyEvery;
			return 20;
		}
		return 0;
	}
}


[System.Serializable]
public class Rage : Resource {

	public override int Regen(float timeElapsed, bool hasCasted, bool inCombat, Character caster){
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
