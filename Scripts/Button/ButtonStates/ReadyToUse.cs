using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReadyToUse : MonoBehaviour, IButtonState
{
    private ResourceManager RESOURCE_MANAGER;
    private ICost prefab;
    public ReadyToUse()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
    }
    public bool Buy()
    {
        if (prefab.GetCost().Contains("wood")) RESOURCE_MANAGER.UpdateWoodAmount(-int.Parse(prefab.GetCost().Split(' ')[0]));
        else RESOURCE_MANAGER.UpdateGoldAmount(-int.Parse(prefab.GetCost().Split(' ')[0]));
        return true;
    }
    public void executeAction(MyButton objectPrefab)
    {
        prefab = objectPrefab.GetICost();
        objectPrefab.GetComponent<Image>().sprite = objectPrefab.GetNormalButtonLook();
    }
}
