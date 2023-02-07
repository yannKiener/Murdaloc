using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGrid : MonoBehaviour {

    public GameObject QuestContainer;
    public GameObject BackButton;

    void OnEnable()
    {
        FindUtils.GetInterface().OpenQuestlog();
    }

    void OnDisable()
    {
        FindUtils.GetInterface().CloseQuestlog();
    }

    public void ShowQuestDetails(string questStr)
    {
        clearChilds(transform);
        Instantiate(BackButton, transform);
        GameObject textGO =  Instantiate(new GameObject("questDetails"), transform);
        Quest quest = Quests.GetQuest(questStr);
        string questDescription = quest.GetDescription();
        if (quest.IsReady())
        {
            questDescription = "Ready to turn in!";
        }
        Text text = textGO.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontSize = 12;
        text.text = questDescription;
        text.color = Color.black;
        textGO.GetComponent<RectTransform>().sizeDelta = new Vector2(140,100);


    }

    public void UpdateQuestLog()
    {
        clearChilds(transform);

        foreach (KeyValuePair<string, Quest> kv in Quests.GetQuests())
        {
            AddNewQuest(kv.Value);
        }

    }

    private void AddNewQuest(Quest quest)
    {
        GameObject container = Instantiate(QuestContainer, transform);
        container.GetComponentInChildren<Text>().text = quest.GetName();
    }


    private void clearChilds(Transform t)
    {
        foreach (Transform c in t)
        {
            GameObject.Destroy(c.gameObject);
        }
    }
}
