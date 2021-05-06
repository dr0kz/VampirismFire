using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerBringingWoodState : IExecuteAction<Worker>
{
    public void executeAction(Worker worker)
    {
        worker.GetComponent<NavMeshObstacle>().enabled = false;
        worker.GetComponent<NavMeshAgent>().enabled = true;
        worker.GetAgent().SetDestination(worker.GetFarm().transform.position);
        worker.GetWood().SetActive(true);
        worker.GetAxe().SetActive(false);
        //worker.SetWorkerWorkingStatus(false);
        worker.configureAnimatorSettings(false, false, false, true);
    }
}
