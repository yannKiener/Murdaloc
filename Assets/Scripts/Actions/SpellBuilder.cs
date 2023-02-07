using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellBuilder  {
	private Spell spell;

	public Spell newHostileSpell(){
		return new HostileSpell ();
	}

	public Spell newFriendlySpell(string name){
		return new FriendlySpell ();
	}

}
