using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBook : MonoBehaviour, Slotable {

	public GameObject usablePrefab;
    
	static int childCount;

	// Use this for initialization
	void Start () {
		childCount = transform.childCount;
		InitializeSpellBookBarWith (FindUtils.GetPlayer().GetSpells());
	}

    public void OnDragFrom(GameObject slot)
    {
        createUsableSlot(slot.transform, Draggable.currentItem.GetComponent<Image>().sprite, Draggable.currentUsable );
    }

    public void OnDropIn(GameObject slot)
    {
        Destroy(Draggable.currentItem);
    }

    // Update is called once per frame
    void Update () {
		
	}

	public void InitializeSpellBookBarWith(Dictionary<string, Spell> spellList){
		Spell[] spells = new Spell[childCount];
		spellList.Values.CopyTo (spells, 0);
        InitializeSpellBookBarWith(spells);
    }

    public void InitializeSpellBookBarWith(Usable[] slots)
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

                createUsableSlot(slot, Interface.LoadSpriteFor(slots[i].GetName()), slots[i]);
            }
        }
    }


    private void clearChilds(Transform t){
		foreach (Transform c in t) {
			GameObject.Destroy (c.gameObject);
		}
	}

    private void createUsableSlot(Transform attachTo, Sprite sprite, Usable usable)
    {

        GameObject usableSlot = Instantiate(usablePrefab, attachTo);
        Image image = usableSlot.GetComponent<Image>();
        image.sprite = sprite;
        usableSlot.GetComponent<Draggable>().usable = usable;
        

    }
}
