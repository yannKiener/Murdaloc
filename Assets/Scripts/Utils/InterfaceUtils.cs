using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class InterfaceUtils {
    
    public static void CreateUsableSlot(GameObject prefab, Transform attachTo, Sprite sprite, Usable usable)
    {
        GameObject usableSlot = GameObject.Instantiate(prefab, attachTo);
        Image image = usableSlot.GetComponent<Image>();
        image.sprite = sprite;
        usableSlot.GetComponent<Draggable>().usable = usable;
        attachTo.GetComponent<Slot>().usable = usable;
        if(usable is Consumable)
        {
            GameObject.Instantiate(Resources.Load<GameObject>("Prefab/UI/Stacks"),usableSlot.transform);
        }
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

    public static Texture LoadTextureForSpell(string name)
    {
        return Resources.Load<Texture>("Images/SpellIcons/" + name) as Texture;
    }

    public static Image LoadImageForSpell(string name)
    {
        return Resources.Load<Image>("Images/SpellIcons/" + name) as Image;
    }

    public static Sprite LoadSpriteForSpell(string name)
    {
        return Resources.Load<Sprite>("Images/SpellIcons/" + name) as Sprite;
    }

    public static Texture2D LoadTextureForItem(string name)
    {
        return Resources.Load<Texture>("Images/Items/" + name) as Texture2D;
    }

    public static Image LoadImageForItem(string name)
    {
         return Resources.Load("Images/Items/" + name) as Image;
    }
    public static Sprite LoadSpriteForConsumable(string name)
    {
        return LoadSpriteForItem("Consumable/" + name);
    }
    public static Sprite LoadSpriteForItem(string name)
    {
        Sprite result = Resources.Load<Sprite>("Images/Items/" + name) as Sprite; 
        if(result == null)
        {
            result = Resources.Load<Sprite>("Images/Items/Default") as Sprite;
        }
        return result;
    }

    public static bool CloseOpenWindows()
    {
        if (FindUtils.GetInventory().activeSelf)
        {
            ShowHideInventory();
            return true;
        }
        if (FindUtils.GetCharacterSheet().activeSelf)
        {
            ShowHideCharacterSheet();
            return true;
        }
        if (FindUtils.GetSpellBook().activeSelf)
        {
            ShowHideSpellBook();
            return true;
        }
        if (FindUtils.GetQuestLog().activeSelf)
        {
            ShowHideQuestLog();
            return true;
        }
        return false;
    }

    //Controles d'interfaces
    public static void ShowHideSpellBook()
    {
        FindUtils.GetSpellBook().SetActive(!FindUtils.GetSpellBook().activeSelf);
    }
    public static void ShowHideInventory()
    {
        FindUtils.GetInventory().SetActive(!FindUtils.GetInventory().activeSelf);
    }
    public static void ShowHideCharacterSheet()
    {
        FindUtils.GetCharacterSheet().SetActive(!FindUtils.GetCharacterSheet().activeSelf);
    }
    public static void ShowHideQuestLog()
    {
        FindUtils.GetQuestLog().SetActive(!FindUtils.GetQuestLog().activeSelf);
    }


    public static string GetStringPrice(int price)
    {
        string result = "";
        int gold = GetGold(price);
        int silver = GetSilver(price);
        int copper = GetCopper(price);
        if(gold > 0)
        {
            result += gold + "g ";
        }
        if(silver > 0)
        {
            result += silver + "s ";
        }
        if(copper > 0)
        {
            result += copper + "c ";
        }
        return result;
    }

    public static int GetCopper(int price)
    {
        return price % 100;
    }

    public static int GetSilver(int price)
    {
        return (price / 100) % 100;
    }

    public static int GetGold(int price)
    {
        return price / 10000;
    }
}