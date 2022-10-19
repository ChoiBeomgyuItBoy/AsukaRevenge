using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBaseState : State
{
    protected EnemyStateMachine stateMachine;

    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    protected void Move(Vector3 motion, float deltaTime)
    {
        stateMachine.Controller.Move((motion + stateMachine.ForceReceiver.Movement) * deltaTime);
    }

    protected void Move(float deltaTime) 
    {
        Move(Vector3.zero, deltaTime);
    }

    protected void FacePlayer()
    {
        if(stateMachine.Player == null) { return; }

        Vector3 lookPosition = stateMachine.Player.transform.position - stateMachine.transform.position;
        lookPosition.y = 0;

        stateMachine.transform.rotation = Quaternion.LookRotation(lookPosition);
    }

    protected bool IsInChaseRange()
    {
        Vector3 playerPosition = stateMachine.Player.transform.position;
        Vector3 myPosition = stateMachine.transform.position;

        float playerDistanceSqr = (playerPosition - myPosition).sqrMagnitude;
        float chasingRangeSqr = stateMachine.PlayerChasingRange * stateMachine.PlayerChasingRange;

        return playerDistanceSqr <= chasingRangeSqr;
    }
}
