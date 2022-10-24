using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : PlayerBaseState
{
    private float CrossFadeDuration = 0.4f;

    private readonly int FallHash = Animator.StringToHash("Fall");

    private Vector3 momentum;

    public PlayerFallingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {  
        stateMachine.LedgeDetector.OnLedgeDetect += OnLedgeDetect;

        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(FallHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if(stateMachine.Controller.velocity.y >= 0f && stateMachine.Controller.isGrounded)
        {
            ReturnToLocomotion();
        }

        FaceTarget();
    }    

    public override void Exit()
    {
        stateMachine.LedgeDetector.OnLedgeDetect -= OnLedgeDetect;
    }

    private void OnLedgeDetect(Vector3 ledgeForward)
    {
        stateMachine.SwitchState(new PlayerHangingState(stateMachine, ledgeForward));
    }
}
