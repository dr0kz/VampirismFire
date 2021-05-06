using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IButtonState : IExecuteAction<MyButton>
{
    bool Buy();
}
