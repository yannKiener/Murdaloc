using System.Collections;
using System.Collections.Generic;

public class StunEffect : Effect
{
    public void apply(Character caster, Character target, int stacks)
    {
        target.Stun();
    }

    public void remove(Character caster, Character target, int stacks)
    {
        target.RemoveStun();
    }
    
}