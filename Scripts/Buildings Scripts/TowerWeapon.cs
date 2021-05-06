using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerWeapon : MonoBehaviour
{
    private Enemy targetingEnemy;
    [SerializeField] private float weaponSpeed; //should this be in tower class?
    private Vector3 lastEnemyPosition;
    private float damage;

    private void Update()
    {
        MoveTowardsEnemy();
    }
    private void MoveTowardsEnemy()
    {
        if (targetingEnemy.GetHealth() > 0.0f)
        {
            lastEnemyPosition = targetingEnemy.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetingEnemy.transform.position, weaponSpeed * Time.deltaTime);
            RotateTowardEnemy();
        }
        else
        {
            transform.position = Vector3.MoveTowards(this.transform.position, lastEnemyPosition, weaponSpeed * Time.deltaTime);
            if (transform.position==lastEnemyPosition) Destroy(gameObject);
        }
    }
    private void RotateTowardEnemy()
    {
        if(transform.position - lastEnemyPosition != Vector3.zero) 
        transform.rotation = Quaternion.LookRotation(transform.position - lastEnemyPosition, Vector3.up);
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag.Equals("Enemy") && collider.GetComponent<Enemy>()==targetingEnemy)
        {
            collider.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    public void SetDamage(float towerDamage)
    {
        this.damage = towerDamage;
    }
    public void SetEnemy(Enemy enemy)
    {
        this.targetingEnemy = enemy;
    }
}
