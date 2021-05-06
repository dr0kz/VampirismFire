using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Threading;


public class Enemy : Character, IEnemyUpdatable
{
    [SerializeField] private float damage;
    [SerializeField] private FireBall fireBallSpell;
    [SerializeField] private Lightning lightningSpell;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attack1SoundEffect;
    [SerializeField] private AudioClip attack2SoundEffect;
    [SerializeField] private NavMeshObstacle navMeshObstacle;
    [SerializeField] private float spellTime;
    [SerializeField] protected NavMeshAgent agent;
    private Building destination;
    private BuildingManager BUILDING_MANAGER;
    private EnemyManager ENEMY_MANAGER;
    private SoundManager SOUND_MANAGER;
    protected Building attackBuilding;
    private Building collidedWithBuilding;
    private Vector3 lookRotation;
    private Vector3 lookPosition;
    private Quaternion rotation;
    private const float rotationSpeed = 7.0f;
    private const float lookForWorkersRadius = 5.0f;
    private float attackSpeed = 0.8f;
    private IExecuteAction<Enemy> currentState;
    private const float increaseDamageEveryHit = 10f;
    private bool smartAI;
    #region States
    private readonly IExecuteAction<Enemy> runningTowardState = new EnemyRunningTowardState();
    private readonly IExecuteAction<Enemy> attackState = new EnemyAttackState();
    private readonly IExecuteAction<Enemy> idleState = new EnemyIdleState();
    #endregion
    #region Getters
    public bool GetIsSmartAI()
    {
        return smartAI;
    }
    public IExecuteAction<Enemy> GetIdleState()
    {
        return idleState;
    }
    public float GetHealth()
    {
        return health;
    }
    public Building GetDestination()
    {
        return destination;
    }
    public NavMeshObstacle GetNavMeshObstacle()
    {
        return navMeshObstacle;
    }
    public NavMeshAgent GetNavMeshAgent()
    {
        return agent;
    }
    public Building GetCollidedWithBuilding()
    {
        return collidedWithBuilding;
    }
    public Animator GetAnimator()
    {
        return animator;
    }
    #endregion
    #region Setters
    public void SetState(IExecuteAction<Enemy> state)
    {
        currentState = state;
        currentState.executeAction(this);
    }

