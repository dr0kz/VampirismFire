using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HealSpell : Spell
{
    [SerializeField] protected float healAmount;
    protected WorkerManager WORKER_MANAGER;
    public float GetHealAmount()
    {
        return healAmount;
    }
}
