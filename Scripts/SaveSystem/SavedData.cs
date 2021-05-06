using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavedData
{
    public WorkerData workerData { get; set; }
    public List<ItemData> items { get; set; }
    public WallData wallData { get; set; }
    public TowerData towerData { get; set; }
    public ArcaneVaultData arcaneVaultData { get; set; }
    public int diamondsAmount { get; set; } //resources in the shop

    public SavedData(WorkerData workerData, List<ItemData> items, WallData wallData, TowerData towerData, ArcaneVaultData arcaneVaultData, int diamondsAmount)
    {
        this.workerData = workerData;
        this.items = items;
        this.wallData = wallData;
        this.towerData = towerData;
        this.arcaneVaultData = arcaneVaultData;
        this.diamondsAmount = diamondsAmount;
    }
}





