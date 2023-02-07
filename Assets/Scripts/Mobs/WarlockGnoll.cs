using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockGnoll : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Hostile mob = gameObject.AddComponent<Hostile>();
        Dictionary<string, int> lootTable = new Dictionary<string, int>();
        lootTable.Add("Ringitem", 80);
        mob.Initialize("Warlock Gnoll",2,false, lootTable);
		mob.AddSpell (Spells.Get ("Corruption"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
