using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EquipmentCategories {

    static Dictionary<EquipmentType, EquipmentCategory> equipmentCategories = new Dictionary<EquipmentType, EquipmentCategory>();

    public static void AddCategory(EquipmentCategory category)
    {
        equipmentCategories[category.GetCategoryType()] = category;
    }

    public static EquipmentCategory GetCategory(EquipmentType type)
    {
        return equipmentCategories[type];
    }
}
