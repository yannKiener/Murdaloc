using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Use this for initialization;
	void Start()
    {
        Player player = gameObject.AddComponent<Player>();
        player.Initialize("Speaf");
		player.initializeSpell (Spells.Get ("Fireball"));
		player.initializeSpell(Spells.Get ("Slam"));
		player.initializeSpell(Spells.Get ("Corruption"));
		player.initializeSpell(Spells.Get ("Renovation"));
		player.initializeSpell(Spells.Get ("Sprint"));

		Interface.LoadPlayer ();
	}

	// Update is called once per frame
	void Update () {

	}
}
