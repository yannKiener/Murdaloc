using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCategory {
    EquipmentType type;
    EquipmentSlot slot;
    List<Stat> possibleMainStats = new List<Stat>();
    List<Stat> possibleOffStats = new List<Stat>();

    public EquipmentCategory(EquipmentType type, EquipmentSlot slot, List<Stat> possibleMainStats, List<Stat> possibleOffStats)
    {
        this.type = type;
        this.slot = slot;
        this.possibleMainStats = possibleMainStats;
        this.possibleOffStats = possibleOffStats;
    }

    public EquipmentType GetCategoryType()
    {
        return type;
    }

    public List<Stat> GetPossibleMainStats()
    {
        return possibleMainStats;
    }
    public List<Stat> GetPossibleOffStats()
    {
        return possibleOffStats;
    }

    public EquipmentSlot GetSlot()
    {
        return slot;
    }
}
