using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayButton : MonoBehaviour {

    public GameObject ExitButtonGO;
    public GameObject PlayButtonGO;
    public GameObject SavesContainerGO;

    public void Play()
    {
        ExitButtonGO.SetActive(false);
        SavesContainerGO.SetActive(true);
        SavesGrid savesGrid = SavesContainerGO.transform.GetChild(0).GetComponent<SavesGrid>();
        List<string> gamesList = GameUtils.GetGameNames();
        foreach (string gameName in gamesList)
        {
            savesGrid.AddSave(gameName.Replace(".murk", ""));
        }
        PlayButtonGO.SetActive(false);
    }

}
