using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour {

    string characterName = "" ;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Initialize(Friendly character)
    {
        gameObject.SetActive(true);
        characterName = character.GetName();
        Vector3 characterPosition = character.gameObject.transform.position;
        Vector2 widthHeight = transform.Find("DialogPanel").GetComponent<DialogPanel>().Initialize(character.GetDialog());
        transform.Find("Name").GetComponent<Text>().text = characterName;

        //TODO : Mettre a la bonne position le gameObject & a la bonne taille
    }
}
