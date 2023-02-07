using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{

    public GameObject choiceContainerPrefab;

    void OnDisable()
    {
        FindUtils.GetPlayer().CancelTarget();
    }

    // Use this for initialization
    void Start()
    {
        if(choiceContainerPrefab == null)
        {
            choiceContainerPrefab = Resources.Load<GameObject>("Prefab/UI/ChoiceContainer");
        }

    }

    public float Initialize(Dialog dialog)
    {
        SoundManager.PlaySound(FindUtils.GetDialogBoxComponent().GetDialogOwner().GetTalkSounds());
        clearChilds(transform.Find("ChoicesContainer"));
        transform.Find("Text").GetComponent<Text>().text = dialog.GetText();
        Transform choicesContainer = transform.Find("ChoicesContainer");
        foreach (Choice choice in dialog.GetChoices())
        {
            if (choice.GetCondition())
            {
                ChoiceContainer choiceContainer =  Instantiate(choiceContainerPrefab, choicesContainer).GetComponent<ChoiceContainer>();
                choiceContainer.Initialize(choice);
            }
        }

        return 0f;//GetComponent<RectTransform>().rect.height;
    }



    private void clearChilds(Transform t)
    {
        foreach (Transform c in t)
        {
            transform.Find("ChoicesContainer").position -= new Vector3(0, Screen.height/20, 0);
            GameObject.Destroy(c.gameObject);
        }
    }
}