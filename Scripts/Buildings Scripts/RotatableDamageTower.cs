using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatableDamageTower : DamageTower
{
    [SerializeField] private float rotationSpeed;
    protected override void AttackEnemy()
    {
        base.AttackEnemy();
        RotateTowardEnemy();
    }
    private void RotateTowardEnemy()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.position - currentEnemy.transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
