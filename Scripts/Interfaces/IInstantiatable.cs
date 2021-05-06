using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICost
{
    string GetCost();
    bool IsUnlocked();
}

public interface IInstantiatable
{
    void Init(IInstantiatable prefab);
}
