using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
public class LumberMill: Farm
{
    [SerializeField] private Worker workerPrefab;
    [SerializeField] private Transform workerSpawnPosition;
    private readonly IBuildingState maximumMeatState = new MaximumMeat();
    protected override void Start()
    {
        base.Start();
        WORKER_MANAGER.notifyUpdatableOnBuildingSpawned();
    }
    public IBuildingState GetReadyState()
    {
        return readyState;
    }
    public IBuildingState GetMaximumMeatState()
    {
        return maximumMeatState;
    }
    public Tree FindClosestTree()
    {
        Tree[] trees = FindObjectsOfType<Tree>();
        Tree closestTree = null;
        float maxDistance = float.MaxValue;
        for (int i = 0; i < trees.Length; i++)
        {
            if (Vector3.Distance(transform.position, trees[i].transform.position) < maxDistance)
            {
                maxDistance = Vector3.Distance(transform.position, trees[i].transform.position);
                closestTree = trees[i];
            }
        }
        return closestTree;
    }
    protected override void OnBuildingFinished()
    {
        BUILDING_MANAGER.ChangeBuildingColor(this, false);
        SOUND_MANAGER.PlaySound(onBuildingFinished, 0.1f);
        if (RESOURCE_MANAGER.GetMeatAmount() == RESOURCE_MANAGER.GetMaxMeatAmount())
        {
            SetState(maximumMeatState);
        }
        else
        {
            SetState(readyState);
        }

    }
}
