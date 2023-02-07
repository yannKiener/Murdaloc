using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsToDraw {
    List<GameObject> objectList;
    int densityPercent;
    bool areEnemies;

    public ObjectsToDraw(List<GameObject> gameObjList, int density, bool isEnemy)
    {
        this.objectList = gameObjList;
        this.densityPercent = density;
        this.areEnemies = isEnemy;
    }

    public List<GameObject> GetObjectList()
    {
        return objectList;
    }

    public int GetDensity()
    {
        return densityPercent;
    }

    public bool IsEnemyList()
    {
        return areEnemies;
    }

    public GameObject GetRandomObject()
    {
        return objectList[UnityEngine.Random.Range(0, objectList.Count)];
    }
}
