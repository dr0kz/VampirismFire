using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Character : MonoBehaviour,IInstantiatable,IDamageable
{
    [SerializeField] protected float health;
    [SerializeField] protected Animator animator;
    [SerializeField] protected HealthBar healthBar;
    protected CanvasManager CANVAS_MANAGER;
    protected float maxHealth;
    protected virtual void Awake()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
    }
    public abstract void Init(IInstantiatable prefab);
    public abstract void TakeDamage(float damageAmount);
}
