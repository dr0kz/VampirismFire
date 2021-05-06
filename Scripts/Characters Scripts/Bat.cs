using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class Bat : Character
{
    [SerializeField] private float movementSpeed;
    [SerializeField] private Enemy enemy;
    private WorkerManager WORKER_MANAGER;
    private Worker attackWorker;
    protected override void Awake()
    {
        base.Awake();
        WORKER_MANAGER = WorkerManager.WORKER_MANAGER;
    }
    private Worker FindWorker()
    {
        return attackWorker = WORKER_MANAGER.GetUpdatableObjects()
            .Select(worker => worker as Worker)
            .First();
    }
    private void Update()
    {
        if(attackWorker!=null || FindWorker()!=null)
        {
            transform.position = Vector3.MoveTowards(transform.position, attackWorker.transform.position, movementSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Worker>() == attackWorker)
        {
            attackWorker.TakeDamage(float.MaxValue);
            gameObject.SetActive(false);
        }
    }
    private void OnDisable()
    {
        Destroy(gameObject);
    }

    public override void Init(IInstantiatable prefab)
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage(float damageAmount)
    {
        throw new System.NotImplementedException();
    }
}
