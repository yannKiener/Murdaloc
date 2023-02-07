using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageGnoll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Hostile mob = gameObject.AddComponent<Hostile>();
        mob.Initialize("Mage Gnoll",2,false);
		mob.AddSpell (Spells.Get ("Fireball"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
