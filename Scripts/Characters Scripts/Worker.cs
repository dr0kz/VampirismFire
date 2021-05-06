using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Linq;
using System;

public class Worker : Character, ISelectable, IWorkerUpdatable, ICost
{
    [SerializeField] private int cost; //Cost of the worker
    [SerializeField] private string resourceName; //Resource type (wood or gold )
    [SerializeField] private GameObject axe; //Axe object
    [SerializeField] private GameObject wood; //Wood object
    [SerializeField] private HealSpell healParticleEffect; //Healing effect object
    [SerializeField] private AudioSource audioSource; //audio source component
    [SerializeField] private AudioClip workerDieSoundEffect;
    [SerializeField] private AudioClip workerAllRightSoundEffect;
    [SerializeField] private AudioClip workerReadyToWorkSoundEffect;
    [SerializeField] private AudioClip workerYesMilordSoundEffect;
    [SerializeField] private NavMeshObstacle navMeshObstacle; //Used when the worker is not moveing ( Create hole in the nav mesh )
    [SerializeField] private NavMeshAgent agent; //AI agent component
    private IExecuteAction<Worker> currentState; //Current state 
    private int woodAmount = 0; //current wood amount in inventory
    private const float rotationSpeed = 8.0f; //Extra rotation
    private LumberMill farm; //Closest farm
    private bool isAlreadyWorking = false; // Current working status
    private bool isAlreadyRepairing = false; //Current repairing status
    private Collider collidedWith; //When worker touches something this variable is set to the collider that the worker touched
    private GameObject destination; //Worker's destination
    private Vector3 lookRotation;
    private Vector3 lookPosition;
    private Tree closestTree; //Always search for the closest available tree
    private Quaternion rotation;
    #region Saved Data
    private float repairAmountPercent; //Repair amount per hit
    private float repairSpeed; //Repairing speed
    private float woodCuttingSpeed; //Wood cutting speed
    private int woodAmountPerHit; //Store wood amount per hit
    private int woodCapacity; //Maximum wood capacity
    #endregion
    #region Singletons
    private WorkerManager WORKER_MANAGER;
    private BuildingManager BUILDINGS_MANAGER;
    private ResourceManager RESOURCE_MANAGER;
    private SoundManager SOUND_MANAGER;
    #endregion
    #region Worker States
    private readonly IExecuteAction<Worker> runningTowardState = new WorkerRunningTowardState();
    private readonly IExecuteAction<Worker> choppingTreeState = new WorkerChoppingTreeState();
    private readonly IExecuteAction<Worker> repairState = new WorkerRepairState();
    private readonly IExecuteAction<Worker> idleState = new WorkerIdleState();
    private readonly IExecuteAction<Worker> bringingWoodState = new WorkerBringingWoodState();
    private readonly IExecuteAction<Worker> leaveWoodState = new WorkerLeaveWoodState();
    #endregion
    #region Worker Getters
    public AudioSource GetAudioSource()
    {
        return audioSource;
    }
    public Collider GetCollider()
    {
        return GetComponent<BoxCollider>();
    }
    public IExecuteAction<Worker> GetRunningTowardState()
    {
        return runningTowardState;
    }
    public IExecuteAction<Worker> GetChoppingTreeState()
    {
        return choppingTreeState;
    }
    public IExecuteAction<Worker> GetRepairState()
    {
        return repairState;
    }
    public IExecuteAction<Worker> GetIdleState()
    {
        return idleState;
    }
    public IExecuteAction<Worker> GetBringingWoodState()
    {
        return bringingWoodState;
    }
    public IExecuteAction<Worker> GetLeaveWoodState()
    {
        return leaveWoodState;
    }
    public bool IsAlreadyWorkingStatus()
    {
        return isAlreadyWorking;
    }
    public bool isAlreadyRepairingStatus()
    {
        return isAlreadyRepairing;
    }
    public Collider GetCollidedWith()
    {
        return collidedWith;
    }
    public GameObject GetDestination()
    {
        return destination;
    }
    public NavMeshAgent GetAgent()
    {
        return agent;
    }
    public string GetCost()
    {
        return cost + " " + resourceName;
    }
    public Tree GetClosestTree()
    {
        return closestTree;
    }
    public int GetWoodAmount()
    {
        return woodAmount;
    }
    public LumberMill GetFarm()
    {
        return farm;
    }
    public int GetWoodCapacity()
    {
        return woodCapacity;
    }
    public GameObject GetAxe()
    {
        return axe;
    }
    public GameObject GetWood()
    {
        return wood;
    }
    #endregion
    #region Worker Setters
    public void SetDestination(GameObject destinationObject)
    {
        destination = destinationObject;
    }
    public void SetState(IExecuteAction<Worker> state)
    {
        currentState = state;
        currentState.executeAction(this);
    }
    public void SetClosestTree(Tree closestTree)
    {
        this.closestTree = closestTree;
    }
    public void SetWorkerWorkingStatus(bool isWorking)
    {
        isAlreadyWorking = isWorking;
    }
    public void SetWorkerRepairingStatus(bool isRepairing)
    {
        isAlreadyRepairing = isRepairing;
    }
    public void SetWoodAmount(int woodAmount)
    {
        this.woodAmount = woodAmount;
    }
    public void SetFarm(Building farm)
    {
        this.farm = (LumberMill)farm;
    }
    #endregion


