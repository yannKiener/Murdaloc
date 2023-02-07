using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageUtils {
    
    public static void ErrorMessage(string text)
    {
        Debug.Log(text);
        GameObject floatingTextGameObj = (GameObject)UnityEngine.GameObject.Instantiate(Resources.Load("FloatingText"));
        FloatingText floatingText = floatingTextGameObj.AddComponent<FloatingText>();
        floatingText.setText(text);
        floatingText.setColor(Color.red);
        floatingText.transform.position += new Vector3(FindUtils.GetPlayer().transform.position.x,0,0);
    }


    public static void Message(string text)
    {
        GameObject floatingTextGameObj = (GameObject)UnityEngine.GameObject.Instantiate(Resources.Load("FloatingText"));
        FloatingText floatingText = floatingTextGameObj.AddComponent<FloatingText>();
        floatingText.setText(text);
        floatingText.setColor(Color.white);
        floatingText.transform.position += new Vector3(FindUtils.GetPlayer().transform.position.x, -1, 0);
    }

}