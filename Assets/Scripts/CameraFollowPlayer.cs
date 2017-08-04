using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

	public GameObject player;

	private Vector3 cameraBasicPosition;
	// Use this for initialization
	void Start () {
		cameraBasicPosition = transform.position;//- player.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(player.transform.position.x,cameraBasicPosition.y,cameraBasicPosition.z)  + cameraBasicPosition;
	}
}
