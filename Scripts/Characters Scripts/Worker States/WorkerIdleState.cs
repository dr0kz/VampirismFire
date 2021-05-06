using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerIdleState : IExecuteAction<Worker>
{
    public void executeAction(Worker worker)
    {
        worker.SetDestination(null);
        worker.GetComponent<NavMeshAgent>().enabled = false;
        worker.GetComponent<NavMeshObstacle>().enabled = true;
        worker.GetAxe().SetActive(false);
        if (worker.GetWoodAmount() != 0) worker.GetWood().SetActive(true);
        worker.configureAnimatorSettings(false, true, false, false);
    }
}
