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
    private Vector3 initialPosition;

    new void Start()
    {
        Interface.LoadPlayer();
        base.Start();
        GetComponent<SpriteRenderer>().sortingOrder = 10;
        this.initialPosition = this.transform.position;
    }

    public void InitializeWith(string name, int cash, int level, float expPercent, Resource rsrc, List<Spell> spellList, List<Equipment> charSheetItems, List<Item> inventoryItems)
    {
        this.CharacterName = name;
        this.level = level;
        this.experiencePercent = expPercent;

        if (rsrc == null)
        {
            resource = new Mana();
        }

        ClearSpells();
        foreach(Spell spell in spellList)
        {
            AddSpell(spell);
        }
        this.stats = GetBaseStatsForLevel(level);
        foreach(Equipment item in charSheetItems)
        {
            FindUtils.GetCharacterSheetGrid().EquipEquipment(item);
        }
        foreach(Item item in inventoryItems)
        {
            FindUtils.GetInventoryGrid().AddItem(item);
        }

        FindUtils.GetInventoryGrid().AddCash(cash);


        /*
         * Initialise player with this infos : 
         actionBar
         QuestsDoing
        */
    }

    void Controls()
    {
        if (Input.GetButtonDown("CycleTargets"))
        {
            CycleTargets();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            if (!Interface.CloseModalDialog() && !InterfaceUtils.CloseOpenWindows() && !CancelCast())
            {
                if(FindUtils.GetPlayer().GetTarget() != null)
                {
                    FindUtils.GetPlayer().CancelTarget();
                } else
                {
                    Interface.OpenCloseMenu();
                }
            }
        }
        if (Input.GetButtonDown("AutoAttack"))
        {
            SwitchAutoAttack();
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

        if (Input.GetKeyDown(KeyCode.X))
        {
            LevelUp();
            FindUtils.GetInventoryGrid().AddCash(100000);
            FindUtils.GetInventoryGrid().AddItem(EquipmentGenerator.GenerateEquipment(level));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Superior health potion"));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Potion of cunning"));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Potion of might"));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Potion of deftness"));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Longjaw Mud Snapper"));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Boiled Clams"));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Refreshing Spring Water"));
            FindUtils.GetInventoryGrid().AddItem(Items.GetConsumableFromDB("Ice Cold Milk"));
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (GetTarget() != null && GetTarget() is Hostile)
                GetTarget().die();
        }

        if (Input.GetMouseButtonDown(0))
        {
            FindUtils.GetInterface().Click();
        }

    }

    void Update() 
	{
		UpdateCharacter ();
        Controls();
        MovePlayer(GetComponent<Rigidbody2D>()); 
	}

    public override void LevelUp()
    {
        base.LevelUp();
        FindUtils.GetCharacterSheetText().text = "Character\nLevel : " + level;

    }

    public void ClearSpells()
    {
        spellList.Clear();
    }

    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }


    public override void AddSpell(Spell spell)
    {
        base.AddSpell(spell);
        if(FindUtils.GetSpellBookGrid() != null)
            FindUtils.GetSpellBookGrid().UpdateSpellBook();

        if (FindUtils.GetActionBar() != null)
        {
            FindUtils.GetActionBar().Add(spell);
        }

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
        SoundManager.PlaySound(Resources.Load<AudioClip>("Sounds/wasted"));
		Instantiate (Resources.Load ("Wasted"));
	}
}


