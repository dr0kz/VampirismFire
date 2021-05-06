using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyUpdatable : IUpdatable
{
    void update(Building building, Building nextBuilding);
}
