using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesButton : MonoBehaviour {

    public string saveName;

    public void StartGame()
    {
        GameUtils.SetPlayer(saveName);
        GameUtils.LoadScene(GameUtils.LoadSave(saveName).GetLastScene());
    }
}
