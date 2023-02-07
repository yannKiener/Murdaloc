using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : Character
{
	private bool jumping = false;
	private bool wantToJump = false;
	private string[] actionBar = new string[5];
	private int enemyOffset = 0;

	void Start(){

	}

	void Update() 
	{
		UpdateCharacter ();

		//EnemyManagement
		if(Input.GetKeyUp(KeyCode.Tab)){
			CycleTargets();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CancelCast ();
		}
		if (Input.GetKeyDown (KeyCode.T)) {
			SwitchAutoAttack ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			CastSpell(actionBar[0]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			CastSpell(actionBar[1]);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			CastSpell (actionBar [2]);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			CastSpell (actionBar [3]);
		}

		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			print ("LevelUp !");
			LevelUp ();
			stats.displayStats ();
		}

		MovePlayer(GetComponent<Rigidbody2D>()); 
	}

	private void CycleTargets(){
		int count = enemyList.Count;
		if (count > 1) {
			enemyOffset += 1;

			if (enemyOffset >= count) {
				enemyOffset = 0;
			}


			SetTarget(enemyList[enemyOffset]);
		}

	}

	public override void SetTarget (Character target)
	{
		if (this.target != null && this.target.GetGameObject ().GetComponent<cakeslice.Outline> () != null) { //Remove old target outline
			cakeslice.Outline outline = this.target.GetGameObject ().GetComponent<cakeslice.Outline> ();
			Destroy(outline);
		}

		target.GetGameObject ().AddComponent<cakeslice.Outline> ();
		base.SetTarget (target);
	}





	public void SetActionBarSlot(int slot, string slotName)
	{
		if (actionBar.Length > slot)
			actionBar [slot] = slotName;
		else
			print ("slotName too great");
	}


	void OnCollisionEnter2D(Collision2D collision)
	{
		/* on arrete le saut selon un tag
        if(collision.transform.tag == "Ground"){
            jumping = false;
        }*/
		jumping = false;

	}

	private void MovePlayer(Rigidbody2D player)
	{
		float xSpeed = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown("space"))
		{
			wantToJump = true;
		}
		if (Input.GetKeyUp("space"))
		{
			wantToJump = false;
		}
		if (Input.GetKey(KeyCode.LeftShift))
		{ //Si c'est maintenu. On pourrait changer les sauts aussi pour ca.
			xSpeed = xSpeed * 2;
		}
			
		float ySpeed = player.velocity.y;

		if (wantToJump && !jumping)
		{
			ySpeed = Constants.JumpForce;
			jumping = true;
		}

		player.velocity = new Vector2(xSpeed * Constants.MaxSpeed, ySpeed);

		//Limit player to camera At ALL TIMES

		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
		pos.x = Mathf.Clamp01(pos.x);
		pos.y = Mathf.Clamp01(pos.y);
		transform.position = Camera.main.ViewportToWorldPoint(pos);
	}

	protected override void EnterCombat(){
		base.EnterCombat();
		GameObject.Find("Main Camera").SendMessage("leavePlayer");
	}

	protected override void LeaveCombat(){
		base.LeaveCombat();
		GameObject.Find("Main Camera").SendMessage("followPlayer");
	}

	public override void kill()	
	{
		base.kill ();
		Instantiate (Resources.Load ("Wasted"));
	}


}


