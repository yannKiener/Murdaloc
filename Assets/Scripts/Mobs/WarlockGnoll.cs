using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarlockGnoll : MonoBehaviour {

	// Use this for initialization
	void Start () {

		Hostile mob = gameObject.AddComponent<Hostile>();
        Dictionary<string, int> lootTable = new Dictionary<string, int>();
        lootTable.Add("Torsoitem", 50);
        lootTable.Add("Headitem", 20);
        lootTable.Add("Legsitem", 20);
        lootTable.Add("Beltitem", 20);
        lootTable.Add("Handsitem", 20);
        lootTable.Add("Ringitem", 20);
        lootTable.Add("Feetitem", 20);
        lootTable.Add("Weapon1item", 20);
        mob.Initialize("Warlock Gnoll",2,false, lootTable);
		mob.AddSpell (Spells.Get ("Corruption"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
