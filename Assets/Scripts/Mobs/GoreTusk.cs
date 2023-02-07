using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreTusk : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Hostile mob = gameObject.AddComponent<Hostile>();
        Dictionary<string, int> lootTable = new Dictionary<string, int>();
        lootTable.Add("Neckitem", 60);
        lootTable.Add("Torsoitem", 40);
		mob.Initialize("Goretusk",1,false,lootTable);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
