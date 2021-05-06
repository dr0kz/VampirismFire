using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterManager : MonoBehaviour, ISubject
{
    protected List<IUpdatable> updatableObjects;
    /// <summary>
    /// Called when the script is loaded
    /// </summary>
    protected virtual void Awake()
    {
        updatableObjects = new List<IUpdatable>();
    }
    /// <summary>
    /// Registers new IUpdatable object
    /// </summary>
    public virtual void register(IUpdatable _object)
    {
        updatableObjects.Add(_object);
    }
    /// <summary>
    /// Removes IUpdatable object
    /// </summary>
    public virtual void remove(IUpdatable _object)
    {
        updatableObjects.Remove(_object);
    }
    public List<IUpdatable> GetUpdatableObjects()
    {
        return updatableObjects;
    }

}
