using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherGnoll : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Hostile mob = gameObject.AddComponent<Hostile>();
        Dictionary<string, object> lootTable = new Dictionary<string, object>();
        lootTable.Add("Random", 100);
        mob.Initialize("Basher Gnoll",5,false, lootTable);
		mob.AddSpell (Spells.Get ("Slam"));
	}
	
	// Update is called once per frame
	void Update () {

		
	}


}
