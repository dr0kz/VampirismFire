using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerLeaveWoodState : MonoBehaviour, IExecuteAction<Worker>
{
    private ResourceManager RESOURCE_MANAGER;
    public WorkerLeaveWoodState()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
    }
    public void executeAction(Worker worker)
    {
        if (worker.GetCollidedWith().gameObject != worker.GetDestination()) return;
        RESOURCE_MANAGER.UpdateWoodAmount(worker.GetWoodAmount());
        worker.SetWoodAmount(0);
        worker.SetClosestTree(worker.GetFarm().FindClosestTree());
        worker.SetDestination(worker.GetClosestTree().gameObject);
        worker.SetState(worker.GetRunningTowardState());
        worker.SetWorkerWorkingStatus(false);
    }
}
