using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DamageTower : Tower
{
    [SerializeField] protected float attackRate;
    [SerializeField] protected TowerWeapon firePrefab;
    [SerializeField] protected Transform shootingPosition;
    [SerializeField] protected int index;
    [SerializeField] protected AudioClip fireSound;
    protected float attackCountDown = 0.0f;
    protected Collider[] colliders;
    protected Enemy currentEnemy;
    protected float damage;

    protected override void Start()
    {
        base.Start();
        damage = SaveSystem.LoadData().towerData.towersDamage[index];
        UpdateDamage();
    }
    private void Update()
    {
        if (currentEnemy == null || DistanceToEnemy(currentEnemy) > radius || currentEnemy.GetHealth()<=0.0f) CheckForNearEnemies();
        else AttackEnemy();
        attackCountDown -= Time.deltaTime;
    }
    public void UpdateDamage()
    {
        damage *= ArcaneVaultSpells.ARCANE_VAULT_SPELLS.towerDoubleDamageMultiplier;
    }
    protected virtual void AttackEnemy()
    {
        if (attackCountDown <= 0f)
        {
            TowerWeapon towerWeapon = Instantiate(firePrefab, shootingPosition.position, Quaternion.identity);
            towerWeapon.SetEnemy(currentEnemy);
            towerWeapon.SetDamage(damage);
            attackCountDown = attackRate;
            //audioSource.volume = 0.082f;
            audioSource.PlayOneShot(fireSound);
        }
    }
    protected float DistanceToEnemy(Enemy enemy)
    {
        return Vector3.Distance(transform.position, enemy.transform.position);
    }
    public override bool IsUnlocked()
    {
        return SaveSystem.LoadData().towerData.biggestUnlockedTowerIndex >= index;
    }
    protected void CheckForNearEnemies()
    {
        float distance = int.MaxValue;
        colliders = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider collider in colliders)
        {
            if (collider.tag.Equals("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if(enemy.GetHealth()>0.0f)
                {
                    if (DistanceToEnemy(enemy) < distance)
                    {
                        distance = DistanceToEnemy(enemy);
                        currentEnemy = enemy;
                    }
                }

            }
        }
    }
}
