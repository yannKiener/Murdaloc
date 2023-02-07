using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : AbstractCharacter
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
		if (Input.GetKeyDown (KeyCode.Alpha6)) {
			CastSpell (actionBar [4]);
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

			cakeslice.Outline outline = target.GetGameObject ().GetComponent<cakeslice.Outline> ();
			Destroy(outline);

			target = enemyList[enemyOffset];
		}

	}

	void OnGUI()
	{

		GUI.Box (new Rect (0, 0, 200, 20), name);
		GUI.Box (new Rect (0, 20, 200, 20), currentLife + " / " + stats.MaxLife);
		GUI.Box(new Rect(0,20,(int) ((float)currentLife/(float)stats.MaxLife*200),20), new Texture2D(1,1)); 
		GUI.Box (new Rect (0, 40, 200, 20), currentResource + " / " + stats.MaxResource);
		GUI.Box(new Rect(0,40,(int) ((float)currentResource/(float)stats.MaxResource*200),20), new Texture2D(1,1)); 

		if (target != null && !target.IsDead()) {
			GUI.Box (new Rect (400, 0, 200, 20), target.GetName());
			GUI.Box (new Rect (400, 20, 200, 20), target.GetCurrentLife() + " / " + target.GetMaxLife());
			GUI.Box (new Rect (400, 20, (int) ((float)target.GetCurrentLife()/(float)target.GetMaxLife()*200), 20), new Texture2D(1,1));
			GUI.Box (new Rect (400, 40, 200, 20), target.GetCurrentResource() + " / " + target.GetMaxResource());
			GUI.Box(new Rect(400,40,(int) ((float)target.GetCurrentResource()/(float)target.GetMaxResource()*200),20), new Texture2D(1,1)); 

			//test outlining target
			if (target.GetGameObject ().GetComponent<cakeslice.Outline> () == null) {
				target.GetGameObject ().AddComponent<cakeslice.Outline> ();
			}
		}

		if (casting) {
			float spellCastTime = castingSpell.GetCastTime (stats);
			if (spellCastTime != 0) {
				int castPercentage = (int)(100 * (castingTime / spellCastTime));

				GUI.Box (new Rect (Screen.width / 2, 93	* Screen.height / 100, 100, 30), castingSpell.GetName () + " : " + castingTime.ToString ("0.0") + " / " + spellCastTime);
				GUI.Box (new Rect (Screen.width / 2, 93 * Screen.height / 100, castPercentage, 30), new Texture2D (1, 1)); 
			}
		}
		//GUI.BeginGroup(new Rect(400,400, 10,20));
		//	GUI.EndGroup();
		//GUI.EndGroup();

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
			ySpeed = JUMPFORCE;
			jumping = true;
		}

		player.velocity = new Vector2(xSpeed * MAXSPEED, ySpeed);

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


