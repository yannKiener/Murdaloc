﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour {

	public int width;
	public bool attachAtRight;
	public GameObject attachAtObject;
	public GameObject mapPrefab;
	[Range(0,100)]
	public int backgroundObjectDensity;
	public List<GameObject> backgroundObjectsList;
	[Range(0,20)]
	public int enemyDensity;
	public List<GameObject> enemyList;
	public string seed;
	public bool useRandomSeed;
    public AudioClip music;

    public GameObject mapToAttach;

	private bool isGenerated;


	// Use this for initialization
	void Start ()
    {
        gameObject.tag = "Map";
        if(mapToAttach != null)
        {
            mapToAttach.tag = "MovingMap";
        }

        isGenerated = false;
		if (attachAtObject.GetComponent<BoxCollider2D> () != null && attachAtObject.tag == "Map") {
			GenerateMap ();
		}
	}

	void mapReady(String id){
		if (!isGenerated && attachAtObject.name == id) {
			GenerateMap ();
		}
	}

    public AudioClip GetMapMusic()
    {
        return music;
    }

    private void stickBoxColliders(GameObject anchor, GameObject toMove, bool isAtRight)
    {
        float gapX = getSideBoxColliderBound(anchor, isAtRight) - getSideBoxColliderBound(toMove, !isAtRight);
        float gapY = getUpperBoxColliderBound(anchor) - getUpperBoxColliderBound(toMove);
        toMove.transform.position += new Vector3(gapX, gapY, 0);
    }

    void GenerateMap()
    {
        mapPrefab = Instantiate(mapPrefab, transform);
        mapPrefab = transform.GetChild(0).gameObject;
        mapPrefab.AddComponent<MapMusic>();
        mapPrefab.transform.localScale = new Vector3(width, 1, 1);

        stickBoxColliders(attachAtObject, mapPrefab, attachAtRight);

        if (backgroundObjectsList == null || backgroundObjectsList.Count == 0)
        {
            backgroundObjectsList = Interface.GetDefaultBackgroundsGameObjects();
        }

        List<ObjectsToDraw> objectsToDrawList = new List<ObjectsToDraw>();

        if (backgroundObjectsList.Count > 0) {
            ObjectsToDraw backGroundObjects = new ObjectsToDraw(backgroundObjectsList, backgroundObjectDensity, false);
            objectsToDrawList.Add(backGroundObjects);
		}

		if (enemyList.Count > 0)
        {
            ObjectsToDraw enemyObjects = new ObjectsToDraw(enemyList, enemyDensity, true);
            objectsToDrawList.Add(enemyObjects);
        }

        if (objectsToDrawList.Count > 0 )
        {
            DrawObjectOnMap(mapPrefab, objectsToDrawList);
        }

        EndGeneration ();
	}

	private void  EndGeneration(){
		isGenerated = true;
        if(mapToAttach != null)
        {
            mapToAttach.tag = "Map";
            stickBoxColliders(gameObject, mapToAttach, attachAtRight);
        }

		GameObject[] gameobjects = GameObject.FindGameObjectsWithTag ("Map");
		foreach (GameObject go in gameobjects) {
			if (go.GetComponent<MapGenerator> () != null) {
				go.SendMessage ("mapReady", this.gameObject.name);
                if(mapToAttach != null)
                {
                    go.SendMessage("mapReady", mapToAttach.gameObject.name);
                }
			}
		}

	}

	private void DrawObjectOnMap(GameObject map, List<ObjectsToDraw> objectsToDrawList) {
		if(useRandomSeed){
			seed = Time.time.ToString();
        } 

        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        float mapPortion = map.GetComponent<Renderer>().bounds.size.x / width;
        float lowerxBound = getLowerXBound(map);

        for (int i = 0; i < width; i++){
            foreach(ObjectsToDraw objectsToDraw in objectsToDrawList)
            {
			    if(pseudoRandom.Next(0,100) <= objectsToDraw.GetDensity())
                {
                    GameObject objToDraw = objectsToDraw.GetRandomObject();
                    objToDraw = Instantiate(objToDraw, transform); //draw at position x
                    float yOffSet = 0;
                    if (!objectsToDraw.IsEnemyList())
                    {
                        float sizeMultiplier = UnityEngine.Random.Range(1, 1.5f);
                        objToDraw.transform.localScale = new Vector3(objToDraw.transform.localScale.x*sizeMultiplier, objToDraw.transform.localScale.y * sizeMultiplier);
                        objToDraw.GetComponent<SpriteRenderer>().sortingOrder = (int)Math.Round(sizeMultiplier);
                        yOffSet = 0.7f - (sizeMultiplier / 1.5f)*0.2f;
                    } else
                    {
                        objToDraw.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    }

                    float x = lowerxBound +  i * mapPortion;
                    float y = objToDraw.GetComponent<Renderer>().bounds.size.y/2 + yOffSet;

                    objToDraw.transform.localPosition = new Vector3(x, y, 0);
                }
            }
        }
	}

    private float getUpperBoxColliderBound(GameObject obj)
    {
        if (obj.GetComponent<BoxCollider2D>() == null)
            obj = obj.transform.GetChild(0).gameObject;

        return obj.GetComponent<BoxCollider2D>().bounds.max.y;
    }

    private float getSideBoxColliderBound(GameObject obj, bool isRight)
    {
        if (obj.GetComponent<BoxCollider2D>() == null)
            obj = obj.transform.GetChild(0).gameObject;
        if (isRight)
        {
            return obj.GetComponent<BoxCollider2D>().bounds.max.x;
        }
        else
        {
            return obj.GetComponent<BoxCollider2D>().bounds.min.x;
        }
    }


    //TODO Faire pareil avec les boxCollider2D a la place des Renderer ? Voir ligne dégueulasse au dessus.
    private float getUpperYBound(GameObject obj){
		return getBounds(obj,true,"y");
	}
	private float getLowerYBound(GameObject obj){
		return getBounds(obj,false,"y");
	}
	private float getUpperXBound(GameObject obj){
		return getBounds(obj,true,"x");
	}
	private float getLowerXBound(GameObject obj){
		return getBounds(obj,false,"x");
	}

	private float getBounds(GameObject obj, Boolean upperRight, String coord){
		float centerPosition = 0;
		float halfSize = 0;

		if (coord == "x"){
			centerPosition = obj.transform.localPosition.x;
			halfSize = obj.GetComponent<Renderer>().bounds.size.x/2;
		} else if(coord == "y"){
			centerPosition = obj.transform.localPosition.y;
			halfSize = obj.GetComponent<Renderer>().bounds.size.y/2;
		}

		return upperRight? centerPosition + halfSize : centerPosition - halfSize;
	}
	
}
