using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavesGrid : MonoBehaviour {

    public GameObject textComponent;

    public void AddSave(string saveName, int level)
    {
        GameObject container = Instantiate(textComponent, transform);
        container.transform.Find("SaveName").GetComponent<Text>().text = saveName;
        container.transform.Find("Level").GetComponent<Text>().text = "Level " + level;
        container.GetComponentInChildren<SavesButton>().saveName = saveName;
    }
}
