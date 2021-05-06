using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface ITask
{
    Dictionary<MyButton, MonoBehaviour> GetDictionary();
    void ButtonTask(MyButton button);
}