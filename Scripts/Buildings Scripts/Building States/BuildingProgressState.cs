using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingProgressState : MonoBehaviour, IBuildingState
{
    private CanvasManager CANVAS_MANAGER;
    public BuildingProgressState()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
    }
    public bool Execute(MyButton button)
    {
        CANVAS_MANAGER.DisplayAlertMessageToUser("You can not execute this action while building is in progress!");
        return false;

    }
}
