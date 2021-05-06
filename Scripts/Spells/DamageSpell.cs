using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DamageSpell : Spell
{
    protected const float damage=200f; //Spell damage amount
    public float GetDamage()
    {
        return damage;
    }
}
