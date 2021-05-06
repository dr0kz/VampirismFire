using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WorkerData
{
    public float movementSpeed { get; set; }
    public float cuttingSpeed { get; set; }
    public float repairingSpeed { get; set; }
    public float repairingPercentPerHit { get; set; }
    public int woodCapacity { get; set; }
    public int woodPerHit { get; set; }

    public WorkerData(float movementSpeed, float cuttingSpeed, float repairingSpeed, float repairingPercentPerHit, int woodCapacity, int woodPerHit)
    {
        this.movementSpeed = movementSpeed;
        this.cuttingSpeed = cuttingSpeed;
        this.repairingSpeed = repairingSpeed;
        this.repairingPercentPerHit = repairingPercentPerHit;
        this.woodCapacity = woodCapacity;
        this.woodPerHit = woodPerHit;
    }
}
