using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartNewGameButton : MonoBehaviour {

    public GameObject inputText;
    public string sceneName;

    public void StartNewgame()
    {
        string text = inputText.GetComponent<Text>().text;
        if (text != null && text.Length > 1)
        {
            GameUtils.SetPlayer(text);
            GameUtils.SetNewGame();
            GameUtils.LoadScene(sceneName);
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit"))
        {
            StartNewgame();
        }
    }
}
