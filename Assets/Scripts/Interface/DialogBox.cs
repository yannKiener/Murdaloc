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
        if (FindUtils.GetVendorPanelGameObject().activeSelf)
            FindUtils.GetVendorPanelGameObject().SetActive(false);

        this.friendly = character;
        gameObject.SetActive(true);
        characterName = character.GetName();
        Vector3 characterPosition = character.gameObject.transform.position;
        transform.Find("DialogPanel").GetComponent<DialogPanel>().Initialize(character.GetDialog());
        transform.Find("Name").GetComponent<Text>().text = characterName;

    }
}
