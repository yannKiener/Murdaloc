using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Use this for initialization;
	void Start()
    {
        Player player = gameObject.AddComponent<Player>();
        player.Initialize("Speaf");
		player.AddSpell (Spells.Get ("fireball"));
		player.AddSpell (Spells.Get ("slam"));
		player.AddSpell (Spells.Get ("corruption"));
		player.AddSpell (Spells.Get ("renovation"));
		player.AddSpell (Spells.Get ("sprint"));

		Interface.LoadPlayer ();
	}

	// Update is called once per frame
	void Update () {

	}
}
