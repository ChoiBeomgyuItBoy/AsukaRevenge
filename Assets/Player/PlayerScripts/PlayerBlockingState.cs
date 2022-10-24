using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlockingState : PlayerBaseState
{
    private readonly int BlockHash = Animator.StringToHash("Block");

    private const float CrossFadeDuration = 0.2f;

    public PlayerBlockingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Health.SetInvulnerable(true);

        stateMachine.Animator.CrossFadeInFixedTime(BlockHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(deltaTime);

        FaceTarget();

        if(!stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }

        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
            return;
        }
    }

    public override void Exit()
    { 
        stateMachine.Health.SetInvulnerable(false);
    }
}
