using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockGnoll : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Hostile mob = gameObject.AddComponent<Hostile>();
		mob.Initialize("Warlock Gnoll");
		mob.AddSpell (Spells.Get ("Corruption"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
