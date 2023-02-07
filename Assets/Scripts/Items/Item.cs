using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : Usable {

    protected string itemName;
    protected string description;
    protected int levelRequirement;
    protected Sprite image;
    protected Stats stats;
    protected string type;

    public Item(string itemName, string description, int levelRequirement, Stats stats, string type)
    {
        this.itemName = itemName;
        this.description = description;
        this.levelRequirement = levelRequirement;
        this.stats = stats;
        this.type = type;
        
        this.image = InterfaceUtils.LoadSpriteForItem(itemName);
        if (image == null)
        {
            Debug.Log("Image null");
        }

    }
    public string GetDescription()
    {
        return description;
    }

    public string GetItemType()
    {
        return type;
    }

    public string GetName()
    {
        return itemName;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }

    public Sprite GetImageAsSprite()
    {
        return image;
    }

    public Stats GetStats()
    {
        return stats;
    }

    public void Use(Character caster)
    {
        FindUtils.GetCharacterSheetGrid().EquipItem(this);
        FindUtils.GetInventory().GetComponent<Inventory>().RemoveItem(this);
    }
}