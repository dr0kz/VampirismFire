using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingState
{
    bool Execute(MyButton button);
}
