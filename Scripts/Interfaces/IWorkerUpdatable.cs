using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWorkerUpdatable : IUpdatable
{
    void update(Building building);
    void update(Building building, Building nextBuilding);
}