    public void SetAttackBuilding(Building attackBuilding)
    {
        this.attackBuilding = attackBuilding;
    }
    #endregion
    private List<PlaneBuilder> colliders;
    private void Start()
    {
        BUILDING_MANAGER = BuildingManager.BUILDING_MANAGER;
        ENEMY_MANAGER = EnemyManager.ENEMY_MANAGER;
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
        if (SOUND_MANAGER.GetSoundEffectsStatus()) audioSource.mute = true;
        colliders = new List<PlaneBuilder>();
        ENEMY_MANAGER.register(this);
        animator.SetFloat("Health", health);
        healthBar.gameObject.SetActive(false);
        maxHealth = health;
        healthBar.SetHealth(health);
        healthBar.SetMaxHealth(maxHealth);
        destination = GameObject.FindGameObjectWithTag("Throne").GetComponent<Building>();
        agent.SetDestination(destination.transform.position);
        SetState(runningTowardState);
        animator.SetBool("Run", true);
        agent.isStopped = false;
        InvokeRepeating("CastSpell", spellTime, spellTime);
    }
    private void Update()
    {
        RotateAlongPath();
    }
    public override void Init(IInstantiatable prefab)
    {
        throw new System.NotImplementedException();
    }
    public void Init(float health, float damage, float spellTime, float attackSpeed, bool smartAI)
    {
        this.health = health;
        this.damage = damage;
        this.spellTime = spellTime;
        if (this.spellTime < 8.0f) this.spellTime = 8.0f;
        this.attackSpeed = attackSpeed;
        this.smartAI = smartAI;
    }
    private void CastSpell()
    {
        float random = Random.Range(0.0f, 1.0f);
        if (random <= 0.5f && attackBuilding!=null)
        {
            Lightning lightning = Instantiate(lightningSpell, attackBuilding.transform.position, Quaternion.identity);
            lightning.Init(attackBuilding);

        }
        else StartCoroutine("FireBallCoroutine");
    }
    private IEnumerator FireBallCoroutine()
    {
        float reduceDamage = 0f;
        List<Worker> workers = Physics.OverlapSphere(transform.position, lookForWorkersRadius)
            .Where(gameobject => gameobject.transform.tag.Equals("Worker"))
            .Select(collider => collider.GetComponent<Worker>())
            .ToList();
        foreach (Worker worker in workers)
        {
            FireBall fireBall = Instantiate(fireBallSpell, worker.transform.position, Quaternion.identity);
            fireBall.transform.SetParent(worker.transform);
            worker.TakeDamage(fireBall.GetDamage()- fireBall.GetDamage()*reduceDamage);
            reduceDamage += 0.05f;
            Destroy(fireBall.gameObject, 0.65f);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.25f));
        }
    }
    private void RotateAlongPath()
    {
        lookRotation = agent.steeringTarget - transform.position;
        if (lookRotation == Vector3.zero) return;
        if (navMeshObstacle.isActiveAndEnabled && attackBuilding != null)
        {
            lookPosition = attackBuilding.transform.position - transform.position;
            lookPosition.y = 0.0f;
            rotation = Quaternion.LookRotation(lookPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        else transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        collidedWithBuilding = other.GetComponent<Building>();
        PlaneBuilder pb;
        if (other.GetComponent<Building>() != null)
        {
            SetState(attackState);
        }
        else if((pb = other.GetComponent<PlaneBuilder>()) != null)
        {
            pb.CurrentlyCollidingWith().Add(this);
            colliders.Add(pb);
            pb.IsEnemyOverPlane(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        PlaneBuilder pb;
        if ((pb=other.GetComponent<PlaneBuilder>()) != null)
        {
            if (pb.CurrentlyCollidingWith().Count == 1)
            {
                pb.IsEnemyOverPlane(false);
            }
            pb.CurrentlyCollidingWith().Remove(this);
            colliders.Remove(pb);
        }
    }
    private void Attack()
    {
        if (animator.GetFloat("AttackSpeed") < 3.0f)
        {
            animator.SetFloat("AttackSpeed", animator.GetFloat("AttackSpeed") + 0.0125f);
        }
        if (attackBuilding==null && health>0.0f)
        {
            SetState(runningTowardState);
        }
        else
        {
            if (Random.Range(0f, 1f) >= 0.5f) audioSource.PlayOneShot(attack1SoundEffect);
            else audioSource.PlayOneShot(attack2SoundEffect);
            attackBuilding.TakeDamage(damage);
        }
        damage += increaseDamageEveryHit;
    }
    private void Die()
    {
        GetComponent<Collider>().enabled = false;
        healthBar.gameObject.SetActive(false);
        if (colliders.Count != 0)
        {
            colliders.ForEach(plane =>
            {
                if (plane.CurrentlyCollidingWith().Count == 1)
                {
                    plane.IsEnemyOverPlane(false);
                }
                plane.CurrentlyCollidingWith().Remove(this);

            });
        }
        if (!navMeshObstacle.enabled)
        {
            agent.isStopped = true;
        }
        animator.SetFloat("Health", 0f);
        ENEMY_MANAGER.remove(this);
        agent.enabled = false;
        Destroy(gameObject, 3.65f);
    }
    public override void TakeDamage(float damage)
    {
        health -= damage;
        if (health < maxHealth)
        {
            healthBar.gameObject.SetActive(true);
        }
        healthBar.SetHealth(health);
        if (health <= 0)
        {
            Die();
        }
    }
    public void update()
    {
        throw new System.NotImplementedException();
    }
    public void update(Building building, Building nextBuilding)
    {
        if (attackBuilding == building) attackBuilding = nextBuilding;
    }
}
