using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

public abstract class Character : MonoBehaviour 
{
    protected int currentLife;
    protected int currentResource;
    public string CharacterName;
	protected bool casting;
	protected float castingTime;
    protected Character target;
	protected bool isDead;
	protected Spell castingSpell;
	public int level;
    public bool isElite;
    protected bool inCombat;
	protected List<Character> enemyList = new List<Character>();
	protected List<EffectOnTime> buffList = new List<EffectOnTime>();
	protected List<EffectOnTime> debuffList = new List<EffectOnTime>();
	protected Dictionary<string, Spell> spellList = new Dictionary<string, Spell> ();
	protected List<Spell> spellsOnCD = new List<Spell>();
	protected Image healthBar;
	protected Resource resource;
	protected bool hasCasted;
	protected Stats stats;
	protected float gcd = 0;
	protected bool autoAttackEnabled = true;
	protected bool autoAttackIsCrit = false;
    protected int autoAttack1Damage = 0;
    protected float autoAttack1Time = 0f;
    protected float autoAttack1Speed = 0f;
    protected int autoAttack2Damage = 0;
    protected float autoAttack2Time = 0f;
    protected float autoAttack2Speed = 0f;
    private Dictionary<string, object> lootTable;
    public List<LootTable> LootTable;
    public List<QuestLootTable> QuestLootTable;
    public List<string> SpellList;
    protected Dialog dialog;
    protected float timeSpendOutOfCombat = 0f;

    public List<AudioClip> aggroSounds;
    public List<AudioClip> attackSounds;
    public List<AudioClip> woundSounds;
    public List<AudioClip> woundCritSounds;
    public List<AudioClip> deathSounds;

    protected Animator anim;


    public void Start()
    {
        gameObject.layer = 9;
        GetComponent<SpriteRenderer>().sortingOrder = 9;
        GetComponent<Rigidbody2D>().freezeRotation = true;
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
        lootTable = new Dictionary<string, object>();
        foreach(LootTable lt in LootTable)
        {
            lootTable.Add(lt.itemName, lt.dropRatePercent);
        }
        foreach (QuestLootTable qlt in QuestLootTable)
        {
            lootTable.Add(qlt.itemName, qlt.questName);
        }
        addSpells(SpellList);

        if (resource == null)
        {
            resource = new Mana();
        }

        stats = GetBaseStatsForLevel(level);
        currentLife = stats.MaxLife;
        currentResource = stats.MaxResource;
        autoAttack1Damage = GetBasicAutoAttackDamage();
        autoAttack1Speed = Constants.BaseAutoAttackSpeed;
        if (lootTable == null || lootTable.Count == 0)
        {
            lootTable = GetDefaultLootTableForLevel(level);
        }

        casting = false;
        isDead = false;
        castingSpell = null;

        anim = GetComponent<Animator>();
    }

    protected Stats GetBaseStatsForLevel(int lvl)
    {
        return new Stats(10 + (Constants.ForceByLevel * (lvl - 1)), 10 + (Constants.AgilityByLevel * (lvl - 1)), 10 + (Constants.IntelligenceByLevel * (lvl - 1)), 10 + (Constants.StaminaByLevel * (lvl - 1)), 10 + (Constants.SpiritByLevel * (lvl - 1)), 5, 0, 0, resource.GetName() == Constants.Mana);
    }

    protected void addSpells(List<string> SpellList)
    {
        foreach (string spellName in SpellList)
        {
            this.AddSpell(Spells.Get(spellName));
        }
    }

    public void AddDialog(string dialogName){
        this.dialog = DatabaseUtils.GetDialog(SceneManager.GetActiveScene().name, dialogName);
    }

	public Dialog GetDialog(){
		return dialog;
	}


    public int GetAutoAttack1Damage()
    {
        return autoAttack1Damage;
    }

