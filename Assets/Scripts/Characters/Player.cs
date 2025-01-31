﻿using System;
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
        if (InputManager.IsButtonDown("CycleTargets"))
        {
            CycleTargets();
        }
        if (InputManager.IsButtonDown("Cancel") || Input.GetKeyDown("escape"))
        {
            Cancel();
        }

        if (InputManager.IsButtonDown("ShowHideSpellBook"))
        {
            InterfaceUtils.ShowHideSpellBook();
        }
        if (InputManager.IsButtonDown("ShowHideInventory"))
        {
            InterfaceUtils.ShowHideInventory();
        }
        if (InputManager.IsButtonDown("ShowHideCharacterSheet"))
        {
            InterfaceUtils.ShowHideCharacterSheet();
        }
        if (InputManager.IsButtonDown("ShowHideQuestLog"))
        {
            InterfaceUtils.ShowHideQuestLog();
        }
        if (InputManager.IsButtonDown("ShowHideTalentSheet"))
        {
            InterfaceUtils.ShowHideTalentSheet();
        }
        if (InputManager.IsButtonDown("AstralRecall"))
        {
            CastSpell("Astral Recall");
        }
        if (InputManager.IsButtonDown("BasicAttack"))
        {
            CastSpell("Basic attack");
        }
        if (SystemInfo.deviceType != DeviceType.Handheld)
        {
            if (InputManager.IsButtonPressed("MoveLeft"))
            {
                SetXSpeed(-1);
            }
            else if(InputManager.IsButtonPressed("MoveRight"))
            {
                SetXSpeed(1);
            }
            else
            {
                SetXSpeed(0);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            //For sound only
            Interface.Click();
        }

        //Cheats
        if (Input.GetKeyDown(KeyCode.F1))
        {
            if (GetTarget() != null && GetTarget() is Hostile)
                GetTarget().ApplyDamage(999999, true, true);
        }

        if (Input.GetKeyDown(KeyCode.F2))
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


        if (Input.GetKeyDown(KeyCode.F3))
        {
            DialogActions.DoAction("AddMageSpells");
            DialogActions.DoAction("AddMageSpec");
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            DialogActions.DoAction("AddWarriorSpells");
            DialogActions.DoAction("AddWarriorSpec");
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            DialogActions.DoAction("AddRogueSpells");
            DialogActions.DoAction("AddRogueSpec");
        }

        if (Input.GetKeyDown(KeyCode.F6))
        {
            die();
        }
        //End Cheats


    }


    public override bool OnClick() { return false; }
    public override void OnClickPlayerCloseEnough() { }
    public override void OnPlayerFarOrDead() { }


    public override void OnUpdate() {
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
            if(!IsDead())
            { 
                SetFullHealthAndMaxResource();
            }
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

    public override void SetTarget(Character target)
    {
        this.target = target;
        target.createStatusBar();
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

    private bool isCastingInstant()
    {
        return castingSpell.GetCastTime(stats) < 0.1f;
    }

	private void MovePlayer(Rigidbody2D player)
    {
        UpdateMoveAnimation(xSpeed);

        if (IsCasting() && !isCastingInstant() && (xSpeed > 0.1f || xSpeed < -0.1f))
        {
            CancelCast();
            MessageUtils.ErrorMessage("Can't cast while walking");
        }

        if (InputManager.IsButtonDown("Jump"))
		{
			wantToJump = true;
		}
		if (InputManager.IsButtonUp("Jump"))
		{
			wantToJump = false;
		}
        //Cheat
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
        if (!isDead)
        { 
            base.die ();
            SoundManager.PlaySound(Resources.Load<AudioClip>("Sounds/wasted"));
	        Instantiate (Resources.Load ("Wasted"));
        }
    }
}


