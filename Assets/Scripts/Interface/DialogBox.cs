using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogBox : MonoBehaviour {

    string characterName = "" ;
    Friendly friendly;

    public Friendly GetDialogOwner()
    {
        return friendly;
    }

    public void Initialize(Friendly character)
    {
        if (FindUtils.GetVendorBox().activeSelf)
            FindUtils.GetVendorBox().SetActive(false);

        this.friendly = character;
        gameObject.SetActive(true);
        characterName = character.GetName();
        transform.Find("DialogPanel").GetComponent<DialogPanel>().Initialize(character.GetDialog());
        transform.Find("Name").GetComponent<Text>().text = characterName;

    }

}
