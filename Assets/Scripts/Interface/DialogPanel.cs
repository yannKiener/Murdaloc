using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{

    public GameObject choiceContainerPrefab;

    // Use this for initialization
    void Start()
    {
        if(choiceContainerPrefab == null)
        {
            choiceContainerPrefab = Resources.Load<GameObject>("Prefab/UI/ChoiceContainer");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2 Initialize(Dialog dialog)
    {
        clearChilds(transform.Find("ChoicesContainer"));
        transform.Find("Text").GetComponent<Text>().text = dialog.GetText();
        Transform choicesContainer = transform.Find("ChoicesContainer");

        foreach(Choice choice in dialog.GetChoices())
        {
            if (choice.GetCondition())
            {
                ChoiceContainer choiceContainer =  Instantiate(choiceContainerPrefab, choicesContainer).GetComponent<ChoiceContainer>();
                choiceContainer.Initialize(choice);
            }
        }

        return new Vector2();
    }



    private void clearChilds(Transform t)
    {
        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
}