    /// <summary>
    /// Called when script is loaded
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        BUILDINGS_MANAGER = BuildingManager.BUILDING_MANAGER;
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
        WORKER_MANAGER = WorkerManager.WORKER_MANAGER;
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
    }
    /// <summary>
    /// Called the first frame
    /// </summary>
    private void Start()
    {
        if (SOUND_MANAGER.GetSoundEffectsStatus()) audioSource.mute = true;
        WorkerData data = SaveSystem.LoadData().workerData;
        #region Link Characteristics From Save System
        agent.speed = data.movementSpeed;
        woodCapacity = data.woodCapacity;
        woodCuttingSpeed = data.cuttingSpeed;
        woodAmountPerHit = data.woodPerHit;
        repairAmountPercent = data.repairingPercentPerHit;
        repairSpeed = data.repairingSpeed;
        #endregion
        animator.SetFloat("cuttingWoodSpeedMultiplier", woodCuttingSpeed);
        animator.SetFloat("repairingSpeedMultiplier", repairSpeed);
        agent.Warp(farm.transform.position);
        maxHealth = health;
        closestTree = farm.FindClosestTree();
        destination = closestTree.gameObject;
        animator.SetFloat("health", health);
        SetState(runningTowardState);
        WORKER_MANAGER.register(this);
        healthBar.SetHealth(health);
        healthBar.SetMaxHealth(maxHealth);
        healthBar.gameObject.SetActive(false);
        audioSource.volume = 0.25f;
        audioSource.PlayOneShot(workerReadyToWorkSoundEffect);
    }
    /// <summary>
    /// Damage worker
    /// </summary>
    public override void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetFloat("health", health);
        healthBar.SetHealth(health);
        healthBar.gameObject.SetActive(true);
        if (health <= 0.0f) Die();
    }
    /// <summary>
    /// Kills the worker when his health drops below 0f
    /// </summary>
    private void Die()
    {
        if(!navMeshObstacle.isActiveAndEnabled) agent.isStopped = true;
        audioSource.PlayOneShot(workerDieSoundEffect);
        GetComponent<BoxCollider>().enabled = false;
        if ((object)CANVAS_MANAGER.GetSelectedObject()==this)
        {
            CANVAS_MANAGER.SetSelectedObject(null);
            Unselect();
        }
        RESOURCE_MANAGER.UpdateMeatAmount(-1);
        WORKER_MANAGER.remove(this);
        Destroy(gameObject, 2.6f);
    }
    /// <summary>
    /// Called every frame 
    /// </summary>
    private void Update()
    {
        RotateWorkerAlongPath();
    }
    /// <summary>
    /// Provides extra rotation
    /// </summary>
    private void RotateWorkerAlongPath()
    {
        lookRotation = agent.steeringTarget - transform.position;
        if (lookRotation.Equals(Vector3.zero) && destination != null)
        {
            lookPosition = destination.transform.position - transform.position;
            lookPosition.y = 0.0f;
            rotation = Quaternion.LookRotation(lookPosition);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
        else if (!lookRotation.Equals(Vector3.zero))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookRotation), rotationSpeed * Time.deltaTime);
        }
    }
    /// <summary>
    /// Set worker's destination
    /// </summary>
    /// <param name="destination">Object that worker is going toward</param>
    public void GoToward(GameObject destination)
    {
        if (this.destination != null && this.destination.Equals(destination)) return;
        if (destination.GetComponent<LumberMill>() != null) farm = destination.GetComponent<LumberMill>();
        this.destination = destination;
        audioSource.PlayOneShot(workerAllRightSoundEffect);
        SetState(runningTowardState);
    }
    /// <summary>
    /// Increase worker's health
    /// </summary>
    public void Heal(float healAmount)
    {
        health += healAmount;
        healthBar.SetHealth(health);
        if (health > maxHealth)
        {
            health = maxHealth;
            healthBar.gameObject.SetActive(false);
        }
        animator.SetFloat("health", health);
    }
    /// <summary>
    /// Increase the amount of wood the worker is carring
    /// Change state to bringing if the amount of wood goes above capacity
    /// </summary>
    public void IncreaseWoodAmount()
    {
        woodAmount += woodAmountPerHit;
        closestTree.OnTreeHitAnimation();
        if (woodAmount >= woodCapacity)
        {
            woodAmount = woodCapacity;
            if (farm == null) return;
            destination = farm.gameObject;
            SetState(bringingWoodState);
        }
    }
    /// <summary>
    /// Increase the health of the building being repaired
    /// </summary>
    public void Repair()
    {
        if (destination != null)
        {
            Building building = destination.GetComponent<Building>();
            building.HealBuilding((repairAmountPercent/100.0f)* building.GetMaxHealth());
        }
        else SetState(idleState);
    }
    /// <summary>
    /// The function triggers when worker touches another collider
    /// </summary>
    /// <param name="other">Collider that worker touched</param>
    private void OnTriggerEnter(Collider other)
    {
        collidedWith = other;
        if (other.GetComponent<LumberMill>() != null) SetState(leaveWoodState);
        else if (other.GetComponent<Building>() != null) SetState(repairState);
        else if (other.GetComponent<Tree>() != null) SetState(choppingTreeState);
    }
    /// <summary>
    /// The function is active while the worker is colliding with another collider
    /// </summary>
    /// <param name="other">Collider that worker is currently colliding with</param>
    private void OnTriggerStay(Collider other) //PROBLEM
    {
        //if (other.GetComponent<LumberMill>() != null) SetState(leaveWoodState);
        //else if (other.GetComponent<Tree>() != null) SetState(choppingTreeState);
        //else if (other.GetComponent<Building>() != null) SetState(repairState);
    }
    /// <summary>
    /// Find closest farm from current worker's possition
    /// </summary>
    public void FindClosestFarm() //O(n)
    {
        farm = BUILDINGS_MANAGER
                .getBuildings() //Get all buildings from the scene
                .Where(building => building.GetComponent<LumberMill>() != null) // Filter all lumber mills
                .OrderBy((building) => Vector3.Distance(transform.position, building.transform.position)) //order by distance between the worker and each lumber mill
                .FirstOrDefault() as LumberMill; // Returns the first element if exist otherwise null
    }
    /// <summary>
    /// Worker selected
    /// </summary>
    public void Select() //O(n^2) worst case , O(1) best case 
    {
        BUILDINGS_MANAGER.ChangeBuildingsColor(true);
        CANVAS_MANAGER.SetSelectedObject(this);
        audioSource.PlayOneShot(workerYesMilordSoundEffect);
    }
    /// <summary>
    /// Worker unselected
    /// </summary>
    public void Unselect()
    {
        BUILDINGS_MANAGER.ChangeBuildingsColor(false);
    }
    /// <summary>
    /// Update worker's closest farm
    /// </summary>
    public void update()
    {
        FindClosestFarm();
    }
    /// <summary>
    /// Update worker's destination on building destroyed
    /// If worker is going toward building then change his state to idle
    /// If the destination was LumberMill then find closest existing LumberMill
    /// </summary>
    /// <param name="building">Building the worker is going toward</param>
    public void update(Building building)
    {
        if (destination == building.gameObject) SetState(idleState);
        if (building.GetComponent<LumberMill>() != null) update();
    }
    /// <summary>
    /// Update worker's destination on building upgraded
    /// </summary>
    /// <param name="building">Building the worker is going toward</param>
    /// <param name="nextBuilding">Next destination </param>
    public void update(Building building, Building nextBuilding)
    {
        if (destination == building.gameObject)
        {
            destination = nextBuilding.gameObject;
        }
    }
    public bool IsUnlocked()
    {
        return true;
    }
    /// <summary>
    /// Configure animator settings
    /// </summary>
    public void configureAnimatorSettings(bool isChopping, bool isIdle, bool isRepairing, bool isRunning)
    {
        animator.SetBool("isChopping", isChopping);
        animator.SetBool("isIdle", isIdle);
        animator.SetBool("isRepairing", isRepairing);
        animator.SetBool("isRunning", isRunning);
        animator.SetInteger("woodAmount", woodAmount);
    }
    /// <summary>
    /// Initializing
    /// Get the position of the lumber mill that spawned this worker and increase the food amount
    /// </summary>
    /// <param name="_building"></param>
    public override void Init(IInstantiatable _building)
    {
        Building building = (Building)_building;
        transform.position = building.transform.position;
        SetFarm(building);
        RESOURCE_MANAGER.UpdateMeatAmount(1);
    }



}

