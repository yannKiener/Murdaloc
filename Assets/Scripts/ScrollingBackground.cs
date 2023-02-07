using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

	public float backgroundSize;
	public float parralaxSpeed;

	private Transform cameraTransform;
	private Transform[] layers;
	private float viewZone = 10;
	private int leftIndex;
	private int rightIndex;
	private float lastCameraX;




	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
		layers = new Transform[transform.childCount];
		for(int i =0; i<transform.childCount; i++) {
			layers[i] = transform.GetChild(i);
		}
		leftIndex = 0;
		rightIndex = layers.Length-1;

		InitialisationWTF();
	}
	// Pour que le scrolling marche bien et que l'utilisateur ne voie pas
	// De mouvement dégueu lors des premiers déplacements..
	// TODO : Faire en sorte de plus appeller cette fonction de merde. (De préférence ajouter += un Vector3 au lieu d'en set directement un pour garder le Y initial)
	private void InitialisationWTF(){ 
		ScrollLeft();
		ScrollLeft();
		ScrollLeft();
		ScrollRight();
		ScrollRight();
	}

	private void ScrollLeft(){
		int lastRight = rightIndex;
		layers[rightIndex].position = new Vector3((layers[leftIndex].position.x - backgroundSize), 0, 0);
		leftIndex = rightIndex;
		rightIndex--;
		if (rightIndex < 0 ){
			rightIndex = layers.Length-1;
		}
	}

	private void ScrollRight(){
		int lastLeft = leftIndex;
		layers[leftIndex].position = new Vector3((layers[rightIndex].position.x + backgroundSize), 0, 0);
		rightIndex = leftIndex;
		leftIndex++;
		if (leftIndex == layers.Length){
			leftIndex = 0;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX =  cameraTransform.position.x - lastCameraX;
		transform.position += Vector3.right *  (deltaX * parralaxSpeed);
		lastCameraX = cameraTransform.position.x;

		if (cameraTransform.position.x	< (layers[leftIndex].transform.position.x + viewZone)){
			ScrollLeft();
		}
		if (cameraTransform.position.x	> (layers[rightIndex].transform.position.x - viewZone)){
			ScrollRight();
		}
	}
}
