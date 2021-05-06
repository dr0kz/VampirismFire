using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BuildingManager : MonoBehaviour
{
    #region Singleton
    public static BuildingManager BUILDING_MANAGER;
    #endregion
    [SerializeField] private BigWall bigWall; //This wall is instantiated after 3 default walls are merged
    private List<Building> buildingsList; //list of all buildings in the scene
    private Dictionary<Building, Dictionary<Renderer, Color>> buildingsColor; //storing buildings colors
    private Dictionary<float, List<Wall>> wallsDictionary; //mapping the x and z coordinate of wall into list of walls that have same x or z coordinate
    /// <summary>
    /// On Scene Load this method is called first
    /// </summary>
    private void Awake()
    {
        BUILDING_MANAGER = this;
        buildingsList = new List<Building>();
        buildingsColor = new Dictionary<Building, Dictionary<Renderer, Color>>();
        wallsDictionary = new Dictionary<float, List<Wall>>();
    }
    /// <summary>
    /// Adds Building to the building list and stores its renderer ( colors ) into Dictionary
    /// If the Building is Wall then this method add the wall to wall dictionary
    /// </summary>
    /// <param name="building"></param>
    public virtual void addBuilding(Building building)
    {
        buildingsList.Add(building);
        Dictionary<Renderer, Color> colorsDictionary;
        buildingsColor.Add(building, new Dictionary<Renderer, Color>());

        buildingsColor.TryGetValue(building, out colorsDictionary);

        foreach (Renderer renderer in building.GetComponentsInChildren<Renderer>())
        {
            if (renderer != null) colorsDictionary.Add(renderer, renderer.material.GetColor("_BaseColor"));
        }
        if (building is Wall wall) AddWallToDictionary(wall);
    }
    /// <summary>
    /// Remove building from the building list and buildingsColor dictionary
    /// if the building is wall then the method removes the building from the wall dictionary
    /// </summary>
    /// <param name="building"></param>
    public virtual void removeBuilding(Building building)
    {
        buildingsList.Remove(building);
        buildingsColor.Remove(building);
        if (building is Wall wall) RemoveWallFromDictionary(wall);
    }
    public List<Building> getBuildings()
    {
        return buildingsList;
    }
    /// <summary>
    /// Change all buildings color
    /// </summary>
    /// <param name="isSelected">Is Building seleted</param>
    public void ChangeBuildingsColor(bool isSelected) //O(n^2) -> O(n) best case scenarion
    {
        Dictionary<Renderer, Color> colors;
        Color rendererColor;
        buildingsList.ForEach(building =>
        {
            foreach (Renderer renderer in building.GetComponentsInChildren<Renderer>())
            {
                if (renderer != null)
                {
                    if (isSelected) renderer.material.SetColor("_BaseColor", Color.red);
                    else
                    {
                        buildingsColor.TryGetValue(building, out colors);
                        colors.TryGetValue(renderer, out rendererColor);
                        renderer.material.SetColor("_BaseColor", rendererColor);
                    }
                }
            }
        });
    }
    /// <summary>
    /// Change building color on building progress
    /// </summary>
    /// <param name="building">Building that is placed or upgraded</param>
    /// <param name="onBuilding">Building progress</param>
    public void ChangeBuildingColor(Building building, bool onBuilding)
    {
        Dictionary<Renderer, Color> colors;
        Color rendererColor = Color.gray;
        rendererColor.a = 0.85f;
        foreach (Renderer renderer in building.GetComponentsInChildren<Renderer>())
        {
            if(renderer != null)
            {
                if (onBuilding)
                {
                    renderer.material.SetColor("_BaseColor", rendererColor);
                }
                else
                {
                    buildingsColor.TryGetValue(building, out colors);
                    colors.TryGetValue(renderer, out rendererColor);
                    renderer.material.SetColor("_BaseColor", rendererColor);
                }
            }

        }

    }/// <summary>
    /// Add wall to walls dictionary and checks whether along the x axis or z axis merging walls is possible
    /// </summary>
    /// <param name="wall">Placed wall in the scene</param>
    private void AddWallToDictionary(Wall wall)
    {
        if (!wallsDictionary.ContainsKey(wall.transform.position.x)) wallsDictionary.Add(wall.transform.position.x, new List<Wall>());
        if (!wallsDictionary.ContainsKey(wall.transform.position.z)) wallsDictionary.Add(wall.transform.position.z, new List<Wall>());
        List<Wall> list1 = new List<Wall>();
        wallsDictionary.TryGetValue(wall.transform.position.x, out list1);
        list1.Add(wall);
        List<Wall> list2 = new List<Wall>();
        wallsDictionary.TryGetValue(wall.transform.position.z, out list2);
        list2.Add(wall);

        if (!MergeWalls(list1, true)) MergeWalls(list2, false);
    }
    /// <summary>
    /// Tries to merge 3 walls in the scene that are next to each other, have same y coordinate and same x coordinate or z coordinate
    /// </summary>
    /// <param name="walls">List of walls</param>
    /// <param name="rotateWall">false if the list contains walls that have the same x coordinate value, true for the z axis </param>
    /// <returns></returns>
    private bool MergeWalls(List<Wall> walls, bool rotateWall) // O(n + nlog(n))
    {
        walls.Sort((k, v) => CompareWalls(k, v));
        for (int i = 1; i < walls.Count - 1; i++)
        {
            if (CheckIfMergingPossible(walls[i - 1], walls[i], walls[i + 1]))
            {
                BigWall wall = Instantiate(bigWall);
                if (rotateWall) wall.transform.Rotate(0, 90, 0, Space.World);
                wall.SetStandingPlane(walls[i].GetStandingPlane());
                List<PlaneBuilder> standingPlanes = new List<PlaneBuilder>();
                standingPlanes.Add(walls[i - 1].GetStandingPlane());
                standingPlanes.Add(walls[i + 1].GetStandingPlane());
                wall.SetStandingPlanes(standingPlanes);
                walls[i + 1].DestroyBuilding(null);
                walls[i].DestroyBuilding(null);
                walls[i - 1].DestroyBuilding(null);
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// Comparator for comparing walls x position and z possition used for sorting the walls List provided in MergeWalls method
    /// </summary>
    private int CompareWalls(Wall x, Wall y)
    {
        return x.transform.position.x.CompareTo(y.transform.position.x) == 0 ?
        x.transform.position.z.CompareTo(y.transform.position.z) : x.transform.position.x.CompareTo(y.transform.position.x);
    }
    /// <summary>
    /// Removes wall from walls dictionary
    /// </summary>
    /// <param name="wall">Wall being removed</param>
    private void RemoveWallFromDictionary(Wall wall)
    {
        List<Wall> list = new List<Wall>();
        wallsDictionary.TryGetValue(wall.transform.position.x, out list);
        list.Remove(wall);
        list = new List<Wall>();
        wallsDictionary.TryGetValue(wall.transform.position.z, out list);
        list.Remove(wall);
    }
    /// <summary>
    /// Condition for merging 3 walls
    /// 1) Every wall is default small wall
    /// 2) Distance between every 2 walls next to each other is less then predifined value
    /// </summary>
    private bool CheckIfMergingPossible(Wall x, Wall y, Wall z)
    {
        return (!(x is BigWall) && !(y is BigWall) && !(z is BigWall) &&
            Mathf.Abs(Vector3.Distance(y.transform.position, x.transform.position)) < 3.3f
            && Mathf.Abs(Vector3.Distance(y.transform.position, z.transform.position)) < 3.3f);
    }
    public void NotifyLumberMillOnMaximumMeatAmount(bool isMaxMeatExceeded)
    {
        buildingsList
            .Where(building => building is LumberMill)
            .Select(building => building as LumberMill)
            .ToList()
            .ForEach(lumberMill => {
                if (isMaxMeatExceeded) lumberMill.SetState(lumberMill.GetMaximumMeatState());
                else lumberMill.SetState(lumberMill.GetReadyState());               
            });
    }
}
