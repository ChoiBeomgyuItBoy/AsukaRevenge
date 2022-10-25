using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPullUpState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.1f;

    private readonly int ClimbingHash = Animator.StringToHash("Climb");

    public PlayerPullUpState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(ClimbingHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {     
        if(GetNormalizedTime(stateMachine.Animator, "Climb") < 1f)
        {
            Vector3 movement = new Vector3();

            movement += stateMachine.transform.up * 1 * stateMachine.PullUpLength / stateMachine.PullDuration; 
            movement += stateMachine.transform.forward * 1 * stateMachine.PullForwardLength / stateMachine.PullDuration;

            stateMachine.Controller.Move(movement * deltaTime);

            return;
        }

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    public override void Exit()
    {
        stateMachine.Controller.Move(Vector3.zero);
        stateMachine.ForceReceiver.Reset();
    }
}
