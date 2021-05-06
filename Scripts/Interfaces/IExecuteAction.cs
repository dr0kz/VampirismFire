using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExecuteAction<T>
{
    void executeAction(T objectPrefab);
}
