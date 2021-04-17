using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    bool Activate(float deltaTime);
    GameObject Player { get; set; }
}
