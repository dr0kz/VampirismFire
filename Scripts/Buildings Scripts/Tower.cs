using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tower : Building
{
    [SerializeField] protected float radius;
    protected override void Awake()
    {
        base.Awake();
        BUILDING_MANAGER = BuildingManager.BUILDING_MANAGER;
    }

}
