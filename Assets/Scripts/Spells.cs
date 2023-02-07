using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spells : MonoBehaviour {
	
	private Dictionary<string,Spell> spellList = new Dictionary<string, Spell>();

	// Use this for initialization
	void Start () {
		HostileSpell FIREBALL = new HostileSpell ("fireball","A magic Fireball. That's it.", 10,2,0,0);
		FriendlySpell splash = new FriendlySpell ();
		spellList.Add (FIREBALL.name, FIREBALL);
		spellList.Add (splash.name, splash);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public Spell GetSpell(string spellName){
		return spellList [spellName];
	}
}
