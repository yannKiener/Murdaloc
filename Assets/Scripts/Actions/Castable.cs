using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Castable
{
    void ApplyTo(Character caster, Character target);
}