using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Character 
{
    void kill();
    string GetName();
    void move();
	bool IsDead();
    void interact();
    void castSpell(string spellName);
    void addSpell(Spell spell);
	void ApplyDamage (int damage);
	void AggroTarget(Character aggroTarget);
	void AggroFrom(Character aggroFrom);

}



public abstract class AbstractCharacter : MonoBehaviour, Character 
{
    protected int maxLife;
    protected int currentLife;
    protected int maxMana;
    protected int currentMana;
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

    public void Initialize(string name)
    {
        maxLife = 100;
        currentLife = maxLife;
        maxMana = 100;
        currentMana = maxMana;
        this.charName = name;
		casting = false;
		isDead = false;
		castingSpell = null;
    }
    
    
    void Update() {
         UpdateCast();
         UpdateCombat();
    }

     public void AggroTarget(Character aggroTarget)
     {
          AggroTarget.AggroFrom(this);
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
         inCombat = true;
     }
     
     protected void LeaveCombat()
     {
         inCombat = false;
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
		if (enemyList.Count >= 1)
			target = enemyList [0];
		else {
			leaveCombat ();
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

    public void addSpell(Spell spell)
    {
		print(spell.GetName());
        spellList.Add(spell.GetName(), spell);
    }

    public void castSpell(string spellName)
    {
        if(!casting) 
        {
        castingSpell = spellList [spellName];
        casting = true;
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
    
    protected void CancelCast()
    { 
    castingSpell = null;
    castingTime = 0;
    casting = false;
    }
    
    protected void DoneCasting()
    {
    if (target != null) {
			castingSpell.Cast (this, target);
		} else {
			print ("NO TARGET");
		}
		CancelCast();
    }
		
	public void ApplyDamage (int damage)
	{
		print ("Applying damage : " + damage + " to : " + this.GetName());
		this.currentLife -= damage;

		if (currentLife <= 0)
			this.kill ();
	}


}



[System.Serializable]
public class Player : AbstractCharacter
{

    private float MAXSPEED = 10f;	
    private float JUMPFORCE = 5f;
    private bool jumping = false;
    private bool wantToJump = false;
	private string[] actionBar = new string[5];
	
	
	void Start(){

	}

    void Update()
    {

        //EnemyManagement
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			castSpell(actionBar[0]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
        {
		    castSpell(actionBar[1]);
		}
		UpdateCombat ();
        MovePlayer(GetComponent<Rigidbody2D>()); 
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
	

	private override void EnterCombat () {
		if (!inCombat) {
		    inCombat = true;
			GameObject.Find ("Main Camera").SendMessage ("leavePlayer");
		}
	}

	private override void LeaveCombat () {
		inCombat = false;
		GameObject.Find ("Main Camera").SendMessage ("followPlayer");
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

}



[System.Serializable]
public class Hostile : AbstractCharacter
{
	
    private bool inCombat = false;
	
    void Update()
    {
        limitMovements();
    }
    
    AggroAroundSelf()
    {
        if (!inCombat) 
        {
           //gerer la distance selon le level?
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 1f);
                int i = 0;
             while (i < hitColliders.Length)
            {
                 if (hitColliders[i].tag == "Player")
                 {
                     AggroTarget(hitColliders[i].gameObject.GetComponent<Character>());
                 }
                 i++;
             }  
        }
    }
	
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Player" && !inCombat) {
            inCombat = true;
            Aggro(collision);
            AggroAroundSelf(collision);
		}
	}
	
	
    private void AggroAroundSelf(Collision2D collision)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), 1f);        
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (hitColliders[i].tag == "Enemy")
            {
                hitColliders[i].SendMessage("Aggro", collision);
            }

            i++;
        }
    }
	

    private void limitMovements() {
        if (inCombat)
        {
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp01(pos.x);
            pos.y = Mathf.Clamp01(pos.y);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
        }
    }

    private void Aggro(Collision2D collision)
    {
            inCombat = true;
            collision.gameObject.SendMessage("enterCombat", this.gameObject);

            //TODO : Supprimer ca et trouver un moyen plus classe pour afficher l'aggro :D
            GameObject aggroSprite = Instantiate(Resources.Load("AggroSprite")) as GameObject;
            aggroSprite.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.GetComponent<BoxCollider2D>().bounds.size.y);
            StartCoroutine(DeleteObjectAfterSeconds(aggroSprite, 0.15f));
    }

    IEnumerator DeleteObjectAfterSeconds(GameObject obj, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Destroy(obj);
    }
	
}

[System.Serializable]
public class Friendly : AbstractCharacter
{

}
