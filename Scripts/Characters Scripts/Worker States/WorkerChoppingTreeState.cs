using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerChoppingTreeState : IExecuteAction<Worker>
{
    public void executeAction(Worker worker)
    {
        if (worker.GetDestination().GetComponent<Tree>() == null || worker.IsAlreadyWorkingStatus()) return;
        worker.GetComponent<NavMeshAgent>().enabled = false;
        worker.GetComponent<NavMeshObstacle>().enabled = true;
        worker.SetWorkerWorkingStatus(true);
        worker.SetClosestTree(worker.GetCollidedWith().GetComponent<Tree>());
        worker.SetDestination(worker.GetCollidedWith().gameObject);
        worker.GetAxe().SetActive(true);
        worker.GetWood().SetActive(false);
        worker.configureAnimatorSettings(true, false, false, false);
    }
}
