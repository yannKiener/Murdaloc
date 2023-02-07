using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect {

	void applyOnce (Character caster, Character target);
	void removeOnce (Character caster, Character target);
	void tic (Character caster, Character target);

}

public class StatModifier : Effect{

	public void applyOnce(Character caster, Character target){

	}

	public void removeOnce (Character caster, Character target){

	}

	public void tic (Character caster, Character target){

	}

}