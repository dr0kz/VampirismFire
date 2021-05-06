using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Threading;
using TMPro;
public class HealTower : Tower
{
    [SerializeField] private ChainHeal healSpell;
    [SerializeField] private AudioClip healSoundEffect;
    [SerializeField] private AudioClip notEnoughMana;
    [SerializeField] private HealthBar manaBar;
    [SerializeField] private TextMeshProUGUI availableHeals;

    private const int maxMana = 400;
    private const int healingSpellManaCost = 100;
    private int currentMana;
    protected override void Start()
    {
        base.Start();
        currentMana = maxMana;
        manaBar.SetHealth(maxMana);
        manaBar.SetMaxHealth(maxMana);
        manaBar.gameObject.SetActive(false);
        availableHeals.text = (currentMana / healingSpellManaCost).ToString();
    }
    /// <summary>
    /// Called when heal tower is selected
    /// </summary>
    public override void Select()
    {
        base.Select();
        WORKER_MANAGER.ChangeWorkersColor(true);
    }
    /// <summary>
    /// Called when heal tower is unselected
    /// </summary>
    public override void Unselect()
    {
        base.Unselect();
        WORKER_MANAGER.ChangeWorkersColor(false);
    }
    private void IncreaseManaEverySecond()
    {
        currentMana += 4;
        manaBar.SetHealth(currentMana);
        availableHeals.text = (currentMana / healingSpellManaCost).ToString();
        if (currentMana > maxMana)
        {
            manaBar.gameObject.SetActive(false);
            currentMana = maxMana;
            CancelInvoke("IncreaseManaEverySecond");
        }
    }
    public override void DestroyBuilding(GameObject particleEffect)
    {
        base.DestroyBuilding(particleEffect);
        WORKER_MANAGER.ChangeWorkersColor(false);
    }
    /// <summary>
    /// Heal all workers in radius around the starting worker.
    /// The heal effect jumps from one worker to another every 0.085 - 0.185 seconds
    /// </summary>
    public IEnumerator HealWorkers(Worker startingWorker)
    {
        if (currentMana - healingSpellManaCost < 0)
        {
            audioSource.PlayOneShot(notEnoughMana);
        }
        else
        {
            manaBar.gameObject.SetActive(true);
            currentMana -= healingSpellManaCost;
            availableHeals.text = (currentMana / healingSpellManaCost).ToString();
            manaBar.SetHealth(currentMana);
            if(!IsInvoking("IncreaseManaEverySecond"))
            {
                InvokeRepeating("IncreaseManaEverySecond", 0f, 1f);
            }
            List<Worker> workersGettingHealder = WORKER_MANAGER.GetUpdatableObjects()
            .Select(_worker => _worker as Worker)
            .Where(_worker => Vector3.Distance(_worker.transform.position, startingWorker.transform.position) <= ChainHeal.healRadius)
            .ToList();
            foreach (Worker worker in workersGettingHealder)
            {
                if (worker != null)
                {
                    worker.GetAudioSource().PlayOneShot(healSoundEffect);
                    ChainHeal healParticle = Instantiate(healSpell);
                    healParticle.transform.position = new Vector3(worker.transform.position.x, worker.transform.position.y + 1f, worker.transform.position.z);
                    healParticle.transform.SetParent(worker.transform);
                    Destroy(healParticle.gameObject, 1.0f);
                    worker.Heal(healParticle.GetHealAmount()); //Worker might get killed before healing effect is applied to him
                }
                yield return new WaitForSeconds(Random.Range(0.085f, 0.185f));//wait before healing next worker
            }
        }

    }
    public bool HaveEnoughMana()
    {
        return currentMana >= healingSpellManaCost;
    }
}
