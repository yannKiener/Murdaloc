using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasherGnoll : MonoBehaviour {


	// Use this for initialization
	void Start () {
        Hostile mob = gameObject.AddComponent<Hostile>();
        Dictionary<string, int> lootTable = new Dictionary<string, int>();
        lootTable.Add("Weapon1item", 50);
        lootTable.Add("TwoHandeditem", 20);
        mob.Initialize("Basher Gnoll",1,false,lootTable);
		mob.AddSpell (Spells.Get ("Slam"));
	}
	
	// Update is called once per frame
	void Update () {

		
	}


}
