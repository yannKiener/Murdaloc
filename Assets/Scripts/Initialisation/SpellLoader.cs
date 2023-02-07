using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellLoader : MonoBehaviour {


	void Awake (){
		Spells.Add(new HostileSpell ("fireball","A magic Fireball. That's it.", 10,2,25,0,0));
		Spells.Add(new FriendlySpell ());
		Spells.Add (new HostileSpell ("pottu","Spawns a Pottu on your target's face", 10,0,10,0,0));

		Spells.Add (new FriendlySpell ("renovation","FIRST HOTSPELL", 5,0.5f,0,0,new List<EffectOnTime>(),new List<EffectOnTime>{ EffectsOnTime.Get("renovation") }));
		Spells.Add (new HostileSpell ("corruption","FIRST DOT SPELL", 5,0.5f,1,0,0,new List<EffectOnTime> { EffectsOnTime.Get("corruption") },new List<EffectOnTime>()));
	}

	// Use this for initialization
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
