using System.Collections;
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
		if (attachAtObject.GetComponent<SpriteRenderer> () != null && attachAtObject.tag == "Map") {
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
        Instantiate(mapPrefab, transform);
        mapPrefab = transform.GetChild(0).gameObject;
        mapPrefab.AddComponent<MapMusic>();
        mapPrefab.transform.localScale = new Vector3(width, 1, 1);

        stickBoxColliders(attachAtObject, mapPrefab, attachAtRight);

        if (backgroundObjectsList == null || backgroundObjectsList.Count == 0)
        {
            backgroundObjectsList = Interface.GetDefaultBackgroundsGameObjects();
        }

        if (backgroundObjectsList.Count > 0) {
			DrawObjectOnMap (backgroundObjectsList ,mapPrefab, backgroundObjectDensity);
		}

		if (enemyList.Count > 0) {
			DrawObjectOnMap (enemyList ,mapPrefab, enemyDensity, true);
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

	private void DrawObjectOnMap(List<GameObject> objectList, GameObject map, int density, bool isEnemy = false) {
		if(useRandomSeed){
			seed = Time.time.ToString();
        }
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());
        float mapPortion = map.GetComponent<Renderer>().bounds.size.x / width;
        float lowerxBound = getLowerXBound(map);

        for (int i = 0; i < width; i++){
			if(pseudoRandom.Next(0,100) <= density){
				GameObject objToDraw = objectList[UnityEngine.Random.Range(0,objectList.Count)];
                if (!isEnemy)
                {
                    float sizeMultiplier = UnityEngine.Random.Range(1, 3f);
                    Debug.Log(sizeMultiplier);
                    //objToDraw.transform.localScale += new Vector3(sizeMultiplier, sizeMultiplier);
                }

                float x = lowerxBound +  i * mapPortion;
                float y = objToDraw.GetComponent<Renderer>().bounds.size.y/2 + 0.5f;
				if (isEnemy) {
					y += objToDraw.GetComponent<Renderer> ().bounds.size.y/2 ;
				}
				Instantiate(objToDraw, transform); //draw at position x
                objToDraw.transform.position = new Vector3(x, y, 0);
                objToDraw.GetComponent<SpriteRenderer>().sortingOrder = isEnemy? 10 : UnityEngine.Random.Range(0,3);
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
