using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubject
{
    void register(IUpdatable _object);
    void remove(IUpdatable _object);
}
