using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour, Slotable {

	public GameObject usablePrefab;
    
    private bool doUpdate = false;
	static int childCount;

	// Use this for initialization
	void Start () {
		childCount = transform.childCount;
        UpdateSpellBook();
	}

    //On copie le composant pour qu'il reste dans le "SpellBook"
    public void OnDragFrom(GameObject slot)
    {
        InterfaceUtils.CreateUsableSlot(usablePrefab, slot.transform, Draggable.currentItem.GetComponent<Image>().sprite, Draggable.currentUsable);
    }

    public void OnDropIn(GameObject slot)
    {
        
    }

    // Update is called once per frame
    void Update () {
		
	}

    void OnDisable()
    {
        doUpdate = true;
    }

    void OnEnable()
    {
        if (doUpdate)
        {
            UpdateSpellBook();
            doUpdate = false;
        }
    }

	public void UpdateSpellBook(){
        Dictionary<string, Spell> spellList = FindUtils.GetPlayer().GetSpells();

        Spell[] spells = new Spell[childCount];
		spellList.Values.CopyTo (spells, 0);
        UpdateSpellBookWith(spells);
    }

    private void UpdateSpellBookWith(Spell[] slots)
    {
        for (int i = 0; i < childCount; i++)
        {
            Transform slot = transform.GetChild(i);
            if (slots[i] != null)
            {
                if (slot.childCount > 0)
                {
                    clearChilds(slot);
                }

                InterfaceUtils.CreateUsableSlot(usablePrefab, slot, InterfaceUtils.LoadSpriteForSpell(slots[i].GetName()), slots[i]);
            }
        }
    }

    private void clearChilds(Transform t){
		foreach (Transform c in t) {
			GameObject.Destroy (c.gameObject);
		}
	}
}