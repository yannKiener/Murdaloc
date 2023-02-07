using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


	// Use this for initialization
    void Start()
    {
        Player player = gameObject.AddComponent<Player>();
        player.Initialize("Speaf");
        player.SetActionBarSlot(0,"FIREBALL");
        player.SetActionBarSlot(1,"splash");

        print(player.GetName()); 
	}
	
	// Update is called once per frame
	void Update () {

	}


}
