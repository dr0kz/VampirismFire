using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerRunningTowardState : IExecuteAction<Worker>
{
    public void executeAction(Worker worker)
    {
        worker.GetCollider().enabled = false;
        worker.GetComponent<NavMeshObstacle>().enabled = false;
        worker.GetComponent<NavMeshAgent>().enabled = true;
        worker.GetAgent().isStopped = false;
        if (worker.GetWoodAmount() == 0) worker.GetWood().SetActive(false);
        else worker.GetWood().SetActive(true);
        worker.GetAxe().SetActive(false);
        worker.GetAgent().SetDestination(worker.GetDestination().transform.position);
        worker.SetWorkerWorkingStatus(false);
        worker.GetCollider().enabled = true;
        worker.configureAnimatorSettings(false, false, false, true);
    }
}
