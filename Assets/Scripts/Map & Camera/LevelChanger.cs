using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelChanger : MonoBehaviour {

    public string Scene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.gameObject.tag == "Player")
        {
            Interface.DrawModalDialog("Change level ? ", new System.Action(() => GameUtils.LoadScene(Scene)));
            //Change level
        }
    }

    private void Start()
    {
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

}
