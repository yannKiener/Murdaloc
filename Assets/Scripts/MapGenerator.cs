using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour {

	public int width;

	[Range(0,100)]
	public int object1Density;
	public string seed;
	public bool useRandomSeed;
	public bool attachAtRight;
	public GameObject attachAtObject;
	public GameObject mapPrefab;
	public GameObject object1Prefab;


	// Use this for initialization
	void Start () {
		GenerateMap();
	}

	void GenerateMap() {
		
		float startingX = 0;
		float startingY = 0;
		
		if(attachAtRight){
			startingX = getUpperXBound(attachAtObject) + (width/2);
			startingY = attachAtObject.transform.position.y + attachAtObject.GetComponent<BoxCollider2D> ().offset.y + attachAtObject.GetComponent<BoxCollider2D> ().bounds.size.y / 2 - mapPrefab.GetComponent<Renderer> ().bounds.size.y*mapPrefab.GetComponent<BoxCollider2D> ().size.y/2;
		} else {
			startingX = getLowerXBound(attachAtObject) - (width/2);
			startingY = attachAtObject.transform.position.y + attachAtObject.GetComponent<BoxCollider2D> ().offset.y + attachAtObject.GetComponent<BoxCollider2D> ().bounds.size.y / 2 - mapPrefab.GetComponent<Renderer> ().bounds.size.y*mapPrefab.GetComponent<BoxCollider2D> ().size.y/2;
		}
		//TODO trouver un moyen plus clean pour coller les deux BoxCollider2D ensemble ! :D Voir plus bas.

		//float startingRightX = getLowerXBound(attachAtObject);

		mapPrefab.transform.position = new Vector3(startingX, startingY, 0);
		mapPrefab.transform.localScale = new Vector3(width, 1, 1);
		Instantiate(mapPrefab);

		DrawObjectOnMap(object1Prefab, mapPrefab, object1Density);
	}

	void DrawObjectOnMap(GameObject objToDraw, GameObject map, int density) {
		if(useRandomSeed){
			seed = Time.time.ToString();
		}
		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int i = 0; i < width; i++){
			if(pseudoRandom.Next(0,100) < density){
				float x = getLowerXBound(map) + i;
				float y = getUpperYBound(map) + objToDraw.GetComponent<Renderer>().bounds.size.y/2;
				objToDraw.transform.position = new Vector3(x,y,0);
				Instantiate(objToDraw); //draw at position x
			}
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
			centerPosition = obj.transform.position.x;
			halfSize = obj.GetComponent<Renderer>().bounds.size.x/2; 
		} else if(coord == "y"){
			centerPosition = obj.transform.position.y;
			halfSize = obj.GetComponent<Renderer>().bounds.size.y/2;
		}

		return upperRight? centerPosition + halfSize : centerPosition - halfSize;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
