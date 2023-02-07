using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemCategories {

    static Dictionary<ItemType, ItemCategory> itemCategories = new Dictionary<ItemType, ItemCategory>();

    public static void AddCategory(ItemCategory category)
    {
        itemCategories.Add(category.GetCategoryType(), category);
    }

    public static ItemCategory GetCategory(ItemType type)
    {
        return itemCategories[type];
    }
}
