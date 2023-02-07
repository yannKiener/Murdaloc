using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Effect {

	void apply(Character caster, Character target, int stacks);
	void remove(Character caster, Character target, int stacks);

}
