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
    void interact();
    void castSpell(string spellName);
    void addSpell(Spell spell);
	void ApplyDamage (int damage);
	void ApplyHeal (int heal);
	void RemoveResource (int res);
	void AddResource (int res);
	void AggroTarget(Character aggroTarget);
	void AggroFrom(Character aggroFrom);
	void CancelCast();
	GameObject GetGameObject();

}



public abstract class AbstractCharacter : MonoBehaviour, Character 
{
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
			resource = new Mana ();
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
             GameObject.Find("Main Camera").SendMessage("leavePlayer");
         }
     }
     
     protected void LeaveCombat()
     {
         inCombat = false;
         GameObject.Find("Main Camera").SendMessage("followPlayer");
     }

	protected void UpdateRegen() {
		currentResource += resource.Regen (Time.deltaTime, hasCasted);
		if (currentResource > maxResource) {
			currentResource = maxResource;
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

    public void addSpell(Spell spell)
    {
		//print(spell.GetName());
        spellList.Add(spell.GetName(), spell);
    }

    public void castSpell(string spellName)
    {
        if(!casting) 
        {
        	castingSpell = spellList [spellName];
			int resCost = castingSpell.GetResourceCost ();
			if (resCost <= currentResource) {
				casting = true;
			} else {
				castingSpell = null;
				print ("Not enough " + resource.GetName());
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

		updateHealthBar ();

		if (currentLife <= 0)
			this.kill ();
	}

	public void ApplyHeal (int heal)
	{
		this.currentLife += heal;

		updateHealthBar ();

		if (currentLife <= 0)
			this.kill ();
	}

	protected void updateHealthBar(){
		float healthPercent = (float)currentLife / (float)maxLife;

		if (!isHealthBarDisplayed && healthBar == null) {
			GameObject healthBarGameObject = (GameObject)Instantiate (Resources.Load ("HealthBar"));
			healthBarGameObject.transform.SetParent (this.gameObject.transform, false);
			healthBarGameObject.transform.position += new Vector3 (0,1,0);
			healthBar = healthBarGameObject.transform.GetChild (0).GetChild (0).gameObject.GetComponent<Image> ();
			healthBar.fillAmount = healthPercent;
			isHealthBarDisplayed = true;
		} else {
			healthBar.fillAmount = healthPercent;
		}
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
	private int enemyOffset = 0;
	
	void Start(){

	}

	void Update() 
	{
		UpdateCombat ();
		UpdateCast();
		UpdateRegen ();

        //EnemyManagement
		if(Input.GetKeyUp(KeyCode.Tab)){
			CycleTargets();
		}
		if (Input.GetKeyDown (KeyCode.Escape)) {
			CancelCast ();
		}
		if (Input.GetKeyDown(KeyCode.Alpha1))
        {
			castSpell(actionBar[0]);
		}
		if (Input.GetKeyDown(KeyCode.Alpha2))
        {
		    castSpell(actionBar[1]);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			castSpell (actionBar [2]);
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
		GUI.Box (new Rect (0, 20, 200, 20), currentLife + " / " + maxLife);
		GUI.Box(new Rect(0,20,currentLife*2,20), new Texture2D(1,1)); 
		GUI.Box (new Rect (0, 40, 200, 20), currentResource + " / " + maxResource);
		GUI.Box(new Rect(0,40,currentResource*2,20), new Texture2D(1,1)); 


		if (target != null && !target.IsDead()) {
			GUI.Box (new Rect (400, 0, 200, 20), target.GetName());
			GUI.Box (new Rect (400, 20, 200, 20), target.GetCurrentLife() + " / " + target.GetMaxLife());
			GUI.Box (new Rect (400, 20, target.GetCurrentLife()*2, 20), new Texture2D(1,1));
			GUI.Box (new Rect (400, 40, 200, 20), target.GetCurrentResource() + " / " + target.GetMaxResource());
			GUI.Box(new Rect(400,40,target.GetCurrentResource()*2,20), new Texture2D(1,1)); 

			//test outlining target
			if (target.GetGameObject ().GetComponent<cakeslice.Outline> () == null) {
				target.GetGameObject ().AddComponent<cakeslice.Outline> ();
			}
		}

		if (casting) {
			int spellCastTime = (int)castingSpell.GetCastTime ();
			int castPercentage = (int)(100 * (castingTime/spellCastTime));

			GUI.Box (new Rect (Screen.width/2, 93	*Screen.height/100, 100, 30), castingSpell.GetName() + " : " +castingTime.ToString("0.0") + " / " + spellCastTime);
			GUI.Box(new Rect(Screen.width/2, 93*Screen.height/100,castPercentage,30), new Texture2D(1,1)); 
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

}



[System.Serializable]
public class Hostile : AbstractCharacter
{
	
    void Update()
    {
        limitMovements();
        if (!inCombat)
        {
            AggroAroundSelf();
        }
    }
    
    void AggroAroundSelf()
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

					//Todo : Supprimer ca plus tard c'est juste pour la déco
					GameObject aggroSprite = Instantiate(Resources.Load("AggroSprite")) as GameObject;
					aggroSprite.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.GetComponent<BoxCollider2D>().bounds.size.y);
					StartCoroutine(DeleteObjectAfterSeconds(aggroSprite, 0.15f));
                 }
                 i++;
             }  
        }
    }
	
	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Player" && !inCombat) {
            inCombat = true;
            //Aggro(collision);
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

	/*
    private void Aggro(Collision2D collision)
    {
            inCombat = true;
            collision.gameObject.SendMessage("enterCombat", this.gameObject);

            //TODO : Supprimer ca et trouver un moyen plus classe pour afficher l'aggro :D
            GameObject aggroSprite = Instantiate(Resources.Load("AggroSprite")) as GameObject;
            aggroSprite.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + this.GetComponent<BoxCollider2D>().bounds.size.y);
            StartCoroutine(DeleteObjectAfterSeconds(aggroSprite, 0.15f));
    } 
    */

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
