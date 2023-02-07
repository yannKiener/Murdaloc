using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Character 
{
    void kill();
    string GetName();
	int GetMaxLife();
	int GetCurrentLife();
	int GetMaxResource();
	int GetCurrentResource();
    void move();
	bool IsDead();
	bool IsCasting();
	bool IsInCombat();
    void interact();
	void CastSpell(string spellName);
	void CastSpell(Spell spell);
    void AddSpell(Spell spell);
	void ApplyDamage (int damage);
	void ApplyHeal (int heal);
	void RemoveResource (int res);
	void AddResource (int res);
	void AggroTarget(Character aggroTarget);
	void AggroFrom(Character aggroFrom);
	void CancelCast();
	float GetHealthPercent ();
	float GetResourcePercent ();
	float GetCastPercent ();
	GameObject GetGameObject();

}



public abstract class AbstractCharacter : MonoBehaviour, Character 
{
	protected float MAXSPEED = 8f;	
	protected float JUMPFORCE = 5f;

    protected int maxLife;
    protected int currentLife;
    protected int currentResource;
	protected int maxResource;
    protected string charName;
	protected bool casting;
	protected float castingTime;
    protected Character target;
	protected bool isDead;
	protected Spell castingSpell;
	protected int level;
	protected bool inCombat;
	protected List<Character> enemyList = new List<Character>();
	protected Dictionary<string, Spell> spellList = new Dictionary<string, Spell> ();
	protected Image healthBar;
	protected bool isHealthBarDisplayed = false;
	protected Resource resource;
	protected bool hasCasted = false;


    public void Initialize(string name)
    {
        maxLife = 100;
        currentLife = maxLife;
        maxResource = 100;
        currentResource = maxResource;
        this.charName = name;
		casting = false;
		isDead = false;
		castingSpell = null;
		if (resource == null) {
			resource = new Energy ();
		}
    }

	public GameObject GetGameObject (){
		return this.gameObject;
	}

	public int GetCurrentLife(){
		return currentLife;
	}

	public int GetMaxLife(){
		return maxLife;
	}

	public int GetCurrentResource(){
		return currentResource;
	}

	public int GetMaxResource(){
		return maxResource;
	}


	public void RemoveResource (int res){
		currentResource -= res;

	}

	public void AddResource (int res){
		currentResource += res;
	}


	public float GetHealthPercent (){
		return (float)currentLife / (float)maxLife;
	}
	public float GetResourcePercent (){
		return (float)currentResource / (float)maxResource;
	}
	public float GetCastPercent (){
		if (casting && castingSpell != null) {
			return (castingTime / castingSpell.GetCastTime ());
		} 
		return 0f;
	}
	public bool IsCasting(){
		return casting;
	}
	public bool IsInCombat(){
		return inCombat;
	}



	public void CancelCast(){
		castingTime = 0;
		casting = false;
		castingSpell = null;
	}

    
    
    void Update() {
         UpdateCast();
         UpdateCombat();
		 UpdateRegen();
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
				     target = enemy;
			     }
		     }
      }
     
     protected void EnterCombat() 
     {
         if (!inCombat)
         {
             inCombat = true;
			 createStatusBar ();
             GameObject.Find("Main Camera").SendMessage("leavePlayer"); //TODO : Connerie mettre ca que au player et pas aux mobs..!!
         }
     }
     
     protected void LeaveCombat()
     {
         inCombat = false;
		 GameObject.Find("Main Camera").SendMessage("followPlayer"); //TODO : Connerie mettre ca que au player et pas aux mobs..!!
     }

	protected void UpdateRegen() {
		currentResource += resource.Regen (Time.deltaTime, hasCasted, inCombat);
		if (currentResource > maxResource) {
			currentResource = maxResource;
		}
		if (currentResource < 0 ) {
			currentResource = 0;
		}
		if (hasCasted) {
			hasCasted = false;
		}
	}
     
     protected void UpdateCombat (){
		if (inCombat) {
			ClearEnemyList ();
			UpdateTarget ();
		}
	}

	protected void ClearEnemyList (){
	//todo : a appeller uniquement quand y'a un.mort
		enemyList.RemoveAll (e => e.IsDead ());
	}

	protected void UpdateTarget(){
		if (enemyList.Count >= 1) {
			if (target == null || target.IsDead ()) {
				target = enemyList [0];
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


    public void kill()
    {
		isDead = true;
        GameObject.Destroy(this.gameObject);
    }

    public string GetName()
    {
        return charName;
    }

    public void move()
    {

    }

    public void AddSpell(Spell spell)
    {
		//print(spell.GetName());
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
			if (castingSpell.IsCastable(this,target)) {
				casting = true;
			} else {
				print ("I can't cast "+ castingSpell.GetName() +" now.");
				castingSpell = null;
			}
		}

	}
    
    protected void UpdateCast()
	{
        if (casting)
		{
			castingTime += Time.deltaTime;
            if(castingTime >= castingSpell.GetCastTime())
            {
                 DoneCasting();
            }
        }
    }

    protected void DoneCasting()
    {
    if (target != null) {
			hasCasted = true;
			castingSpell.Cast (this, target);
		} else {
			print ("NO TARGET");
		}
		CancelCast();
    }
		
	public void ApplyDamage (int damage)
	{
		this.currentLife -= damage;

		if (currentLife <= 0)
			this.kill ();
	}

	public void ApplyHeal (int heal)
	{
		this.currentLife += heal;

		if (currentLife > maxLife) 
			currentLife = maxLife;

		if (currentLife <= 0)
			this.kill ();
	}

	protected void createStatusBar(){
		GameObject statusBarGameObject = (GameObject)Instantiate (Resources.Load ("StatusBar"));
		statusBarGameObject.transform.SetParent (this.gameObject.transform, false);
		float spriteHeight = this.gameObject.GetComponent<SpriteRenderer> ().bounds.size.y;
		statusBarGameObject.transform.position += new Vector3 (0,spriteHeight + 0.5f,0);
		statusBarGameObject.AddComponent<StatusBar>();
	}

}





[System.Serializable]
public class Friendly : AbstractCharacter
{

}