    //Used to display stats
    public int GetMinAutoAttack1Damage()
    {
        int autoAttackWithPower = autoAttack1Damage + (int)(autoAttack1Damage * stats.Power / 100);
        return (int)(autoAttackWithPower - autoAttackWithPower*Constants.RandomDamageRange/100);
    }
    public int GetMaxAutoAttack1Damage()
    {
        int autoAttackWithPower = autoAttack1Damage + (int)(autoAttack1Damage * stats.Power / 100);
        return (int)(autoAttackWithPower + autoAttackWithPower * Constants.RandomDamageRange/100);
    }
    public float GetAutoAttack1Speed()
    {
        return modifiedAutoAttackTime(autoAttack1Speed);
    }
    public int GetMinAutoAttack2Damage()
    {
        int autoAttackWithPower = autoAttack2Damage + (int)(autoAttack2Damage * stats.Power / 100);
        return (int)(autoAttackWithPower - autoAttackWithPower * Constants.RandomDamageRange/100);
    }
    public int GetMaxAutoAttack2Damage()
    {
        int autoAttackWithPower = autoAttack2Damage + (int)(autoAttack2Damage * stats.Power / 100);
        return (int)(autoAttackWithPower + autoAttackWithPower * Constants.RandomDamageRange/100);
    }   
    public float GetAutoAttack2Speed()
    {
        return modifiedAutoAttackTime(autoAttack2Speed);
    }

    public bool IsElite()
    {
        return isElite;
    }

    public List<Item> GetLoots()
    {
        List<Item> result = new List<Item>();
        if( lootTable != null) { 
            foreach(KeyValuePair<string, object> kv in lootTable)
            {
                if(kv.Value is int)
                {
                    int intValue = (int)kv.Value;
                    if(Random.Range(0,101) < intValue)
                    {
                        if (kv.Key.Equals("Random"))
                        {
                            result.Add(EquipmentGenerator.GenerateEquipment(level));
                        } else { 
                            result.Add(Items.GetItemFromDB(kv.Key));
                        }
                    } 
                } else if(kv.Value is string)
                {
                    string stringValue = kv.Value.ToString();
                    if (DialogStatus.GetStatus(stringValue + "Started") && !DialogStatus.GetStatus(stringValue + "Ready") && !DialogStatus.GetStatus(stringValue + "Over")) 
                    {
                        result.Add(Items.GetQuestEquipmentFromDB(kv.Key));
                    }
                }
            }
        }
        return result;
    }

    public void SetLootTable(Dictionary<string, object> lootTable)
    {
        this.lootTable = lootTable;
    }

