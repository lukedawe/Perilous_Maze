using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// an interface for all states
public interface IState
{
    bool Activate(float deltaTime);
    GameObject Player { get; set; }
}
