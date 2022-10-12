using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    private void Start()
    {
        SwitchState(new PlayerTestState(this));
    }
}
