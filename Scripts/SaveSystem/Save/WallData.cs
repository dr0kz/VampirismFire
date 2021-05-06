using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WallData
{
    public List<float> wallsHealth { get; set; }
    public int biggestUnlockedWallIndex { get; set; }
    public WallData(List<float> wallsHealth, int biggestUnlockedWallIndex)
    {
        this.wallsHealth = new List<float>();
        this.wallsHealth.AddRange(wallsHealth);
        this.biggestUnlockedWallIndex = biggestUnlockedWallIndex;
    }
}

