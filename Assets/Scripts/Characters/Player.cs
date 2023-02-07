using System;
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
    private float xSpeed;
    private int talentPoints;
    public GameObject LevelUpAnimation;


    private void Awake()
    {
        Interface.LoadPlayer();
        gameObject.layer = 9;
        GetComponent<SpriteRenderer>().sortingOrder = 10;
        casting = false;
        isDead = false;
        castingSpell = null;

        if (resource == null)
        {
            resource = new Mana();
        }
        stats = GetBaseStatsForLevel(level);
        currentLife = stats.MaxLife;
        currentResource = stats.MaxResource;
        autoAttack1Damage = GetBasicAutoAttackDamage();
        autoAttack1Speed = Constants.BaseAutoAttackSpeed;

        addSpells(SpellList);

        this.initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    public void StartJumping()
    {
        wantToJump = true;
    }
    public void StopJumping()
    {
        wantToJump = false;
    }

    public void SetFullHealthAndMaxResource()
    {
        currentLife = stats.MaxLife;
        currentResource = stats.MaxResource;
    }

    public void AddTalentPoint()
    {
        talentPoints += 1;
        FindUtils.GetTalentSheetGrid().UpdateTalentsPointsRemainingText();
    }

    public int GetTalentPoints()
    {
        return talentPoints;
    }

    public bool RemoveOneTalentPoint()
    {
        if(talentPoints > 0)
        {
            talentPoints--;
            return true;
        }
        return false;
    }

    public void ResetTalentPoints()
    {
        talentPoints = level - 1;
    }


    new void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void InitializeWith(string name, int lv, int tPoints, float expPercent, Resource rsrc, List<Spell> spellList)
    {
        this.CharacterName = name;
        this.level = lv;
        this.experiencePercent = expPercent;
        this.talentPoints = tPoints;

        if (rsrc == null)
        {
            resource = new Mana();
        } else
        {
            resource = rsrc;
        }

        ClearSpells();
        foreach(Spell spell in spellList)
        {
            AddSpell(spell);
        }

        FindUtils.GetSpellBookGrid().UpdateSpellBook();
        this.stats = GetBaseStatsForLevel(lv);
    }

    public void SetXSpeed(float xSpd)
    {
        xSpeed = xSpd;
    }

    public void Cancel()
    {
        if (!Interface.CloseModalDialog() && !InterfaceUtils.CloseOpenWindows() && !CancelCast())
        {
            if (FindUtils.GetPlayer().GetTarget() != null)
            {
                FindUtils.GetPlayer().CancelTarget();
            }
            else
            {
                Interface.OpenCloseMenu();
            }
        }
    }

    void Controls()
    {
        if (Input.GetButtonDown("CycleTargets"))
        {
            CycleTargets();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            Cancel();
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
        if (Input.GetButtonDown("ShowHideTalentSheet"))
        {
            InterfaceUtils.ShowHideTalentSheet();
        }
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            SetXSpeed(Input.GetAxis("Horizontal"));
        } 

        if (Input.GetKeyDown(KeyCode.X))
        {
            LevelUp();
            LevelUp();
            LevelUp();
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
                GetTarget().ApplyDamage(999999, true, true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Interface.Click();
        }

    }

    void Update() 
	{
		UpdateCharacter ();
        Controls();
        if (!IsDead() && !IsStunned())
        {
            MovePlayer(GetComponent<Rigidbody2D>());
        }
	}

    public override void LevelUp()
    {
        if(level < Constants.MaxLevel)
        {
            if(LevelUpAnimation != null)
            {
                GameObject lvlUpAnimInstance = Instantiate(LevelUpAnimation, transform);
                Animator animLvlUp = lvlUpAnimInstance.GetComponent<Animator>();
                animLvlUp.Play("LevelUp");
                Destroy(lvlUpAnimInstance, animLvlUp.GetCurrentAnimatorClipInfo(0)[0].clip.length/1.5f);
            }
            base.LevelUp();
            SetFullHealthAndMaxResource();
            AddTalentPoint();
            
            FindUtils.GetCharacterSheetText().text = "Character\nLevel : " + level;
        }
    }



    public void ClearSpells()
    {
        spellList.Clear();
    }

    public Vector3 GetInitialPosition()
    {
        return initialPosition;
    }

    public override void RemoveSpell(string spellname)
    {
        base.RemoveSpell(spellname);
        if (FindUtils.GetSpellBookGrid() != null)
            FindUtils.GetSpellBookGrid().UpdateSpellBook();

        FindUtils.GetActionBar().Remove(spellname);
    }

    public override void RemoveSpell(Spell spell)
    {
        RemoveSpell(spell.GetName());
    }

    public override void AddSpell(Spell spell)
    {
        base.AddSpell(spell);
        if(FindUtils.GetSpellBookGrid() != null)
            FindUtils.GetSpellBookGrid().UpdateSpellBook();

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

	void OnCollisionEnter2D(Collision2D collision)
	{
		/* on arrete le saut selon un tag
        if(collision.transform.tag == "Ground"){
            jumping = false;
        }*/
		jumping = false;
        if (!IsDead()) // La deuxieme condition est la juste pour l'intro..
        {
            anim.Play("Stand");
        }
    }

	private void MovePlayer(Rigidbody2D player)
    {
        UpdateMoveAnimation(xSpeed);

        if (IsCasting() && (xSpeed > 0.1f || xSpeed < -0.1f))
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
		{ 
			xSpeed = xSpeed * 2;
		}
			
		float ySpeed = player.velocity.y;

		if (wantToJump && !jumping)
		{
			ySpeed = Constants.JumpForce;
			jumping = true;
            anim.Play("Jump",-1,0f);
        }

		player.velocity = new Vector2(xSpeed * stats.MaxSpeed, ySpeed);

        //Limit player to camera bounds At ALL TIMES
        if (isOutOfRightBound() || isOutOfLeftBound() || isOutOfUpDownBounds())
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
	}

    private bool isOutOfUpDownBounds()
    {
        return Camera.main.WorldToViewportPoint(transform.position).y > 1 || Camera.main.WorldToViewportPoint(transform.position).y < 0;
    }

    private bool isOutOfRightBound()
    {
        return Camera.main.WorldToViewportPoint(transform.position).x > 1;
    }

    private bool isOutOfLeftBound()
    {
        return Camera.main.WorldToViewportPoint(transform.position).x < 0;
    }

    protected override void EnterCombat(){
		base.EnterCombat();
		GameObject.Find("Main Camera").SendMessage("leavePlayer");
	}

	protected override void LeaveCombat()
    {
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


