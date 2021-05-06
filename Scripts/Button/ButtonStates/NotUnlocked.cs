using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class NotUnlocked : MonoBehaviour, IButtonState
{
    private CanvasManager CANVAS_MANAGER;
    public NotUnlocked()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
    }
    public bool Buy()
    {
        CANVAS_MANAGER.DisplayAlertMessageToUser("Building not unlocked from the shop menu!");
        return false;
    }
    public void executeAction(MyButton objectPrefab)
    {
        objectPrefab.GetComponent<Image>().sprite = objectPrefab.GetLockedButtonLook();
        objectPrefab.GetCostText().gameObject.SetActive(false);
    }
}