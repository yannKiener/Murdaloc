using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherGnoll : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Hostile mob = gameObject.AddComponent<Hostile>();
		mob.Initialize("Basher Gnoll");
		mob.AddSpell (Spells.Get ("slam"));
	}
	
	// Update is called once per frame
	void Update () {

		
	}


}
