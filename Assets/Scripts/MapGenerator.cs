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

	private int[] object1Map;

	// Use this for initialization
	void Start () {
		GenerateMap();
	}

	void GenerateMap() {
		Vector3 originalPosition = mapPrefab.transform.position;
		Vector3 originalScale = mapPrefab.transform.localScale;

		print(mapPrefab.transform.position);
		mapPrefab.transform.position += new Vector3((width/2) + startingLeftPoint, 0, 0);
		mapPrefab.transform.localScale += new Vector3(width, 0, 0);
		Instantiate(mapPrefab);

		mapPrefab.transform.position = originalPosition;
		mapPrefab.transform.localScale = originalScale;
		object1Map = new int [width];
	}

	void DrawObject1() {
		if(useRandomSeed){
			seed = Time.time.ToString();
		}

		System.Random pseudoRandom = new System.Random(seed.GetHashCode());
		for (int x = 0; x < width; x++){
			object1Map[x] = (pseudoRandom.Next(0,100) < object1Density)? 1 : 0;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
