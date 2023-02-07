using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour {

    bool inCombat = false;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

        limitMovements();
		
	}

	void OnCollisionEnter2D(Collision2D collision){
		if (collision.gameObject.tag == "Player" && !inCombat) {
            inCombat = true;
            Aggro(collision);
            AggroAroundSelf(collision);
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
