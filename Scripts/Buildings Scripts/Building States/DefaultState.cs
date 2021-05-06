using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultState : MonoBehaviour, IBuildingState
{
    public bool Execute(MyButton button)
    {
        return button.BuyPrefab();
    }
}