    protected virtual Dictionary<string, object> GetDefaultLootTableForLevel(int level)
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result.Add("Random", 5 + (int)((level / Constants.MaxLevel) * 100));
        result.Add("Haunch of Meat", 20);
        result.Add("Longjaw Mud Snapper", 10);
        result.Add("Ice Cold Milk", 20);
        result.Add("Refreshing Spring Water", 10);
        result.Add("Potion of cunning", 1);
        result.Add("Potion of might", 1);
        result.Add("Potion of deftness", 1);
        if (isElite)
        {
            result["Random"] = 35+(int)((level / Constants.MaxLevel) * 100);
        }
        return result;
    }

    public Dictionary<string, object> GetLootTable()
    {
        return lootTable;
    }

    public void AddLootTable(Dictionary<string, int> lootTable)
    {
        foreach(KeyValuePair<string, int> kv in lootTable)
        {
            this.lootTable.Add(kv.Key, kv.Value);
        }
    }


    public Stats GetStats(){
		return stats;
    }

    public void SetStats(Stats s)
    {
         stats = s;
    }

    public void SwitchAutoAttack(){
		autoAttackEnabled = !autoAttackEnabled;
	}

	public void CancelAutoAttack(){
		autoAttackEnabled = false;
	}

	public void StartAutoAttack(){
		if (target != null) {
			autoAttackEnabled = true;
		}
	}

	public virtual void LevelUp(){
        MessageUtils.Message("Level up !");
        Interface.LevelUp();
        level++;
		this.stats.Add(new Stats(Constants.ForceByLevel, Constants.AgilityByLevel, Constants.IntelligenceByLevel, Constants.StaminaByLevel, Constants.SpiritByLevel, 0,0,0));
        if(FindUtils.GetCharacterSheetGrid().GetEquipmentForSlot(EquipmentSlot.Weapon1) == null)
        {
            autoAttack1Damage = GetBasicAutoAttackDamage();
        }
        this.currentLife = this.GetMaxLife();

	}

    public int GetLevel()
    {
        return level; 
    }

	public List<EffectOnTime> GetBuffs(){
		return buffList;
	}

	public List<EffectOnTime> GetDebuffs(){
		return debuffList;
	}

	public EffectOnTime getEffect(EffectOnTime effect){
		if (effect.IsBuff ()) {
			return buffList.Find (b => b.GetName() == effect.GetName()) ;
		} else {
			return debuffList.Find (b => b.GetName() == effect.GetName()) ;
		}
	}
		
	public void AddEffectOnTime (EffectOnTime effect){
		if (effect.IsBuff ()) {
			buffList.Add (effect);
		} else {
			debuffList.Add (effect);
		}
	}


	public void RemoveEffectOnTime (EffectOnTime effect){
		effect.Remove ();
	}

	public GameObject GetGameObject (){
		return this.gameObject;
	}

	public Character GetTarget(){
		return target;
	}

	public virtual void SetTarget(Character target){
		this.target = target;
	}

    public virtual void CancelTarget()
    {
        if(target != null)
        {
            this.target = null;
        }
    }

    public Spell GetCastingSpell(){
		return castingSpell;
	}

	public float GetCastingTime(){
		return castingTime;
	}

	public int GetCurrentLife(){
		return currentLife;
	}

	public int GetMaxLife(){
		return stats.MaxLife;
	}

    public Resource GetResourceType()
    {
        return resource;
    }

	public int GetCurrentResource(){
		return currentResource;
	}

	public int GetMaxResource(){
		return stats.MaxResource;
	}


	public void RemoveResource (int res){
		currentResource -= res;

	}

	public void AddResource (int res){
		currentResource += res;
	}


	public float GetHealthPercent (){
		return (float)currentLife / (float)stats.MaxLife;
	}
	public float GetResourcePercent (){
		return (float)currentResource / (float)stats.MaxResource;
	}
	public float GetCastPercent (){
		if (casting && castingSpell != null) {
			return (castingTime / castingSpell.GetCastTime (stats));
		} 
		return 0f;
	}
	public bool IsCasting(){
		return casting;
	}
	public bool IsInCombat(){
		return inCombat;
	}

	public bool CancelCast(){
        gcd = 0;
		castingTime = 0;
        bool wasCasting = casting;
		casting = false;
        if(castingSpell != null)
        {
            SoundManager.StopSound(castingSpell.GetPreCastSound());
		    castingSpell = null;
        }
        return wasCasting;

    }

    public void SetAutoAttack1(int damage, float speed)
    {
        autoAttack1Damage = damage;
        autoAttack1Speed = speed;
    }

    public void SetAutoAttack2(int damage, float speed)
    {
        autoAttack2Damage = damage;
        autoAttack2Speed = speed;
    }

    public void ResetAutoAttacks()
    {
        ResetAutoAttack1();
        ResetAutoAttack2();
    }
    public void ResetAutoAttack1()
    {
        autoAttack1Damage = GetBasicAutoAttackDamage();
        autoAttack1Speed = Constants.BaseAutoAttackSpeed;
    }

    public int GetBasicAutoAttackDamage()
    {
        return (int)((Constants.BaseAutoAttackDPS * Constants.BaseAutoAttackSpeed) + level * Constants.AutoAttackDPSPerLevel * Constants.BaseAutoAttackSpeed / Constants.PlayerAutoAttackDivider);
    }

    public void ResetAutoAttack2()
    {
        autoAttack2Damage = 0;
        autoAttack2Speed = 0;
    }



    protected void UpdateCharacter(){
        if (!IsDead())
        {
            UpdateCast();
            UpdateCombat();
            UpdateRegen();
            UpdateEffects();
            UpdateGCD();
            UpdateAutoAttack();
            UpdateCoolDowns();
        }
    }

	void OnMouseDown(){
        if (!IsDead())
        {
            FindUtils.GetPlayer().SetTarget(this);
        }
	}
		

     public void AggroTarget(Character aggroTarget)
     {
        if(aggroTarget != this && !IsDead() && !IsInCombat())
        {
            aggroTarget.AggroFrom(this);
            AddToEnemyList(aggroTarget);
            EnterCombat();
            SoundManager.PlaySound(aggroSounds);
        }
     }
   
   public void AggroFrom(Character aggroFrom)
    {
        if (aggroFrom != this)
        {
            AddToEnemyList(aggroFrom);
            EnterCombat();
        }
     }
      
     protected void AddToEnemyList(Character enemy)
     {
		if (enemy != null && !enemyList.Contains (enemy)) {
			     enemyList.Add (enemy);
			     if (enemyList.Count == 1) {
					SetTarget (enemy);
			     }
		     }
      }
     
	virtual protected void EnterCombat() 
     {
        if (!inCombat)
		{
             StartAutoAttack();
             inCombat = true;
			 createStatusBar ();
         }
     }
     
	virtual protected void LeaveCombat()
     {
		CancelAutoAttack ();
        inCombat = false;
     }

	protected void UpdateCoolDowns(){
		for(int i = spellsOnCD.Count -1; i >= 0; i--){
			Spell s = spellsOnCD [i];
			s.UpdateCoolDown (Time.deltaTime);
			if (s.checkCoolDown ()) {
				spellsOnCD.Remove (s);
			}
		}
	}

	public void addSpellOnCD(Spell s){
		spellsOnCD.Add (s);
	}

    protected void PlayAutoAttackSoundAndAnim(bool isMainWeapon)
    {
        SoundManager.PlaySoundsWithRandomChance(attackSounds, 50);

        string unarmed = "Unarmed";
        string weapType = unarmed;
        if (this is Player)
        {
            Equipment weap;
            if (isMainWeapon)
            {
                weap = FindUtils.GetCharacterSheetGrid().GetEquipmentForSlot(EquipmentSlot.Weapon1);
            } else
            {
                weap = FindUtils.GetCharacterSheetGrid().GetEquipmentForSlot(EquipmentSlot.Weapon2);
            }
            if(weap != null)
            {
                weapType = weap.GetEquipmentType().ToString();
            }
        }


        if (anim != null)
        {
            if (weapType.Equals(unarmed)){

                anim.Play("Attack");
            } else {
                string attackAnim = "Attack";

                if (weapType.Contains("TwoHanded"))
                {
                    attackAnim += "2h";
                } else
                {
                    attackAnim += "1h";
                }

                //Choose from two anims randomly
                if(Random.Range(0,2) == 0)
                {
                    attackAnim += "2";
                }
                anim.Play(attackAnim);
            } 
        }

        SoundManager.PlaySound(DatabaseUtils.GetWeaponAudio(weapType)); 
    }

	protected virtual void UpdateAutoAttack(){
		if (autoAttackEnabled && target != null && !casting)
        {
            if (autoAttack1Damage > 0 && autoAttack1Speed > 0f)
            {
                autoAttack1Time += Time.deltaTime;
                if (autoAttack1Time >= modifiedAutoAttackTime(autoAttack1Speed) && autoAttackDistanceOK())
                {
                    autoAttack1Time = 0;
                    target.ApplyDamage(modifiedAutoAttackDamage(autoAttack1Damage), autoAttackIsCrit, true);
                    PlayAutoAttackSoundAndAnim(true);
                    //Todo Animation Auto Attack 1
                }
            }

            if(autoAttack2Damage > 0 && autoAttack2Speed > 0f)
            {
                autoAttack2Time += Time.deltaTime;
                if (autoAttack2Time >= modifiedAutoAttackTime(autoAttack2Speed) && autoAttackDistanceOK())
                {
                    autoAttack2Time = 0;
                    target.ApplyDamage(modifiedAutoAttackDamage(autoAttack2Damage), autoAttackIsCrit, true);
                    PlayAutoAttackSoundAndAnim(false);
                    
                    //Todo Animation Auto Attack 2
                }
            }
		}
	}

	protected bool autoAttackDistanceOK(){
		return Mathf.Abs ((target.transform.position.x - transform.position.x)) < Constants.MaxAutoAttackDistance;
	}


	protected int modifiedAutoAttackDamage(int autoAttackDamage){
        this.autoAttackIsCrit = stats.Critical > Random.Range (1, 101);

        autoAttackDamage = autoAttackDamage + (autoAttackDamage * stats.Power / 100); //Applying power 
		if (this.autoAttackIsCrit) { // Apply Crit
            autoAttackDamage = autoAttackDamage * 2;
		}
		return (int)(autoAttackDamage + autoAttackDamage * Random.Range (-Constants.RandomDamageRange, Constants.RandomDamageRange) / 100);

	}

	protected float modifiedAutoAttackTime(float autoAttackTime){
		return (autoAttackTime - (autoAttackTime * stats.Haste/Constants.hasteDivider));

	}

	protected void UpdateEffects(){
		updateBufflist (buffList);
		updateBufflist (debuffList);
	}

	protected void updateBufflist(List<EffectOnTime> effectList){
		for (int i = effectList.Count - 1; i >= 0; i--) {
			EffectOnTime effect = effectList [i];
			if (effect.IsToBeRemoved()) {
				effectList.RemoveAt (i);
			}
			else {
				effect.Update ();
			}

		}
	}

	protected void UpdateGCD(){
		if (gcd > 0) {
			gcd -= Time.deltaTime;
		}
	}

	protected void UpdateRegen() {
		if (!(currentResource >= stats.MaxResource)) {
			currentResource += resource.Regen (Time.deltaTime, hasCasted, inCombat, this);
		} 
		if (currentResource > stats.MaxResource){
			currentResource = stats.MaxResource;
		}
		if (currentResource < 0 ) {
			currentResource = 0;
		}
		if (hasCasted) {
			hasCasted = false;
		}
        if (!inCombat)
        {
            timeSpendOutOfCombat += Time.deltaTime;
            if(timeSpendOutOfCombat >= Constants.RegenLifeEvery)
            {
                timeSpendOutOfCombat -= Constants.RegenLifeEvery;
                if(!(currentLife >= GetMaxLife()))
                {
                    currentLife += ((GetMaxLife() * Constants.RegenLifePercent)/100);
                }

                if (currentLife > GetMaxLife())
                {
                    currentLife = GetMaxLife();
                }
                if (currentLife < 0)
                {
                    currentLife = 0;
                }
            }
        }
	}
     
     protected void UpdateCombat ()
    {
        ClearEnemyList();
        if (inCombat) {
			UpdateTarget ();
		} else if (target != null && target.IsDead())
        {
            target = null;
        }
	}

	protected void ClearEnemyList (){ 
		enemyList.RemoveAll (e => e.IsDead ());
	}

	protected void UpdateTarget(){
		if (enemyList.Count >= 1) {
			if (target == null || target.IsDead ()) {
				SetTarget(enemyList [0]);
			}
		}
		else {
			LeaveCombat ();
			target = null;
		}
	}
   
	public bool IsDead(){
		return isDead;
	}

    public void interact()
    {

    }

	void OnDestroy(){
		//Animate ?
	}


    public virtual void die()
    {
        SoundManager.PlaySound(deathSounds);
        if(castingSpell != null)
        {
            SoundManager.StopSound(castingSpell.GetPreCastSound());
        }

		isDead = true;
        CancelTarget();
        CancelCast();

        if (anim != null)
        {
            anim.Play("Death");
            if(!(this is Player))
            {
                StartCoroutine("FadeOut");
            }
        } else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator FadeOut()
    {
        Color spriteColor = transform.GetComponent<SpriteRenderer>().color;
        float alpha = spriteColor.a;

        yield return new WaitForSeconds(Constants.TimeBeforeSpawnOrDespawn);

        while (alpha > 0f) {
            alpha -= Time.deltaTime / Constants.TimeToFadeInOrOutSpawnOrDespawn;
            transform.GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }
    }

    public string GetName()
    {
        return CharacterName;
    }

    public void move()
    {

    }

	public Dictionary<string, Spell> GetSpells(){
		return this.spellList;
	}

    public virtual void AddSpell(Spell spell)
    {
        spellList[spell.GetName()] = spell.Clone();
    }

    public virtual void RemoveSpell(string spellname)
    {
        spellList.Remove(spellname);
    }
    public virtual void RemoveSpell(Spell spell)
    {
        RemoveSpell(spell.GetName());
    }

    public void CastSpell(string spellName)
    {
		CastSpell (spellList [spellName]);
    }

	public void CastSpell(Spell spell){
		if(!casting) 
		{
			castingSpell = spell;
			if (GCDReady() && castingSpell.IsCastable(this,target)) {
				casting = true;
                if (castingSpell.GetCastTime(stats) > 0)
                {
                    SoundManager.PlaySound(castingSpell.GetPreCastSound());
                }
                gcd = Constants.GlobalCooldown - (Constants.GlobalCooldown * stats.Haste / Constants.hasteDivider);
			} else {
				castingSpell = null;
			}
		}
	}

	public bool GCDReady(){
		bool isReady = gcd <= 0;
		//if (!isReady)
			//print ("GCD : " + gcd.ToString("0.0"));
		return isReady;
	}

    protected void UpdateMoveAnimation(float xSpeed)
    {
        if(xSpeed < 0 && transform.localScale.x > 0)
        {
            FlipSprite();
        }
        if(xSpeed > 0 && transform.localScale.x < 0)
        {
            FlipSprite();
        }

        if (anim != null)
        {
            anim.SetBool("IsMoving", xSpeed != 0);
        }
    }

    protected void FlipSprite()
    {
        transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
    }
    
    protected void UpdateCast()
	{
        if (casting)
		{
			castingTime += Time.deltaTime;
            if(castingTime >= castingSpell.GetCastTime(stats))
            {
                 DoneCasting();
            }
        }
    }

    protected void DoneCasting()
    {
        hasCasted = true;
        SoundManager.StopSound(castingSpell.GetPreCastSound());
        castingSpell.Cast (this, target);
        if (anim != null)
        {
            anim.Play("Cast");
        }
        castingTime = 0;
        casting = false;
        castingSpell = null;
    }
		
	public void ApplyDamage (int damage, bool isCrit = false, bool isAutoAttack = false)
	{

        if (!this.GetName().Equals(FindUtils.GetPlayer().GetName()))
        {
           FindUtils.GetDps().AddDamageToDps(damage);
            if (!IsInCombat())
            {
                AggroFrom(FindUtils.GetPlayer());
            }
        }

        if (damage > 0)
        {
            createFloatingText(damage.ToString(), new Color(1, 0, 0), isCrit, isAutoAttack);

            this.currentLife -= damage;

            if (currentLife <= 0)
            {
                currentLife = 0;
                this.die();
            } else
            {
                if (isCrit)
                {
                    SoundManager.PlaySoundsWithRandomChance(woundCritSounds, 70);
                }
                else
                {
                    SoundManager.PlaySoundsWithRandomChance(woundSounds, 30);
                }
            }
        }
	}

	public void ApplyHeal (int heal, bool isCrit = false)
	{
		if (heal > 0) {
			
			this.currentLife += heal;
			createFloatingText (heal.ToString (), new Color (0, 1, 0), isCrit);

			if (currentLife > stats.MaxLife)
				currentLife = stats.MaxLife;

			if (currentLife <= 0)
				this.die();
		}
	}

	protected void createFloatingText (string text, Color color, bool isCrit, bool isAutoAttack = false)
	{
		GameObject floatingTextGameObj = (GameObject)Instantiate (Resources.Load ("Prefab/UI/FloatingText"));
		setObjectAboveAsChild (floatingTextGameObj, 1f);
		FloatingText floatingText = floatingTextGameObj.AddComponent<FloatingText> ();
		floatingText.setText (text);
		floatingText.setColor (color);
		if (isCrit) {
			floatingText.SetCrit();
		}
		if (isAutoAttack) {
			floatingText.setColor (Color.white);
		}
    }

	protected void createStatusBar(){
		GameObject statusBarGameObject = (GameObject)Instantiate (Resources.Load ("Prefab/UI/StatusBar"));
		setObjectAboveAsChild (statusBarGameObject, 0.5f);
		statusBarGameObject.AddComponent<StatusBar>();
	}

	private void setObjectAboveAsChild(GameObject gameObj, float yOffSet){
		gameObj.transform.SetParent (this.gameObject.transform, false);
		gameObj.transform.position += new Vector3 (0,this.gameObject.GetComponent<SpriteRenderer> ().bounds.size.y + yOffSet,0);
	}


}