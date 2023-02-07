using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour {

    public string Scene;
    public string Message;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Interface.DrawModalDialog(Message, new System.Action(() => GameUtils.LoadScene(Scene)));
        }
    }

    private void Start()
    {
        if(Message == null || Message.Length == 0)
        {
            Message = "Change level ? ";
        }
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

}