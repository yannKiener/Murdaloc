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
		player.AddSpell (Spells.Get ("POTTU"));
		player.AddSpell (Spells.Get ("corruption"));
		player.AddSpell (Spells.Get ("renovation"));
		this.setSpellShortcut(player);
        print(player.GetName()); 
	}

	void setSpellShortcut (Player player)
	{
		player.SetActionBarSlot(0,"fireball");
		player.SetActionBarSlot(1,"renovation");
		player.SetActionBarSlot(2,"pottu");
		player.SetActionBarSlot(3,"corruption");
	}
	// Update is called once per frame
	void Update () {

	}
}
