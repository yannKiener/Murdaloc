using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MonoBehaviour {

    public GameObject GamesContainer;
    public GameObject NewGameContainer;

    public void SwitchGamesPanel()
    {
        GamesContainer.SetActive(!GamesContainer.activeSelf);
        NewGameContainer.SetActive(!NewGameContainer.activeSelf);
    }
}
