using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyMurloc : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        Friendly mob = gameObject.AddComponent<Friendly>();
        mob.Initialize("Friendly Murloc", 1, false);
        mob.AddDialog("test");

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
