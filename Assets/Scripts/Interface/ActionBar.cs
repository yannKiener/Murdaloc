using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBar : MonoBehaviour {

	public GameObject slotPrefab;

	Player player;
	static Usable[] slots;
	static int childCount;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		childCount = transform.childCount;
		slots = new Usable[childCount];

		InitializeActionBarWith (player.GetSpells());
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetButtonDown ("ActionBar1")) {
            slots[0].Use(player);
        }
        if (Input.GetButtonDown("ActionBar2"))
        {
            slots[1].Use(player);
        }
        if (Input.GetButtonDown("ActionBar3"))
        {
            slots[2].Use(player);
        }
        if (Input.GetButtonDown("ActionBar4"))
        {
            slots[3].Use(player);
        }
        if (Input.GetButtonDown("ActionBar5"))
        {
            slots[4].Use(player);
        }
        if (Input.GetButtonDown("ActionBar6"))
        {
            slots[5].Use(player);
        }
        if (Input.GetButtonDown("ActionBar7"))
        {
            slots[6].Use(player);
        }
        if (Input.GetButtonDown("ActionBar8"))
        {
            slots[7].Use(player);
        }
        if (Input.GetButtonDown("ActionBar9"))
        {
            slots[8].Use(player);
        }
        if (Input.GetButtonDown("ActionBar10"))
        {
            slots[9].Use(player);
        }

    }

    public void SwapSlots(int pos1, int pos2)
    {
        Usable temp = slots[pos1];
        slots[pos1] = slots[pos2];
        slots[pos2] = temp;

    }

	public void InitializeActionBar(){
		for(int i = 0;i < childCount; i++){
			Transform slot = transform.GetChild (i);
			if (slots [i] != null) {
				if (slot.childCount > 0) {
					clearChilds (slot);
				}

				GameObject usableSlot = Instantiate (slotPrefab, slot);
				Image image = usableSlot.GetComponent<Image> ();
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

    public void InitializeActionBarWith(Usable[] slotz){
		slots = slotz;
        InitializeActionBar();
	}

	public void InitializeActionBarWith(Dictionary<string, Spell> spellList){
		Spell[] spells = new Spell[childCount];
		spellList.Values.CopyTo (spells, 0);
        InitializeActionBarWith(spells);
	}

	private void clearChilds(Transform t){

		foreach (Transform c in t) {
			GameObject.Destroy (c.gameObject);
		}
	}
}
