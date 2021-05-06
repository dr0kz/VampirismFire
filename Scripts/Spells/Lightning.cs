using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : DamageSpell
{
    private IDamageable damageBuilding;
    private void Start()
    {
        Destroy(gameObject, 3f);
        InvokeRepeating("ApplyDamage", 0.1f, 0.1f);
    }
    private void ApplyDamage()
    {
        if(damageBuilding.Equals(null))
        {
            CancelInvoke("ApplyDamage");
            gameObject.SetActive(false);
        }
        else
        {
            damageBuilding.TakeDamage(damage / 10);
        }
    }
    private void OnDisable()
    {
        Destroy(gameObject);
    }
    public void Init(IDamageable damageBuilding)
    {
        this.damageBuilding = damageBuilding;
    }
}
