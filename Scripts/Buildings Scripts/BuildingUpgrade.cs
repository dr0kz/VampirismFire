using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingUpgrade : MonoBehaviour
{
    [SerializeField] private Building[] buildings;
    
    public Building[] GetBuildings()
    {
        return buildings;
    }
}
