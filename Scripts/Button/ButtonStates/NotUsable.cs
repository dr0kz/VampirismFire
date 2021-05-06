using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class NotUsable : MonoBehaviour, IButtonState
{
    private CanvasManager CANVAS_MANAGER;
    public NotUsable()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
    }
    public bool Buy()
    {
        CANVAS_MANAGER.DisplayAlertMessageToUser("The spell is already activated!");
        return false;
    }

    public void executeAction(MyButton objectPrefab)
    {
        objectPrefab.GetComponent<Image>().sprite = objectPrefab.GetNotUsableButtonLook();
        objectPrefab.GetCostText().gameObject.SetActive(false);
        objectPrefab.GetWoodImage().gameObject.SetActive(false);
        objectPrefab.GetGoldImage().gameObject.SetActive(false);
    }
}
