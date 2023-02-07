using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
     public class Spell {

        private string name;
        private string description;
        private int resourceCost;
        private float castTime;


        public string GetName() {
            return name;
        }
        public string GetDescription() {
            return description;
        }
        public int GetResourceCost() {
            return resourceCost;
        }
        public float GetCastTime() {
            return castTime;
        }

        void Cast(){
            CheckCondition();

        }

        private bool CheckCondition(){


        }


    }
