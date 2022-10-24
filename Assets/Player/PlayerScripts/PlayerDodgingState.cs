using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly int DodgingBlendTreeHash = Animator.StringToHash("DodgingBlendTree");
    private readonly int DodgingForwardHash = Animator.StringToHash("DodgingForward");
    private readonly int DodgingRightHash = Animator.StringToHash("DodgingRight");

    private Vector3 dodgingDirectionInput;
    private float remainingDodgeTime;

    
    public PlayerDodgingState(PlayerStateMachine stateMachine, Vector3 dodgingDirectionInput) : base(stateMachine) 
    { 
        this.dodgingDirectionInput = dodgingDirectionInput;
    }

    public override void Enter()
    {
        remainingDodgeTime = stateMachine.DodgeDuration;

        stateMachine.Animator.SetFloat(DodgingForwardHash, dodgingDirectionInput.y);
        stateMachine.Animator.SetFloat(DodgingRightHash, dodgingDirectionInput.x);
        stateMachine.Animator.CrossFadeInFixedTime(DodgingBlendTreeHash, CrossFadeDuration);

        stateMachine.Health.SetInvulnerable(true);
        stateMachine.Controller.detectCollisions = false;
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * dodgingDirectionInput.x * stateMachine.DodgeLength / stateMachine.DodgeDuration;
        movement += stateMachine.transform.forward * dodgingDirectionInput.y * stateMachine.DodgeLength / stateMachine.DodgeDuration;

        Move(movement, deltaTime);

        FaceTarget();

        remainingDodgeTime -= deltaTime;

        if(remainingDodgeTime <= 0f)
        {
            stateMachine.SwitchState(new PlayerTargetingState(stateMachine));
        }
    }

    public override void Exit()
    {
        stateMachine.Health.SetInvulnerable(false);
        stateMachine.Controller.detectCollisions = true;
    }
}
