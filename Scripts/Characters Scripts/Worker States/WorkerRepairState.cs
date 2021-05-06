using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerRepairState : IExecuteAction<Worker>
{
    public void executeAction(Worker worker)
    {
        if (worker.GetCollidedWith()!=null && worker.GetCollidedWith().gameObject != worker.GetDestination()) return;
        worker.GetComponent<NavMeshAgent>().enabled = false;
        worker.GetComponent<NavMeshObstacle>().enabled = true;
        worker.configureAnimatorSettings(false, false, true, false);
        worker.GetAxe().SetActive(true);
        worker.GetWood().SetActive(false);
    }
}
