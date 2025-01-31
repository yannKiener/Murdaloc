﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Hostile : Character
{
	private int direction = 0;
    public bool moveRandom = true;
    public bool isPassive = false;

    private float timeBetweenCast = 0;

    new void Start(){
        base.Start();
		this.gameObject.tag = "Enemy";
        this.gameObject.layer = 9;
        RespawnReady();

        stats.AddStat(Stat.autoAttackDamage, level * Constants.AutoAttackDPSPerLevel * Constants.BaseAutoAttackSpeed);

        if (isElite)
        {
            stats.Add(stats);
            stats.AddPercent(Stat.stamina, Constants.EliteStaminaBonusPercent);
            stats.AddPercent(Stat.force, Constants.EliteStatsBonusPercent);
            stats.AddPercent(Stat.agility, Constants.EliteStatsBonusPercent);
            stats.AddPercent(Stat.intelligence, Constants.EliteStatsBonusPercent);
            stats.AddPercent(Stat.spirit, Constants.EliteStatsBonusPercent);
            stats.AddPercent(Stat.autoAttackDamage, Constants.EliteStatsBonusPercent);
            currentLife = stats.MaxLife;
        }

        if(statPercent != 100)
        {
            stats.UpdatePercentToAll(statPercent);
        }
        if(speedPercent != 100)
        {
            stats.MaxSpeed = stats.MaxSpeed * speedPercent / 100;
        }

        autoAttack1Damage = stats.AutoAttackDamage;
        autoAttack1Speed = stats.AutoAttackTime;

        this.autoAttack1Damage = (int)(autoAttack1Damage * Constants.MobAutoAttackMultiplier);
    }


    public override void OnUpdate() {

        UpdateCharacter();
        if (!IsStunned())
        {
            manageCombat();
        }
        move(GetComponent<Rigidbody2D>());
        limitCombatMovements();
	}

    public bool IsPassive()
    {
        return isPassive;
    }

	void AggroAroundSelf()
	{
		if (!inCombat) 
		{
			//gerer la distance selon le level?
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), Constants.AggroDistance);
			int i = 0;
			while (i < hitColliders.Length)
			{
                if (hitColliders[i].tag == "Player")
                {
                    Character player = hitColliders[i].gameObject.GetComponent<Character>();
                    if (!player.IsDead())
                    {
                        AggroTarget(player);

                        //Todo : Supprimer ca plus tard c'est juste pour la déco
                        GameObject aggroSprite = Instantiate(Resources.Load("AggroSprite")) as GameObject;
                        aggroSprite.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.GetComponent<BoxCollider2D>().bounds.size.y);
                        StartCoroutine(DeleteObjectAfterSeconds(aggroSprite, 0.15f));
                        AggroOthers(player);
                    }
				}
				i++;
			}  
		} 
	}

    private bool HasWaitedEnoughTimeToCast()
    {
        return timeBetweenCast >= Constants.MinimumTimeBetweenCast;
    }
    
    private Spell GetRandomSpell()
    {
        string randomSpellName = spellList.Keys.ElementAt(Random.Range(0, spellList.Count));
        return spellList[randomSpellName];
    }

	private void manageCombat()
    {
        timeBetweenCast += Time.deltaTime;

        if (inCombat && spellList.Count != 0 && !casting && HasWaitedEnoughTimeToCast()) {
			int castPercentage = getRandomPercentage ();
			if (castPercentage < 1) { //1% de chance par frame de cast un spell random*
                Spell spellToCast = GetRandomSpell();
                CastSpell(spellToCast, false);
                timeBetweenCast = 0 - spellToCast.GetCastTime(stats);
            }
		}
	}


	private void move(Rigidbody2D mob){
		if (!casting && !IsDead() && !IsStunned()) {
            float speed = stats.MaxSpeed * Constants.EnemyRoamSpeedMultiplier;

            if (inCombat) {
				float targetPos = target.GetGameObject ().transform.position.x;
				float selfPos = this.gameObject.transform.position.x;
				if (targetPos + Constants.MaxAutoAttackDistance < selfPos) {
					direction = -1;
				} else if (targetPos - Constants.MaxAutoAttackDistance > selfPos) {
					direction = 1;
				} else {
                    UpdateTargetingDirection(targetPos);
                    direction = 0;
				}
                speed = stats.MaxSpeed * Constants.EnemyCombatSpeedMultiplier;
            }

            UpdateMoveAnimation(direction);
            mob.velocity = new Vector2 (direction * speed, mob.velocity.y);
		} else
        {
            mob.velocity = new Vector2(0, mob.velocity.y);
            UpdateMoveAnimation(0);
        }
	}

    private void UpdateTargetingDirection(float targetPosX)
    {

        if (targetPosX < transform.position.x && transform.localScale.x > 0)
        {
            FlipSprite();
        }

        if (targetPosX > transform.position.x && transform.localScale.x < 0)
        {
            FlipSprite();
        }
    }
		
	private void randomizeDirection(){
		int percentage = getRandomPercentage ();

		if (percentage <= 30) { // 30% de chances qu'il s'arrête.
			direction = 0;
		} else if (percentage > 50)	 { // Sinon 50% de chances que la direction soit randomisée
			direction = Random.Range (-1, 2);
		}  // et 20% de chances qu'il continue la même direction.
	}

	private int getRandomPercentage(){
		return Random.Range (0, 101);
	}


	private void AggroOthers(Character charact)
	{
        if (!IsPassive())
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), Constants.AggroOtherDistance);
            int i = 0;
            while (i < hitColliders.Length)
            {
                if (hitColliders[i].tag == "Enemy")
                {
                    Hostile enemy = hitColliders[i].GetComponent<Hostile>();
                    if (!enemy.IsPassive())
                    {
                        enemy.AggroTarget(charact);
                    }
                   
                }

                i++;
            }
        }
	}

    protected override void EnterCombat()
    {
        if (!inCombat)
        {
            StartAutoAttack();
            inCombat = true;
            createStatusBar();  
            AggroOthers(FindUtils.GetPlayer());
        }
    }


	private void limitCombatMovements() {
		if (inCombat)
		{
			Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
			pos.x = Mathf.Clamp01(pos.x);
			pos.y = Mathf.Clamp01(pos.y);
			transform.position = Camera.main.ViewportToWorldPoint(pos);
		}
	}

    public override void die()
    {
        Player player = FindUtils.GetPlayer();
        float playerLevel = (float)player.GetLevel();
        float exp = calculateExpGiven(playerLevel);
        float ratioForNextLevel = exp / calculateExpGiven(playerLevel + 1);
        if (isElite)
        {
            exp = exp * 2.5f;
        }
        player.AddExp(exp, ratioForNextLevel);

        Quests.UpdateTrackedQuests(this);

        List<Item> itemsLoot = GetLoots();
        if (itemsLoot != null && itemsLoot.Count > 0 && !itemsLoot.Contains(null))
        {
            GameObject lootBox = GameObject.Instantiate(Resources.Load<GameObject>("Prefab/LootBox"));
            lootBox.GetComponent<Loot>().Initialize(itemsLoot, new Vector3(transform.position.x, transform.position.y,-2));
        }

        if (moveRandom)
        {
            CancelInvoke("randomizeDirection");
        }
        if (!IsPassive())
        {
            CancelInvoke("AggroAroundSelf");
        }
        direction = 0;
        Invoke("Respawn", Constants.RespawnTimer);
        base.die();
    }

    private void Respawn()
    {
        this.isDead = false;
        if(anim != null)
        {
            anim.Play("Stand");
        }
        currentLife = stats.MaxLife;
        Invoke("RespawnReady", Constants.TimeToFadeInOrOutSpawnOrDespawn + Constants.TimeBeforeSpawnOrDespawn);
        StartCoroutine("FadeIn");
    }

    private void RespawnReady()
    {
        if (moveRandom)
        {
            InvokeRepeating("randomizeDirection", 1f, 1f);
        }
        if (!IsPassive())
        {
            InvokeRepeating("AggroAroundSelf", 1f, 0.5f);
        }
    }


    IEnumerator FadeIn()
    {
        Color spriteColor = transform.GetComponent<SpriteRenderer>().color;
        float alpha = spriteColor.a;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime / Constants.TimeToFadeInOrOutSpawnOrDespawn;
            transform.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
    }



    private float calculateExpGiven(float playerLevel)
    {
        return (level / ((float)playerLevel * playerLevel)) * (Constants.BaseExp - (playerLevel / (float)Constants.MaxLevel)); 
    }


    IEnumerator DeleteObjectAfterSeconds(GameObject obj, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		Destroy(obj);
	}

    public override bool OnClick()
    {
        base.OnClick();
        return false;
    }




    public override void OnClickPlayerCloseEnough() { }
    public override void OnPlayerFarOrDead() { }



}