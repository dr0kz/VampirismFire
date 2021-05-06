using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class ResourceManager : MonoBehaviour
{
    #region Singleton
    public static ResourceManager RESOURCE_MANAGER;
    #endregion
    [SerializeField] private int woodAmount = 0;
    [SerializeField] private int goldAmount = 0;
    [SerializeField] private int meatAmount = 0;
    [SerializeField] private int maxMeatAmount = 12;
    [SerializeField] private int totalWaves;
    private int waveLevel = 0;
    private CanvasManager CANVAS_MANAGER;
    private BuildingManager BUILDING_MANAGER;
    private int totalWoodAmount;
    private int totalGoldAmount;
    #region Getters
    public int GetWaveLevel()
    {
        return waveLevel;
    }
    public int GetMaxMeatAmount()
    {
        return maxMeatAmount;
    }
    public int GetMeatAmount()
    {
        return meatAmount;
    }
    public int GetGoldAmount()
    {
        return goldAmount;
    }
    public int GetTotalWaves()
    {
        return totalWaves;
    }
    public int GetWoodAmount()
    {
        return woodAmount;
    }
    #endregion
    /// <summary>
    /// Called when the script is loaded
    /// </summary>
    private void Awake()
    {
        RESOURCE_MANAGER = this;
    }
    /// <summary>
    /// Called on the first frame
    /// </summary>
    private void Start()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
        BUILDING_MANAGER = BuildingManager.BUILDING_MANAGER;
        CANVAS_MANAGER.ShowMeatAmount(meatAmount, maxMeatAmount);
        UpdateWaveLevel();
        UpdateWoodAmount(0);
        UpdateGoldAmount(0);
        UpdateMeatAmount(0);
    }
    /// <summary>
    /// Updates the wood amount and displays the wood amount to the screen
    /// </summary>
    /// <param name="woodAmount"></param>
    public void UpdateWoodAmount(int woodAmount)
    {
        this.woodAmount += woodAmount;
        CANVAS_MANAGER.ShowWoodAmount(this.woodAmount);
        CANVAS_MANAGER.UpdateButtons();

    }
    /// <summary>
    /// Updates the gold amount and displays the gold amount to the screen
    /// </summary>
    /// <param name="goldAmount"></param>
    public void UpdateGoldAmount(int goldAmount)
    {
        this.goldAmount += goldAmount;
        CANVAS_MANAGER.ShowGoldAmount(this.goldAmount);
        CANVAS_MANAGER.UpdateButtons();
    }
    /// <summary>
    /// Updates the meat amount and displays the meat amount to the screen
    /// </summary>
    /// <param name="meatAmount"></param>
    public void UpdateMeatAmount(int meatAmount)
    {
        this.meatAmount += meatAmount;
        if (this.meatAmount == this.maxMeatAmount)
        {
            BUILDING_MANAGER.NotifyLumberMillOnMaximumMeatAmount(true);
        }
        else
        {
            BUILDING_MANAGER.NotifyLumberMillOnMaximumMeatAmount(false);
        }
        CANVAS_MANAGER.ShowMeatAmount(this.meatAmount,maxMeatAmount);
        CANVAS_MANAGER.UpdateButtons();
    }
    /// <summary>
    /// Updates the wave level and displays the wave level to the screen
    /// </summary>
    public void UpdateWaveLevel()
    {
        CANVAS_MANAGER.ShowWaveLevel(++waveLevel);
    }
}
