using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Character : MonoBehaviour 
{
    protected int currentLife;
    protected int currentResource;
    protected string charName;
	protected bool casting;
	protected float castingTime;
    protected Character target;
	protected bool isDead;
	protected Spell castingSpell;
	protected int level;
	protected bool inCombat;
	protected List<Character> enemyList = new List<Character>();
	protected List<EffectOnTime> buffList = new List<EffectOnTime>();
	protected List<EffectOnTime> debuffList = new List<EffectOnTime>();
	protected Dictionary<string, Spell> spellList = new Dictionary<string, Spell> ();
	protected List<Spell> spellsOnCD = new List<Spell>();
	protected Image healthBar;
	protected bool isHealthBarDisplayed = false;
	protected Resource resource;
	protected bool hasCasted;
	protected Stats stats;
	protected float gcd = 0;
	protected bool autoAttackEnabled = true;
	protected bool autoAttackIsCrit = false;
	protected float autoAttackTime = 0f;
    protected Dictionary<string, object> lootTable;
    protected bool isElite;
	protected Dialog dialog;
    protected float timeSpendOutOfCombat = 0f;

    public void Initialize(string name, int level = 1, bool isElite = false, Dictionary<string, object> lootTable = null)
	{
		if (resource == null) {
			resource = new Mana ();
		}
		stats = new Stats (10 + (Constants.ForceByLevel * (level-1)), 10 + (Constants.AgilityByLevel * (level - 1)), 10 + (Constants.IntelligenceByLevel * (level - 1)), 10 + (Constants.StaminaByLevel * (level - 1)), 10 + (Constants.SpiritByLevel * (level - 1)), 5, 0, 0,resource.GetName() == Constants.Mana);
		currentLife = stats.MaxLife;
		currentResource = stats.MaxResource;
        this.charName = name;
		casting = false;
        this.level = level;
		isDead = false;
		castingSpell = null;
        this.isElite = isElite;
        if(lootTable == null)
        {
            lootTable = GetDefaultLootTableForLevel(level);
        }
        this.lootTable = lootTable;
    }
	
	public void AddDialog(string dialogName){
        this.dialog = DatabaseUtils.GetDialog(dialogName);
    }

	public Dialog GetDialog(){
		return dialog;
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
                            result.Add(ItemGenerator.GenerateItem(level));
                        } else { 
                            result.Add(Items.GetItemFromDB(kv.Key));
                        }
                    } 
                } else if(kv.Value is string)
                {
                    string stringValue = kv.Value.ToString();
                    Debug.Log(stringValue);
                    Debug.Log(DialogStatus.GetStatus(stringValue + "Started"));
                    Debug.Log(!DialogStatus.GetStatus(stringValue + "Ready"));
                    if (DialogStatus.GetStatus(stringValue + "Started") && !DialogStatus.GetStatus(stringValue + "Ready")) 
                    {
                        //TODOO : Vérifier par l'objectif de la quête avec un truc genre "GetObjectiveWithItem(string itemName)" si c'est lootable ou non
                        result.Add(Items.GetQuestItemFromDB(kv.Key));
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
        result.Add("Random", (int)((level / Constants.MaxLevel) * 100));
        if (isElite)
        {
            result.Add("Random",30+(int)((level / Constants.MaxLevel) * 100));
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
        level++;
		this.stats.Add(new Stats(Constants.ForceByLevel, Constants.AgilityByLevel, Constants.IntelligenceByLevel, Constants.StaminaByLevel, Constants.SpiritByLevel, 0,0,0));
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
            cakeslice.Outline outline = this.target.GetGameObject().GetComponent<cakeslice.Outline>();
            Destroy(outline);
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
		castingSpell = null;
        return wasCasting;

    }

    

	protected void UpdateCharacter(){
		UpdateCast();
		UpdateCombat();
		UpdateRegen();
		UpdateEffects ();
		UpdateGCD ();
		UpdateAutoAttack ();
		UpdateCoolDowns ();
	}

	void OnMouseDown(){
		FindUtils.GetPlayer().SetTarget (this);
	}
		

     public void AggroTarget(Character aggroTarget)
     {
          aggroTarget.AggroFrom(this);
          AddToEnemyList(aggroTarget);
          EnterCombat();
     }
   
   public void AggroFrom(Character aggroFrom)
     {
         AddToEnemyList(aggroFrom);
         EnterCombat();
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

	protected virtual void UpdateAutoAttack(){
		if (autoAttackEnabled && target != null && !casting) {
			autoAttackTime += Time.deltaTime;
			if (autoAttackTime >= modifiedAutoAttackTime () && autoAttackDistanceOK()) {
				autoAttackTime = 0;
				target.ApplyDamage (modifiedAutoAttackDamage (), autoAttackIsCrit,true);
				//Todo Animation Auto Attack
			}
		}
	}

	protected bool autoAttackDistanceOK(){
		return Mathf.Abs ((target.transform.position.x - transform.position.x)) < Constants.MaxAutoAttackDistance;
	}


	protected int modifiedAutoAttackDamage(){
		int damage = stats.AutoAttackDamage;
		this.autoAttackIsCrit = stats.Critical > Random.Range (1, 101);

		damage = damage + (damage * stats.Power / 100); //Applying power 
		if (this.autoAttackIsCrit) { // Apply Crit
			damage = damage * 2;
		}
		return (int)(damage + damage * Random.Range (-30f, 30f) / 100);

	}

	protected float modifiedAutoAttackTime(){
		return (stats.AutoAttackTime - (stats.AutoAttackTime * stats.Haste/Constants.hasteDivider));

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
     
     protected void UpdateCombat (){
		if (inCombat) {
			ClearEnemyList ();
			UpdateTarget ();
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
		isDead = true;
		gameObject.SetActive (false);
        //GameObject.Destroy(this.gameObject);
    }

    public string GetName()
    {
        return charName;
    }

    public void move()
    {

    }

	public Dictionary<string, Spell> GetSpells(){
		return this.spellList;
	}

    public virtual void AddSpell(Spell spell)
    {
        spellList.Add(spell.GetName(), spell);
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
				gcd = Constants.GlobalCooldown - (Constants.GlobalCooldown * stats.Haste / Constants.hasteDivider);
			} else {
				castingSpell = null;
			}
		}
	}

	protected bool GCDReady(){
		bool isReady = gcd <= 0;
		//if (!isReady)
			//print ("GCD : " + gcd.ToString("0.0"));
		return isReady;
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
		castingSpell.Cast (this, target);
        castingTime = 0;
        casting = false;
        castingSpell = null;
    }
		
	public void ApplyDamage (int damage, bool isCrit = false, bool isAutoAttack = false)
	{
		if (damage > 0) {

			this.currentLife -= damage;
			createFloatingText (damage.ToString (), new Color (1, 0, 0), isCrit, isAutoAttack);

			if (currentLife <= 0) {
				currentLife = 0;
				this.die();
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
		GameObject floatingTextGameObj = (GameObject)Instantiate (Resources.Load ("FloatingText"));
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
		GameObject statusBarGameObject = (GameObject)Instantiate (Resources.Load ("StatusBar"));
		setObjectAboveAsChild (statusBarGameObject, 0.5f);
		statusBarGameObject.AddComponent<StatusBar>();
	}

	private void setObjectAboveAsChild(GameObject gameObj, float yOffSet){
		gameObj.transform.SetParent (this.gameObject.transform, false);
		gameObj.transform.position += new Vector3 (0,this.gameObject.GetComponent<SpriteRenderer> ().bounds.size.y + yOffSet,0);

	}


}