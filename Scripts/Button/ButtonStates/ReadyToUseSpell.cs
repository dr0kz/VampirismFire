using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReadyToUseSpell : MonoBehaviour, IButtonState
{
    private ResourceManager RESOURCE_MANAGER;
    private MyButton prefab;
    public ReadyToUseSpell()
    {
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
    }
    public bool Buy()
    {
        if (prefab.GetResource().Equals("wood")) RESOURCE_MANAGER.UpdateWoodAmount(-prefab.GetPrice());
        else RESOURCE_MANAGER.UpdateGoldAmount(-prefab.GetPrice());
        return true;
    }
    public void executeAction(MyButton objectPrefab)
    {
        prefab = objectPrefab;
        objectPrefab.GetComponent<Image>().sprite = objectPrefab.GetNormalButtonLook();
    }
}
