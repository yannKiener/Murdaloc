using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




    public abstract class effectOnTime
    {
        protected float duration;
        protected float timeLeft;
        protected float timePerTic;
        protected float nextTic;
        protected int totalDamage;
        protected int totalHeal;
        protected string name;
        protected string description;
        

        private void Apply()
        {
            timeLeft = duration;
            nextTic = duration - timePerTic;
        }

        private void Remove()
        {

        }

        private void Tic()
        {

        }

        private void Update()
        {
            if (timeLeft < 0 || isEffectOver())
            {
                Remove();
            }
            else
            {
                if (timeLeft < nextTic)
                {
                    Tic();
                    nextTic -= timePerTic;
                }
            }

        }

        private bool isEffectOver()
        {
            return false;
        }
            
    }


    public class buff : effectOnTime
    {
        buff(float duration, float timePerTic)
        {
            this.timePerTic = timePerTic;
            this.duration = duration;
        }
    
    
    }

    public class debuff : effectOnTime
    {
        debuff()
        {
        }


    }


}
