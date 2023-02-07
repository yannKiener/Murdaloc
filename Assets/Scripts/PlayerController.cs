using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float MAXSPEED = 10f;	
	public float JUMPFORCE = 5f;
	bool jumping;
	bool wantToJump;
	bool inCombat = false;
	List<GameObject> enemyList;
	GameObject target;

	// Use this for initialization
	void Start () {
		enemyList = new List<GameObject>();
		jumping = false;
		wantToJump = false;
	}
	
	// Update is called once per frame
	void Update () {

        //EnemyManagement
		if (Input.GetKeyDown ("a")){
			attackTarget (target);
		}
		if (enemyList.Count == 0 && inCombat) {
			leaveCombat ();
		} 



		MovePlayer(GetComponent<Rigidbody2D>()); 
	}

	void OnCollisionEnter2D(Collision2D collision){
		/* on arrete le saut selon un tag
		if(collision.transform.tag == "Ground"){
			jumping = false;
		}*/
		jumping = false;

	}
		

	private void MovePlayer (Rigidbody2D player)
	{
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


		float ySpeed = player.velocity.y;

		if (wantToJump && !jumping){
			ySpeed = JUMPFORCE;
			jumping = true;
		}

		player.velocity = new Vector2 (xSpeed * MAXSPEED, ySpeed);

        //Limit player to camera At ALL TIMES

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
	}

	public void enterCombat (GameObject enemy) {
		if (!inCombat) {
			print ("+combat");
			inCombat = true;
			GameObject.Find ("Main Camera").SendMessage ("leavePlayer");
		}
		if (!enemyList.Contains (enemy)) {
			enemyList.Add (enemy);
			if (enemyList.Count == 1) {
				target = enemy;
			}
		}
	}

	public void leaveCombat () {
		print ("-combat");
		inCombat = false;
		GameObject.Find ("Main Camera").SendMessage ("followPlayer");
	}

	public void attackTarget (GameObject tar) {
		enemyList.Remove (tar);
		Destroy (tar);
		if (enemyList.Count != 0) {
            target = enemyList[0];
		}
	}

	private void limitMapToCamera () {
		GameObject.Find ("Main Camera").SendMessage ("getBoundaries");
		
	}

}
