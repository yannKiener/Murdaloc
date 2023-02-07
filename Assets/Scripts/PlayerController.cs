using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float MAXSPEED = 10f;	
	public float JUMPFORCE = 5f;
	bool jumping;
	bool wantToJump;

	// Use this for initialization
	void Start () {
		jumping = false;
		wantToJump = false;
	}
	
	// Update is called once per frame
	void Update () {
		float xSpeed = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown ("space")){
			wantToJump = true;
		}
		if (Input.GetKeyUp("space")) {
			wantToJump = false;
		}
		if (Input.GetKey (KeyCode.LeftShift)){ //Si c'est maintenu. On pourrait changer les sauts aussi pour ca.
			xSpeed = xSpeed * 2;
		}
	

		MovePlayer(GetComponent<Rigidbody2D>(),xSpeed); 
	}

	void OnCollisionEnter2D(Collision2D collision){
		/* on arrete le saut selon un tag
		if(collision.transform.tag == "Ground"){
			jumping = false;
		}*/
		jumping = false;

	}
		


	private void MovePlayer (Rigidbody2D player, float xSpeed)
	{
		float ySpeed = player.velocity.y;

		if (wantToJump && !jumping){
			ySpeed = JUMPFORCE;
			jumping = true;
		}

		player.velocity = new Vector2 (xSpeed * MAXSPEED, ySpeed);
	}
}
