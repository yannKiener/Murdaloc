using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAndEffectLoader : MonoBehaviour {


	void Awake (){
		EffectsOnTime.Add (new EffectOnTime ("corruption","First DoT of the game",false,2,6,0.5f,100,0));
		EffectsOnTime.Add (new EffectOnTime ("renovation","First HoT of the game",true,4,10,1,0,80));

		Spells.Add (new HostileSpell ("autoattack","An automatic attack", 0,0,2,0,0));
		Spells.Add (new HostileSpell ("fireball","A magic Fireball. That's it.", 10,2,25,0,0));
		Spells.Add (new HostileSpell ("pottu","Spawns a Pottu on your target's face", 10,0,10,0,0));
		Spells.Add (new FriendlySpell ("renovation","FIRST HOTSPELL", 5,0.5f,0,0,new List<EffectOnTime>(),new List<EffectOnTime>{ EffectsOnTime.Get("renovation") }));
		Spells.Add (new HostileSpell ("corruption","FIRST DOT SPELL", 5,0.5f,0,0,0,new List<EffectOnTime> { EffectsOnTime.Get("corruption") },new List<EffectOnTime>()));
	}

	// Use this for initialization
	void Start () {
        
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
