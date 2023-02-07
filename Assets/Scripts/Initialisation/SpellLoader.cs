using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLoader : MonoBehaviour {

	// Use this for initialization
	void Start () {
        
		HostileSpell FIREBALL = new HostileSpell ("fireball","A magic Fireball. That's it.", 10,2,0,0);
		FriendlySpell splash = new FriendlySpell ();
		Spells.Add(FIREBALL);
        Spells.Add(splash);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
