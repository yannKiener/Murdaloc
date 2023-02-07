using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectLoader : MonoBehaviour {


	void Awake (){

		EffectOnTime corruption = new EffectOnTime ("corruption","First DoT of the game",false,6,0.5f,30,0);
		EffectOnTime renovation = new EffectOnTime ("renovation","First HoT of the game",true,10,1,0,50);

		EffectsOnTime.Add (corruption);
		EffectsOnTime.Add (renovation);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
