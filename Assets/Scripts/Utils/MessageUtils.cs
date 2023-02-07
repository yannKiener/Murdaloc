using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MessageUtils {
    
    public static void ErrorMessage(string text)
    {
        GameObject floatingTextGameObj = (GameObject)UnityEngine.GameObject.Instantiate(Resources.Load("FloatingText"));
        FloatingText floatingText = floatingTextGameObj.AddComponent<FloatingText>();
        floatingText.setText(text);
        floatingText.setColor(Color.red);
        floatingText.transform.position += new Vector3(FindUtils.GetPlayer().transform.position.x,0,0);

    }

}