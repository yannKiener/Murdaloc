using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Use this for initialization;
	void Start()
    {
        Player player = gameObject.AddComponent<Player>();
        player.Initialize("Speaf");
		player.AddSpell (Spells.Get ("Fireball"));
		player.AddSpell (Spells.Get ("Slam"));
		player.AddSpell (Spells.Get ("Corruption"));
		player.AddSpell (Spells.Get ("Renovation"));
		player.AddSpell (Spells.Get ("Sprint"));

		Interface.LoadPlayer ();
	}

	// Update is called once per frame
	void Update () {

	}
}
