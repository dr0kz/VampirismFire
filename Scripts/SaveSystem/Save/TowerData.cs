using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerData
{
    public int biggestUnlockedTowerIndex { get; set; }
    public List<float> towersDamage { get; set; }
    public TowerData(List<float> towersDamage, int biggestUnlockedTowerIndex)
    {
        this.towersDamage = new List<float>();
        this.towersDamage.AddRange(towersDamage);
        this.biggestUnlockedTowerIndex = biggestUnlockedTowerIndex;
    }
}

