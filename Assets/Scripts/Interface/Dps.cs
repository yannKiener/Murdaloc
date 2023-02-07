using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dps : MonoBehaviour {

    float dps = 0;
    float damageDone = 0;
    float timecounter;

	// Use this for initialization
	void Start () {
		
	}

    public void AddDamageToDps(float number)
    {
        damageDone += number;
        StartCoroutine(RemoveDamageToDps(number, Constants.keepDpsDamageSeconds));
    }

    IEnumerator RemoveDamageToDps(float number, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        damageDone -= number;
    }

    // Update is called once per frame
    void Update () {
        timecounter += Time.deltaTime;

        if (timecounter > 1)
        {
            timecounter -= 1;
            dps = (damageDone / Constants.keepDpsDamageSeconds);
            GetComponent<Text>().text = dps.ToString("0.00");
        }
	}
}
