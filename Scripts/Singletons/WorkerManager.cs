using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Diagnostics;
public class WorkerManager : CharacterManager
{
    #region Singleton
    public static WorkerManager WORKER_MANAGER;
    #endregion
    private Dictionary<Worker, Dictionary<Renderer, List<Color>>> workerColorsDictionary; //workers colors
    private CanvasManager CANVAS_MANAGER; //Reference to CanvasManager singleton
    /// <summary>
    /// Called on script loaded
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        WORKER_MANAGER = this;
        workerColorsDictionary = new Dictionary<Worker, Dictionary<Renderer, List<Color>>>();
    }
    /// <summary>
    /// Called on the first frame
    /// </summary>
    private void Start()
    {
        CANVAS_MANAGER = CanvasManager.CANVAS_MANAGER;
    }
    /// <summary>
    /// Register worker and maps every worker into dictionary of renderers
    /// </summary>
    /// <param name="_object">Worker</param>
    public override void register(IUpdatable _object)
    {
        base.register(_object);
        Worker worker = _object as Worker;
        List<Color> listColors;
        workerColorsDictionary.Add(worker, new Dictionary<Renderer, List<Color>>());
        Dictionary<Renderer, List<Color>> renderers = new Dictionary<Renderer, List<Color>>();
        workerColorsDictionary.TryGetValue(worker, out renderers);

        worker.GetComponentsInChildren<Renderer>()
            .Where(renderer => renderer != null)
            .ToList()
            .ForEach(renderer =>
            {
                renderers.Add(renderer, new List<Color>());
                renderers.TryGetValue(renderer, out listColors);
                foreach (Material material in renderer.materials)
                    listColors.Add(material.GetColor("_BaseColor"));
            });
    }
    /// <summary>
    /// Removes the worker from the updatableObjects list
    /// Removes the worker from the workerColorsDictionary
    /// If worker was currently selected then set the currently selected object to null
    /// </summary>
    /// <param name="_object"></param>
    public override void remove(IUpdatable _object)
    {
        base.remove(_object);
        workerColorsDictionary.Remove(_object as Worker);
        if ((object)CANVAS_MANAGER.GetSelectedObject() == this)
        {
            CANVAS_MANAGER.SetSelectedObject(null);
        }
    }
    /// <summary>
    /// Changes each color of each renderer
    /// </summary>
    /// <param name="isSelected">If true then every worker get new color, else workers get their previous color</param>
    public void ChangeWorkersColor(bool isSelected) // O(n^3) worst case => O(n) best case
    {
        Dictionary<Renderer, List<Color>> renderers;
        Material[] materials;
        List<Color> colors;
    
        updatableObjects
            .Select(worker => worker as Worker)
            .ToList()
            .ForEach(worker =>
            {
                workerColorsDictionary.TryGetValue(worker, out renderers);

                worker.GetComponentsInChildren<Renderer>()
                .Where(renderer => renderer != null && !renderer.transform.tag.Equals("colorNotChangeable"))
                .ToList()
                .ForEach(renderer => {
                    if (isSelected) renderer.materials.ToList().ForEach(material => material.SetColor("_BaseColor", Color.green));
                    else
                    {
                        renderers.TryGetValue(renderer, out colors);
                        materials = renderer.materials;
                        for (int i = 0; i < materials.Count(); i++)
                        {
                            materials[i].SetColor("_BaseColor", colors[i]);
                        }
                    }
                });
            });
    }
    public void notifyUpdatableOnBuildingSpawned()
    {
        updatableObjects.ForEach(character => character.update());
    }
    public void notifyUpdatableOnBuildingDestroyed(Building building)
    {
        updatableObjects.ForEach(character => ((IWorkerUpdatable)character).update(building));
    }
    public void notifyUpdatableOnBuildingUpgrade(Building building, Building nextBuilding)
    {
        updatableObjects.ForEach(character => ((IWorkerUpdatable)character).update(building, nextBuilding));
    }
}
