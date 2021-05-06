using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class BigWall : Wall
{
    private List<PlaneBuilder> extraStandingPlanes;

    protected override void Awake()
    {
        base.Awake();
        extraStandingPlanes = new List<PlaneBuilder>();
    }
    protected override void Start()
    {
        base.Start();
        extraStandingPlanes.ForEach(standingPlane => standingPlane.GetComponent<BoxCollider>().enabled = false);
    }
    public void SetStandingPlanes(List<PlaneBuilder> extraStandingPlanes)
    {
        extraStandingPlanes.ForEach(standingPlane => this.extraStandingPlanes.Add(standingPlane));
    }
    public override void DestroyBuilding(GameObject particleEffect)
    {
        base.DestroyBuilding(particleEffect);
        extraStandingPlanes.ForEach(standingPlane => standingPlane.GetComponent<BoxCollider>().enabled = true);
        extraStandingPlanes.Clear();
    }
    public override void Init(IInstantiatable _building)
    {
        base.Init(_building);
        BigWall building = (BigWall)_building;
        transform.Rotate(0, building.transform.eulerAngles.y, 0, Space.World);
        foreach(PlaneBuilder plane in building.extraStandingPlanes) extraStandingPlanes.Add(plane);
    }    
}
