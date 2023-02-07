using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour {

    public string Scene;
    public string Message;
    public Vector2 positionAfterLoading;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Interface.DrawModalDialog(Message, new System.Action(() => { GameUtils.LoadScene(Scene); SceneManager.sceneLoaded += movePlayerToPosition; }));
        }
    }

    private void movePlayerToPosition(Scene scene, LoadSceneMode mode)
    {
        FindUtils.GetPlayer().gameObject.transform.position = positionAfterLoading;
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.transform.position = new Vector3(positionAfterLoading.x, camera.transform.position.y, camera.transform.position.z);
        SceneManager.sceneLoaded -= movePlayerToPosition;
    }


    private void Start()
    {
        if(Message == null || Message.Length == 0)
        {
            Message = "Change level to " + Scene + " ? ";
        }
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

}