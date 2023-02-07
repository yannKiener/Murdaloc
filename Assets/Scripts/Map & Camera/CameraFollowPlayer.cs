using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour {

	public GameObject player;
	public bool follow = true;
	private float timer;

	private Vector3 cameraBasicPosition;
	// Use this for initialization
	void Start () {
		cameraBasicPosition = transform.position;
        if(player == null)
        {
            player = FindUtils.GetPlayer().gameObject;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (follow) {
			if (timer < 1) {
				timer = timer + 0.01f;
			}
            transform.position = Vector3.Lerp(transform.position, new Vector3(player.transform.position.x, cameraBasicPosition.y, cameraBasicPosition.z), timer);
		}
	}

	public void leavePlayer() {
		timer = 0;
		follow = false;
	}

	public void followPlayer() {
		follow = true;
	}





}
