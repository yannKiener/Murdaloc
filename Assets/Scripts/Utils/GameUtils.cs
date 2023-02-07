using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUtils : MonoBehaviour {

    private static string scene = null;
    private static string playerName = null;
    private GameObject loadingScreen;
    private Slider slider;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public static void LoadScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            while (!InterfaceUtils.CloseOpenWindows())
            {
                InterfaceUtils.CloseOpenWindows();
            }
            SaveGame();
            playerName = FindUtils.GetPlayer().GetName();
        }
        scene = sceneName;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
        {
            if(playerName != null)
            {
                LoadGame(playerName);
            }
        }
    }

    public static void SaveGame()
    {
        Debug.Log("Saving...");
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/"  + FindUtils.GetPlayer().GetName() + ".murk");
        bf.Serialize(file, new SaveGame());
        file.Close();
    }

    public static void LoadGame(string saveName)
    {
        Debug.Log("Loading...");
        SaveGame save = new SaveGame();
        if (File.Exists(Application.persistentDataPath + "/" + saveName + ".murk"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveName + ".murk", FileMode.Open);
            save = (SaveGame)bf.Deserialize(file);
            file.Close();
        }

        save.Load();
    }

    private void Start()
    {
        loadingScreen = transform.GetChild(0).gameObject;
        slider = loadingScreen.transform.GetComponentInChildren<Slider>();
    }

    void Update()
    {
        if(scene != null)
        {
            StartCoroutine(LoadSceneAsync(scene));
            scene = null;
        }

    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation =  SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loadingScreen.SetActive(true);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            if (progress >= 1)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }



}
