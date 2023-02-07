using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavesGrid : MonoBehaviour {

    public GameObject textComponent;

    public void AddSave(string saveName)
    {
        GameObject container = Instantiate(textComponent, transform);
        container.GetComponentInChildren<Text>().text = saveName;
        container.GetComponentInChildren<SavesButton>().saveName = saveName;
    }
}
