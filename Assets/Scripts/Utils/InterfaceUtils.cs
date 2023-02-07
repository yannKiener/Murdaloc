using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class InterfaceUtils {
    
    public static void CreateUsableSlot(GameObject prefab, Transform attachTo, Sprite sprite, Usable usable)
    {
        GameObject usableSlot = UnityEngine.GameObject.Instantiate(prefab, attachTo);
        Image image = usableSlot.GetComponent<Image>();
        image.sprite = sprite;
        usableSlot.GetComponent<Draggable>().usable = usable;
        attachTo.GetComponent<Slot>().usable = usable;
    }

    public static Texture2D GetTextureWithColor(Color col)
    {
        Color[] uselessColorArray = new Color[1];
        uselessColorArray[0] = col;
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixels(uselessColorArray);
        texture.Apply();
        return texture;
    }

    public static Texture LoadTextureFor(string name)
    {
        return Resources.Load("Images/SpellIcons/" + name) as Texture;
    }

    public static Image LoadImageFor(string name)
    {
        return Resources.Load("Images/SpellIcons/" + name) as Image;
    }

    public static Sprite LoadSpriteFor(string name)
    {
        return Resources.Load<Sprite>("Images/SpellIcons/" + name) as Sprite;
    }
    
}