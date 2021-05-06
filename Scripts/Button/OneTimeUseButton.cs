using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class OneTimeUseButton : MyButton
{
    [SerializeField] private int cost;
    [SerializeField] private string resourceType;
    protected override void Start()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
        if (currentState != notUnlockedState && currentState != notUsableState)
        {
            DisplayPriceAndDisplayResource();
        }
    }
    public override int GetPrice()
    {
        return cost;
    }
    public override string GetResource()
    {
        return resourceType;
    }
    public override void Init()
    {
        price = cost;
        resource = resourceType;
        SetState(startState);
    }
}
