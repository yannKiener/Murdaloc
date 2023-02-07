using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Hostile mob = gameObject.AddComponent<Hostile>();
        mob.Initialize("Méchant Carré Rouge");
		mob.AddSpell (Spells.Get ("fireball"));
		mob.AddSpell (Spells.Get ("autoattack"));
	}
	
	// Update is called once per frame
	void Update () {

		
	}


}
