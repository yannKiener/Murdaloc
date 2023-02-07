using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageGnoll : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Hostile mob = gameObject.AddComponent<Hostile>();
        Dictionary<string, int> lootTable = new Dictionary<string, int>();
        lootTable.Add("Weapon2item", 50);
        mob.Initialize("Mage Gnoll",2,false, lootTable);
		mob.AddSpell (Spells.Get ("Fireball"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
