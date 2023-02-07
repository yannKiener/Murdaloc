using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player : Character
{
	private bool jumping = false;
	private bool wantToJump = false;
	private int enemyOffset = 0;
    private float experiencePercent;

	void Start()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 10;

    }

	void Update() 
	{
		UpdateCharacter ();

		//EnemyManagement
		if(Input.GetButtonDown("CycleTargets"))
        {
			CycleTargets();
		}
		if (Input.GetButtonDown("Cancel")) {
            if (!InterfaceUtils.CloseOpenWindows() && !CancelCast())
            {
                FindUtils.GetPlayer().CancelTarget();
            }
		}
		if (Input.GetButtonDown("AutoAttack")) {
			SwitchAutoAttack ();
		}
        if (Input.GetButtonDown("ShowHideSpellBook"))
        {
            InterfaceUtils.ShowHideSpellBook();
        }
        if (Input.GetButtonDown("ShowHideInventory"))
        {
            InterfaceUtils.ShowHideInventory();
        }
        if (Input.GetButtonDown("ShowHideCharacterSheet"))
        {
            InterfaceUtils.ShowHideCharacterSheet();
        }
        if (Input.GetButtonDown("ShowHideQuestLog"))
        {
            InterfaceUtils.ShowHideQuestLog();
        }

        if (Input.GetKeyDown (KeyCode.X)) {
			LevelUp ();
            FindUtils.GetInventoryGrid().AddItem(ItemGenerator.GenerateItem(level));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if(GetTarget() != null)
                GetTarget().die();
        }


        MovePlayer(GetComponent<Rigidbody2D>()); 
	}

    public override void LevelUp()
    {
        base.LevelUp();
        FindUtils.GetCharacterSheetText().text = "Character\nLevel : " + level;

    }

    public override void AddSpell(Spell spell)
    {
        base.AddSpell(spell);
        if(FindUtils.GetSpellBookGrid() != null)
            FindUtils.GetSpellBookGrid().UpdateSpellBook();
    }

    public void initializeSpell(Spell spell)
    {
        base.AddSpell(spell);
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

	protected override void UpdateAutoAttack ()
	{
		if (target != null && target.gameObject.tag == "Enemy") {
			base.UpdateAutoAttack ();
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

        if(IsCasting() && (xSpeed > 0.1f || xSpeed < -0.1f))
        {
            CancelCast();
            MessageUtils.ErrorMessage("Can't cast while walking");
        }
        
		if (Input.GetButtonDown("Jump"))
		{
			wantToJump = true;
		}
		if (Input.GetButtonUp("Jump"))
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

		player.velocity = new Vector2(xSpeed * stats.MaxSpeed, ySpeed);

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

    public float GetExp()
    {
        return experiencePercent;
    }
    public void AddExp(float expPercent, float ratioForNextLevel = 1)
    {
        this.experiencePercent += expPercent;
        if(experiencePercent >= 100)
        {
            LevelUp();
            experiencePercent -= 100;
            experiencePercent /= ratioForNextLevel;
        }

    }

	public override void die()	
	{
		base.die ();
		Instantiate (Resources.Load ("Wasted"));
	}
}


