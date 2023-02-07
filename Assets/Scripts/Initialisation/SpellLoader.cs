using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLoader : MonoBehaviour {


	void Awake (){

		HostileSpell FIREBALL = new HostileSpell ("fireball","A magic Fireball. That's it.", 10,2,25,0,0);
		HostileSpell POTTU = new HostileSpell ("pottu","Spawns a Pottu on your target's face", 10,0,10,0,0);
		//FriendlySpell CALMSPIRIT = new FriendlySpell ("calmspirit","Regens mana", 10,0,10,0,0);
		FriendlySpell splash = new FriendlySpell ();
		Spells.Add(FIREBALL);
		Spells.Add(splash);
		Spells.Add (POTTU);
	}

	// Use this for initialization
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
