using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSave : MonoBehaviour
{
    #region All Objects That Need To Be Saved
    [Header("WORKER DATA")]
    [SerializeField] public float movementSpeed;
    [SerializeField] public float cuttingSpeed;
    [SerializeField] public float repairingSpeed;
    [SerializeField] public float repairingPercentPerHit;
    [SerializeField] public int woodCapacity;
    [SerializeField] public int woodPerHit;
    [Header("ITEMS DATA")]
    [SerializeField] public List<Item> items;
    [Header("WALL DATA")]
    [SerializeField] public List<float> walls;
    [Header("TOWER DATA")]
    [SerializeField] public List<float> towers;
    [Header("ARCANE VAULT DATA")]
    [SerializeField] public float armorMultiplier;
    [SerializeField] public float healthMultiplier;
    [SerializeField] public float backdoorProtection;
    [Header("AVAILABLE DIAMOND RESOURCE")]
    [SerializeField] public int diamonds;
    #endregion

    /// <summary>
    /// Called when the script is loaded
    /// </summary>
    private void Awake()
    {
        //Save data only the first time the game is launched
        if (SaveSystem.LoadData() == null)
        {
            Debug.Log("FIRST TIME SAVE");
            WorkerData workerData = new WorkerData(movementSpeed, cuttingSpeed, repairingSpeed, repairingPercentPerHit, woodCapacity, woodPerHit);
            List<ItemData> list = new List<ItemData>();
            items.ForEach(item =>list.Add(new ItemData(item.level, item.cost, item.index, item.pageIndex, item.isVisible, true, true,true)));
            WallData walData = new WallData(walls, 0);
            TowerData towerData = new TowerData(towers, 0);
            ArcaneVaultData arcaneVaultData = new ArcaneVaultData(false, armorMultiplier, healthMultiplier, backdoorProtection);
            SaveSystem.SaveData(workerData, list, walData, towerData, arcaneVaultData, diamonds);
        }
    }
}
