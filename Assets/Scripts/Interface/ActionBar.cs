using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBar : MonoBehaviour {
	Player player;
	Usable[] slots;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
		slots = new Usable[transform.childCount];
		for(int i = 0,
		Debug.Log (transform.childCount);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static void updateActionBar(){

	}
}
