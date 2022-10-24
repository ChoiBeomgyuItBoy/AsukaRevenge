using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : PlayerBaseState
{
    private float CrossFadeDuration = 0.2f;

    private readonly int JumpHash = Animator.StringToHash("Jump");

    private Vector3 momentum;

    public PlayerJumpingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {  
        stateMachine.LedgeDetector.OnLedgeDetect += OnLedgeDetect;

        stateMachine.ForceReceiver.Jump(stateMachine.JumpForce);

        momentum = stateMachine.Controller.velocity;
        momentum.y = 0f;

        stateMachine.Animator.CrossFadeInFixedTime(JumpHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        Move(momentum, deltaTime);

        if(stateMachine.Controller.velocity.y < 0.2f)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
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
