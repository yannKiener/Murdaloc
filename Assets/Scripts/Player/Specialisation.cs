using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Specialisation
{
    string name;
    Dictionary<int, Talent> talentTree = new Dictionary<int, Talent>();
    int pointsInSpec = 0;

    public Specialisation (string name)
    {
        this.name = name;
    }
    
    public Specialisation(Specialisation s)
    {
        this.name = s.GetName();
        this.talentTree = s.GetTalentTree();
    }

    public void ResetPointsInSpec()
    {
        pointsInSpec = 0;
    }

    public void AddPointInSpec()
    {
        pointsInSpec += 1;
    }

    public int GetPointsInSpec()
    {
        return pointsInSpec;
    }

    public string GetName()
    {
        return name;
    }

    public Dictionary<int, Talent> GetTalentTree()
    {
        return talentTree;
    }

    public void SetTalent(int slotNumber, Talent talent)
    {
        if(talent != null && slotNumber <= 28 && slotNumber > 0)
        {
            talentTree[slotNumber] = talent;
        } else
        {
            Debug.LogError("TALENT IS NULL OR  IT'S NUMBER IS OUT OF BOUND !!");
        }
    }
}