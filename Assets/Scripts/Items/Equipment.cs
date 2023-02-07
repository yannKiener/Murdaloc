using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Equipment : Item
{
    protected int levelRequirement;
    protected Stats stats;
    protected EquipmentType type;
    public bool isEquipped;

    public Equipment(string itemName, string description, int levelRequirement, Stats stats, EquipmentType type)
    {
        this.sellPrice = 5000 + levelRequirement * 2500;
        this.itemName = itemName;
        this.description = description;
        this.levelRequirement = levelRequirement;
        this.stats = stats;
        this.type = type;
        isEquipped = false;
        isInInventory = true;
        
        this.image = InterfaceUtils.LoadSpriteForItem(itemName);
    }

    public override string GetDescription()
    {
        return type + ". " + description +"\nLevel required : " + levelRequirement+ stats.GetStatsDetail() + "\n" + getPriceSentence();
    }

    public void SetImage(Sprite image)
    {
        this.image = image;
    }

    public EquipmentSlot GetEquipmentSlot()
    {
        return EquipmentCategories.GetCategory(type).GetSlot();
    }

    public EquipmentType GetEquipmentType()
    {
        return type;
    }

    public int GetLevelRequirement()
    {
        return levelRequirement;
    }


    public Stats GetStats()
    {
        return stats;
    }

    public bool IsUsable()
    {
        return (FindUtils.GetPlayer().GetLevel() >= levelRequirement);
    }

    public override void Use(Character caster)
    {
        if (isInInventory)
        {
            if (IsUsable()) { 
                if (isEquipped)
                {
                    FindUtils.GetCharacterSheetGrid().RemoveEquipment(this);
                } else
                {
                    FindUtils.GetCharacterSheetGrid().EquipEquipment(this);
                }
            } else
            {
                MessageUtils.ErrorMessage("Can't use that yet.");
            }
        } else {
            GetFromLootToInventory();
        }
    }
}