using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldFarm : Farm
{
    [SerializeField] private int intervalLength;
    [SerializeField] private int goldPerInterval;
    protected override void Start()
    {
        base.Start();
        InvokeRepeating("IncreaseGold", intervalLength, intervalLength);
    }
    private void IncreaseGold()
    {
        RESOURCE_MANAGER.UpdateGoldAmount(goldPerInterval);
    }

}
