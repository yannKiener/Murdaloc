using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour {

	public GameObject slotPrefab;

	Player player;
	static Usable[] slots;
	static int childCount;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player>();
		childCount = transform.childCount;
		slots = new Usable[childCount];
		Debug.Log (player);
		InitializeSpellBookBarWith (player.GetSpells());
	}

	// Update is called once per frame
	void Update () {
		
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
			}
		}
	}

	public void InitializeActionBarWith(Usable[] slotz){
		slots = slotz;
		InitializeActionBar();
	}

	public void InitializeSpellBookBarWith(Dictionary<string, Spell> spellList){
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
