using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoreTusk : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Hostile mob = gameObject.AddComponent<Hostile>();
        mob.Initialize("Goretusk",1,false);
        mob.AddDialog("test");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
