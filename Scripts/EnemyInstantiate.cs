using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInstantiate : MonoBehaviour
{
    [Header("ENEMY")]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private bool activateEnemySpell;
    [SerializeField] private float startSpellTimeCooldown;
    //TODO [SerializeField] private bool smartAI; //if path exists to the throne then every other object is bypassed

    [Space(20)]
    [Header("WAVE SETTINGS")]
    [SerializeField] private byte numberOfEnemiesPerWave; //Total number of enemies per wave
    [SerializeField] private byte increaseNumberOfEnemiesBy; //By how many enemies
    [SerializeField] private byte increaseNumberOfEnemiesEvery; //Increase the number of enemies spawned per wave , every X waves
    [SerializeField] private float increaseEnemyHpEveryWaveBy; //Inrease the hp of the enemies every wave
    [SerializeField] private float decreaseEnemySpellTime; //Increase the spell chance of the enemies every wave
    [SerializeField] private float increaseDamage; //Increase damage to the enemies every wave
    [SerializeField] private int maximumNumberOFEnemiesPerWave;
    [SerializeField] private int waitBeforeFirstWave;
    [SerializeField] private bool doesEnemiesDropResource;
    [SerializeField] private string destinationTag;

    [Space(20)]
    [Header("BOSS")]
    [SerializeField] private Boss bossPrefab;
    [SerializeField] private float extraDamage;
    [SerializeField] private float extraHealth;
    [SerializeField] private bool activateBossSpells;

    private ResourceManager RESOURCE_MANAGER;

    private const float reduceEnemySpellTime = 25f;
    private const float minimumEnemySpellTime = 8f;

    private bool canInstantiate = false;
    private int numberOfEnemiesInCurrentWave = 1;
    private float enemyHp = 0f;
    private float attackSpeed = 1.0f;
    private float damage = 0f;

    private void Awake()
    {
        StartCoroutine("WaitBeforeFirstWave");
    }
    private void Start()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
    }
    /// <summary>
    /// Called on the first frame
    /// </summary>
    public void StartInstantiate()
    {
        numberOfEnemiesInCurrentWave = numberOfEnemiesPerWave;
        StartCoroutine("InstantiateWave");
        IncreaseEnemyStats();
    }
    public int GetWaitBeforeFirstWave()
    {
        return waitBeforeFirstWave;
    }
    private IEnumerator WaitBeforeFirstWave()
    {
        yield return new WaitForSeconds(waitBeforeFirstWave);
        canInstantiate = true;
        StartInstantiate();
    }
    public bool GetInstantiatePermission()
    {
        return canInstantiate;
    }
    /// <summary>
    /// Boost health , damage , attackspeed for each enemy for the next wave and increse the number of enemies per wave every increaseNumberOfEnemiesEvery waves
    /// </summary>
    private void IncreaseEnemyStats()
    {
        if (RESOURCE_MANAGER.GetWaveLevel() % increaseNumberOfEnemiesEvery == 0 && numberOfEnemiesPerWave < maximumNumberOFEnemiesPerWave)
        {
            numberOfEnemiesPerWave += 1;
        }
        if (attackSpeed < 3f)
        {
            attackSpeed += 0.1f;
        }
        enemyHp += increaseEnemyHpEveryWaveBy;
        damage += increaseDamage;
        decreaseEnemySpellTime -= 1.0f;
    }
    /// <summary>
    /// Instantiate enemies
    /// </summary>
    public IEnumerator InstantiateWave()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < numberOfEnemiesInCurrentWave; i++)
        {
            yield return new WaitForSeconds(Random.Range(1.5f,4f));
            Enemy enemy = Instantiate(enemyPrefab, this.transform.position, Quaternion.identity);
            bool smart = Random.Range(0f, 1f) >= 0.5f;
            enemy.Init(enemyHp, damage, decreaseEnemySpellTime, attackSpeed, smart);
        }
    }
    public void InstantiateBoss() //last wave
    {
        Boss boss = Instantiate(bossPrefab, this.transform.position, Quaternion.identity);
        bool smart = Random.Range(0f, 1f) >= 0.5f;
        boss.Init(enemyHp + extraHealth, damage + extraDamage, decreaseEnemySpellTime, attackSpeed, smart);

    }
}
