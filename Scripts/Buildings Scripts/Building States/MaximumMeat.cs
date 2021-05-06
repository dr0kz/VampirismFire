using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaximumMeat : IBuildingState
{
    private CanvasManager CANVAS_MANAGER;
    private ResourceManager RESOURCE_MANAGER;
    public MaximumMeat()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
        RESOURCE_MANAGER = ResourceManager.RESOURCE_MANAGER;
    }
    public bool Execute(MyButton button)
    {
        CANVAS_MANAGER.DisplayAlertMessageToUser("You can not spawn more than " + RESOURCE_MANAGER.GetMaxMeatAmount() + " workers!");
        return false;
    }
}

