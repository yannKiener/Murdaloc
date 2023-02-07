using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {

    public GameObject FirstMenuContainer;
    public GameObject GamesMenuContainer;

    public void Play()
    {
        SavesGrid savesGrid = GamesMenuContainer.transform.Find("SavesContainer").GetChild(0).GetComponent<SavesGrid>();
        savesGrid.Clear();
        List<string> gamesList = GameUtils.GetGameNames();
        foreach (string gameName in gamesList)
        {
            string saveName = gameName.Replace(".murk", "");
            int level = GameUtils.LoadSave(saveName).GetLevel();
            savesGrid.AddSave(saveName, level);
        }
        ChangeActiveButtons();
    }

    public void Return()
    {
        ChangeActiveButtons();
    }

    private void ChangeActiveButtons()
    {
        FirstMenuContainer.SetActive(!FirstMenuContainer.activeSelf);
        GamesMenuContainer.SetActive(!GamesMenuContainer.activeSelf);
    }
}
