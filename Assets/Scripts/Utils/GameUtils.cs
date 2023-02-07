﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public class GameUtils : MonoBehaviour {

    private static string scene = null;
    private static string playerName = null;
    private static bool isNewGame = false;
    private GameObject loadingScreen;
    private Slider slider;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    public static void SetPlayer(string name)
    {
        playerName = name;
    }

    public static void SetNewGame()
    {
        isNewGame = true;
    }

    public static List<string> GetGameNames()
    {
        return Directory.GetFiles(Application.persistentDataPath, "*.murk").Select(Path.GetFileName).ToList<string>();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    public static void LoadScene(string sceneName)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            if (!InterfaceUtils.CloseOpenWindows())
            {
                Debug.Log("Closing Windows..");
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
                InterfaceUtils.CloseOpenWindows();
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
            if (isNewGame)
            {
                FindUtils.GetPlayer().CharacterName = playerName;
                SaveGame();
            }
            if(playerName != null)
            {
                LoadSave(playerName).LoadData();
            }
        }
    }

    public static void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/"  + FindUtils.GetPlayer().GetName() + ".murk");
        bf.Serialize(file, new SaveGame());
        file.Close();
    }

    public static SaveGame LoadSave(string saveName)
    {
        SaveGame save = null;
        if (File.Exists(Application.persistentDataPath + "/" + saveName + ".murk"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveName + ".murk", FileMode.Open);
            save = (SaveGame)bf.Deserialize(file);
            file.Close();
        }

        return save;
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
            if (SceneManager.GetActiveScene().name != scene)
            {
                StartCoroutine(LoadSceneAsync(scene));
            }
            scene = null;
        }

    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation =  SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            yield return null;
        }
    }



}
