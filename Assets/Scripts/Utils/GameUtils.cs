using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUtils : MonoBehaviour {

    private static string scene = null;
    private GameObject loadingScreen;
    private Slider slider;

    public static void LoadScene(string sceneName)
    {
        scene = sceneName;
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
