using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour {

	public GameObject slotPrefab;

	Player player;
	Usable[] slots;
	int childCount;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		childCount = transform.childCount;
		slots = new Usable[childCount];

		UpdateActionBar (player.GetSpells());
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("")) {
			//TODO
		}
		
	}

	public void UpdateActionBar(){
		for(int i = 0;i < childCount; i++){
			Transform slot = transform.GetChild (i);
			if (slots [i] != null) {
				if (slot.childCount > 0) {
					clearChilds (slot);
				}

				GameObject usableSlot = Instantiate (slotPrefab, slot);
				Image image = usableSlot.GetComponent<Image> ();
				Debug.Log (slots [i].GetName ());
				image.sprite = Interface.LoadSpriteFor (slots [i].GetName ());

				/*
			GameObject child = slot.GetChild (0).gameObject;
			if (child != null) {
				//Add to slots
				Debug.Log ("child : " + child.GetComponent<Image> ().name);
			}*/
			}
		}
	}

	public void UpdateActionBar(Usable[] slots){
		this.slots = slots;
		UpdateActionBar ();
	}

	public void UpdateActionBar(Dictionary<string, Spell> spellList){
		Spell[] spells = new Spell[childCount];
		spellList.Values.CopyTo (spells, 0);
		UpdateActionBar (spells);
	}

	private void clearChilds(Transform t){

		foreach (Transform c in t) {
			GameObject.Destroy (c.gameObject);
		}
	}
}
