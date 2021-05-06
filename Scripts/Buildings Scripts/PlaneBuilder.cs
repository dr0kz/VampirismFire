using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaneBuilder : MonoBehaviour, ISelectable, ITask
{
    [SerializeField] private MyButton[] buttons;
    [SerializeField] private MonoBehaviour[] buildings;

    private Dictionary<MyButton, MonoBehaviour> buildingDictionary;
    private bool isEnemyOverPlane = false;
    private CanvasManager CANVAS_MANAGER;
    private BuildingManager BUILDING_MANAGER;
    private Color planeColor;
    private List<Enemy> currentlyCollidingWith;
    private void Start()
    {
        currentlyCollidingWith = new List<Enemy>();
        planeColor = this.GetComponent<Renderer>().material.GetColor("_Color");
        buildingDictionary = new Dictionary<MyButton, MonoBehaviour>();
        for (int i = 0; i < Mathf.Min(buttons.Length, buildings.Length); i++)
            buildingDictionary.Add(buttons[i], buildings[i]);
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
        BUILDING_MANAGER = BuildingManager.BUILDING_MANAGER;
    }
    public List<Enemy> CurrentlyCollidingWith()
    {
        return currentlyCollidingWith;
    }
    public void Select()
    {
        if (!isEnemyOverPlane)
        {
            ChangePlaneColor(Color.cyan);
            CANVAS_MANAGER.ShowButtonsAndHandleButtonTask<MonoBehaviour>(buttons, this, buildings);
            CANVAS_MANAGER.ShowButtonsBackgroundImage(buttons.Length);
            CANVAS_MANAGER.SetSelectedObject(this);
        }
    }
    public void IsEnemyOverPlane(bool status)
    {
        isEnemyOverPlane = status;
        if (status && (object)CANVAS_MANAGER.GetSelectedObject() == this) Unselect();
    }
    public Color GetPlaneDefaultColor()
    {
        return planeColor;
    }
    public float GetPlaneHeight()
    {
        return GetComponent<MeshRenderer>().bounds.size.y;
    }
    public void ButtonTask(MyButton button)
    {
        if(button.BuyPrefab())
        {
            IInstantiatable building = (IInstantiatable)buildingDictionary[button];
            Building buildingInstantiate = Instantiate((Building)building);
            buildingInstantiate.SetStandingPlane(this);
            GetComponent<BoxCollider>().enabled = false;
            Unselect();
        }
    }
    public void Unselect()
    {
        CANVAS_MANAGER.RemoveActiveButtons();
        CANVAS_MANAGER.SetSelectedObject(null);
        ChangePlaneColor(planeColor);
    }
    public void ChangePlaneColor(Color color)
    {
        GetComponent<MeshRenderer>().material.SetColor("_BaseColor", color);
    }
    public Dictionary<MyButton, MonoBehaviour> GetDictionary()
    {
        return buildingDictionary;
    }

}
