using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Hostile : AbstractCharacter
{
	private int direction = 0;

	void Start(){
		InvokeRepeating ("randomizeDirection", 1f, 1f);
	}

	void Update()
	{
		move ( GetComponent<Rigidbody2D>());
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


	private void move(Rigidbody2D mob){
		if (!casting) {
			if (inCombat) {
				float targetPos = target.GetGameObject ().transform.position.x;
				float selfPos = this.gameObject.transform.position.x;
				if (targetPos + 1f < selfPos) {
					direction = -1;
				} else if (targetPos - 1f > selfPos) {
					direction = 1;
				} else {
					direction = 0;
				}

			}

			mob.velocity = new Vector2 (direction * MAXSPEED/3, mob.velocity.y);

		}

	}

	private void randomizeDirection(){
		int percentage = getRandomPercentage ();

		if (percentage <= 20) { // 20% de chances qu'il s'arrête.
			direction = 0;
		} else if (percentage > 40)	 { // Sinon 60% de chances que la direction soit randomisée
			direction = Random.Range (-1, 2);
		}  // et 20% de chances qu'il continue la même direction.
	}

	private int getRandomPercentage(){
		return Random.Range (0, 101);
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


	IEnumerator DeleteObjectAfterSeconds(GameObject obj, float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		Destroy(obj);
	}

}