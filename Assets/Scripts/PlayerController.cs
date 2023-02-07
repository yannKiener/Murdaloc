using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	bool inCombat = false;
	List<GameObject> enemyList;
	GameObject target;

	// Use this for initialization
    void Start()
    {
        Player player = gameObject.AddComponent<Player>();
        player.Initialize("Speaf");

        print(player.GetName()); 
		enemyList = new List<GameObject>();
	}
	
	// Update is called once per frame
	void Update () {

        //EnemyManagement
		if (Input.GetKeyDown ("1")){
			attackTarget (target);
		}
		if (enemyList.Count == 0 && inCombat) {
			leaveCombat ();
		} 


	}


	public void enterCombat (GameObject enemy) {
		if (!inCombat) {
			print ("+combat");
			inCombat = true;
			GameObject.Find ("Main Camera").SendMessage ("leavePlayer");
		}
		if (!enemyList.Contains (enemy)) {
			enemyList.Add (enemy);
			if (enemyList.Count == 1) {
				target = enemy;
			}
		}
	}

	public void leaveCombat () {
		print ("-combat");
		inCombat = false;
		GameObject.Find ("Main Camera").SendMessage ("followPlayer");
	}

	public void attackTarget (GameObject tar) {
		enemyList.Remove (tar);
		Destroy (tar);
		if (enemyList.Count != 0) {
            target = enemyList[0];
		}
	}

	private void limitMapToCamera () {
		GameObject.Find ("Main Camera").SendMessage ("getBoundaries");
		
	}

}
