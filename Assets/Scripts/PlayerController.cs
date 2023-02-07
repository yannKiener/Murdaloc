using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Use this for initialization;
    void Start()
    {
        Player player = gameObject.AddComponent<Player>();
        player.Initialize("Speaf");
		player.addSpell (Spells.Get ("fireball"));
		player.addSpell (Spells.Get ("splash"));
		player.addSpell (Spells.Get ("POTTU"));
		player.SetActionBarSlot(0,"fireball");
		player.SetActionBarSlot(1,"splash");
		player.SetActionBarSlot(2,"pottu");
        print(player.GetName()); 

	}
	
	// Update is called once per frame
	void Update () {

	}
}
