using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapGenerator : MonoBehaviour {

	public int startingLeftPoint;
	public int width;

	[Range(0,100)]
	public int object1Density;
	public string seed;
	public bool useRandomSeed;	
	public GameObject mapPrefab;
	public GameObject object1Prefab;

	//private int[] object1Map;

	// Use this for initialization
	void Start () {
		GenerateMap();
	}

	void GenerateMap() {
		Vector3 originalPosition = mapPrefab.transform.position;
		Vector3 originalScale = mapPrefab.transform.localScale;

		print("map position :" + mapPrefab.transform.position);
		mapPrefab.transform.position += new Vector3((width/2) + startingLeftPoint, 0, 0);
		mapPrefab.transform.localScale += new Vector3(width, 0, 0);
		Instantiate(mapPrefab);

		mapPrefab.transform.position = originalPosition;
		mapPrefab.transform.localScale = originalScale;
		DrawObjectOnMap(object1Prefab, mapPrefab, object1Density);
	}

	void DrawObjectOnMap(GameObject objToDraw, GameObject map, int density) {
		if(useRandomSeed){
			seed = Time.time.ToString();
		}
		System.Random pseudoRandom = new System.Random(seed.GetHashCode());

		for (int i = 0; i < width; i++){
			if(pseudoRandom.Next(0,100) < density){
				float x = startingLeftPoint + i; //TODO :  trouver la position "gauche" autrement depuis la map en param
				float y = map.transform.position.y + map.GetComponent<BoxCollider2D>().size.y/2 + objToDraw.GetComponent<Renderer>().bounds.size.y /2; //On veut que le pied de l'arbre soit en haut de la map..! Désolé pour la ligne dégueulasse :D
				objToDraw.transform.position = new Vector3(x,y,0);
				Instantiate(objToDraw); //draw at position x
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
