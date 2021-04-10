using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    bool Activate();
    GameObject Player { get; set; }
}
