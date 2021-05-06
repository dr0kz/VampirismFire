using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public abstract class Building : MonoBehaviour, ISelectable, ITask, IDamageable, ICost, IInstantiatable
{
    [SerializeField] protected float health; //total building's health
    [SerializeField] protected GameObject OnBuildingDestroyParticleEffect;
    [SerializeField] protected MyButton[] buttons; //buttons that appear on building click
    [SerializeField] protected MonoBehaviour[] prefabs; //prefabs for every button
    [SerializeField] protected HealthBar healthBar; //health bar
    [SerializeField] protected HealthBar buildingProgressBar; //build progress bar
    [SerializeField] protected Canvas healthBarCanvas; //canvas for the health bar and build progress bar
    [SerializeField] protected PlaneBuilder standingPlane; //plane under the building
    [SerializeField] protected float buildTime; //time required for the building to be placed / upgraded
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip onBuildingPlaced;
    [SerializeField] protected AudioClip onBuildingProgress;
    [SerializeField] protected AudioClip onBuildingFinished;
    [SerializeField] protected AudioClip onBuildingDestroyed;
    [SerializeField] private int cost;
    [SerializeField] private string resourceName; //wood / gold

    protected CanvasManager CANVAS_MANAGER;
    protected BuildingManager BUILDING_MANAGER;
    protected ResourceManager RESOURCE_MANAGER;
    protected WorkerManager WORKER_MANAGER;
    protected EnemyManager ENEMY_MANAGER;
    protected SoundManager SOUND_MANAGER;
    protected float maxHealth; //max health of the building
    protected Dictionary<MyButton, MonoBehaviour> buildingDictionary; //dictionary for connection every button to every prefab
    protected bool isBuildingInProgress = true; // true if building is in progress, false the opossite
    protected IBuildingState currentState;
    #region States
    protected readonly IBuildingState readyState = new DefaultState();
    protected readonly IBuildingState buildingProgressState = new BuildingProgressState();
    #endregion
    #region Getters
    public PlaneBuilder GetStandingPlane()
    {
        return standingPlane;
    }
    public Vector3 getPosition()
    {
        return transform.position;
    }
    public float GetBuildingHeight()
    {
        return GetComponent<Collider>().bounds.size.y;
    }
    public float GetHealth()
    {
        return health;
    }
    public Dictionary<MyButton, MonoBehaviour> GetDictionary()
    {
        return buildingDictionary;
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }
    #endregion
    #region Setters
    public void SetStandingPlane(PlaneBuilder standingPlane)
    {
        this.standingPlane = standingPlane;
    }
    #endregion

    public void SetState(IBuildingState currentState)
    {
        this.currentState = currentState;
    }
    /// <summary>
    /// Called when the script is loaded
    /// </summary>
    protected virtual void Awake()
    {
        buildingDictionary = new Dictionary<MyButton, MonoBehaviour>();
    }
    /// <summary>
    /// Called on the first frame
    /// </summary>
    protected virtual void Start()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
        BUILDING_MANAGER = BuildingManager.BUILDING_MANAGER;
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
        WORKER_MANAGER = WorkerManager.WORKER_MANAGER;
        ENEMY_MANAGER = EnemyManager.ENEMY_MANAGER;
        SOUND_MANAGER = SoundManager.SOUND_MANAGER;
        if (SOUND_MANAGER.GetSoundEffectsStatus()) audioSource.mute = true;
        transform.position = new Vector3(standingPlane.transform.position.x
        , standingPlane.transform.position.y + standingPlane.GetPlaneHeight() / 2.0f + GetBuildingHeight() / 2.0f
        , standingPlane.transform.position.z);

        standingPlane.GetComponent<BoxCollider>().enabled = false; // the plane's collider under the building is disabled

        if (maxHealth == 0)
        {
            maxHealth = health;
        }
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
        if (health < maxHealth)
        {
            healthBar.gameObject.SetActive(true);
        }
        else
        {
            healthBar.gameObject.SetActive(false);
        }
        BUILDING_MANAGER.addBuilding(this);
        if (gameObject.activeSelf)
        {
            BuildTimeProcess();
        }

    }
    /// <summary>
    /// On building placed or upgraded
    /// </summary>
    protected void BuildTimeProcess()
    {
        audioSource.PlayOneShot(onBuildingPlaced);
        audioSource.PlayOneShot(onBuildingProgress);
        SetState(buildingProgressState);
        Invoke("OnBuildingFinished", buildTime);
        BUILDING_MANAGER.ChangeBuildingColor(this, true);
        buildingProgressBar.SetMaxHealth(buildTime);
        buildingProgressBar.SetHealth(buildTime);
        buildingProgressBar.gameObject.SetActive(true);
        StartCoroutine("BuildTime");
    }
    /// <summary>
    /// Time required for the build progress of the building
    /// </summary>
    /// <returns></returns>
    private IEnumerator BuildTime()
    {
        while (buildingProgressBar.GetHealth() > 0.0f)
        {
            yield return new WaitForSeconds(1.0f);
            buildingProgressBar.SetHealth(buildingProgressBar.GetHealth() - buildingProgressBar.GetMaxHealth() / buildTime);
        }
        buildingProgressBar.gameObject.SetActive(false);
        isBuildingInProgress = false;
    }
    /// <summary>
    /// On building build progress finish
    /// </summary>
    protected virtual void OnBuildingFinished()
    {
        BUILDING_MANAGER.ChangeBuildingColor(this, false);
        audioSource.PlayOneShot(onBuildingFinished);
        SetState(readyState);
    }
    /// <summary>
    /// On Building Selected
    /// </summary>
    public virtual void Select()
    {
        CANVAS_MANAGER.ShowButtonsAndHandleButtonTask(buttons, this, prefabs);
        CANVAS_MANAGER.ShowButtonsBackgroundImage(buttons.Length);
        CANVAS_MANAGER.SetSelectedObject(this);
    }
    /// <summary>
    /// On Building UnSelected
    /// </summary>
    public virtual void Unselect()
    {
        CANVAS_MANAGER.RemoveActiveButtons();
    }
    /// <summary>
    /// Decrease building's health
    /// </summary>
    /// <param name="damage">Amount to decrease the building's health</param>
    /// <returns>New building's health</returns>
    public virtual void TakeDamage(float damage)
    {
        if (ArcaneVaultSpells.ARCANE_VAULT_SPELLS.isBackdoorProtectionActivated) return;
        health -= damage;
        if (health < maxHealth) healthBar.gameObject.SetActive(true);
        healthBar.SetHealth(health);
        if (health <= 0.0f) DestroyBuilding(OnBuildingDestroyParticleEffect);
    }
    /// <summary>
    /// Increase building's health
    /// </summary>
    /// <param name="healAmount">Amount to increase the building's health</param>
    public void HealBuilding(float healAmount)
    {
        health += healAmount;
        healthBar.SetHealth(health);
        if (health >= maxHealth)
        {
            health = maxHealth;
            healthBar.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Remove the building from the scene, enable the plane under the building and remove the building from the list of buildings
    /// </summary>
    public virtual void DestroyBuilding(GameObject particleEffect)
    {
        if(particleEffect!=null)
        {
            SOUND_MANAGER.PlaySound(onBuildingDestroyed, 0.07f);
        }
        if ((object)CANVAS_MANAGER.GetSelectedObject() == this)
        {
            CANVAS_MANAGER.RemoveActiveButtons();
            CANVAS_MANAGER.SetSelectedObject(null);
        }
        BUILDING_MANAGER.removeBuilding(this);
        if(standingPlane.CurrentlyCollidingWith().Count==0)
        {
            standingPlane.GetComponent<BoxCollider>().enabled = true;
        }

        if (particleEffect != null)
        {
            OnBuildingDestroyParticleEffect = Instantiate(particleEffect);
            OnBuildingDestroyParticleEffect.transform.position = transform.position;
            Destroy(OnBuildingDestroyParticleEffect.gameObject, 1f);
        }
        WORKER_MANAGER.notifyUpdatableOnBuildingDestroyed(this);
        gameObject.SetActive(false);
    }
    protected virtual void OnDisable()
    {
        Destroy(gameObject);
    }
    /// <summary>
    /// Applies task to button
    /// Only the Destroy button is executable during build progress
    /// </summary>
    /// <param name="button">Tagged button</param>
    public virtual void ButtonTask(MyButton button)
    {
        if (!button.transform.tag.Equals("Destroy"))
        {
            if (currentState.Execute(button))
            {
                Upgrade(button);
            }
        }
        else
        {
            DestroyBuilding(OnBuildingDestroyParticleEffect);
            CANVAS_MANAGER.SetSelectedObject(null);
            Unselect();
        }
    }
    /// <summary>
    /// example: 1500 wood
    /// example: 200 gold
    /// </summary>
    /// <returns>Returns the cost as string</returns>
    public string GetCost()
    {
        return cost + " " + resourceName;
    }
    public virtual bool IsUnlocked()
    {
        return true;
    }
    protected virtual void Upgrade(MyButton button)
    {

        IInstantiatable prefab = (IInstantiatable)buildingDictionary[button];
        if (prefab is Building building)
        {
            Building prefabInstantiate = Instantiate((Building)prefab);
            ((IInstantiatable)prefabInstantiate).Init(this);
            WORKER_MANAGER.notifyUpdatableOnBuildingUpgrade(this, prefabInstantiate);
            ENEMY_MANAGER.notifyUpdatableOnBuildingUpgrade(this, prefabInstantiate);
            DestroyBuilding(null);
            Unselect();
        }
        else if (prefab is Worker character)
        {
            Worker worker = Instantiate((Worker)prefab);
            worker.Init(this);
        }
    }
    public virtual void Init(IInstantiatable _building)
    {
        Building building = (Building)_building;
        standingPlane = building.standingPlane;
        maxHealth = health;
        float percent = (building.health * 100.0f) / building.maxHealth;
        health = (maxHealth * percent) / 100.0f + ((100.0f - percent) * maxHealth) / 200.0f;
    }
}




