using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTargetingState : PlayerBaseState
{
    private const float CrossFadeDuration = 0.2f;
    
    private readonly int TargetingBlendTreeHash = Animator.StringToHash("TargetingBlendTree");
    private readonly int TargetingForwardHash = Animator.StringToHash("TargetingForward");
    private readonly int TargetingRightHash = Animator.StringToHash("TargetingRight");


    public PlayerTargetingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        stateMachine.InputReader.TargetEvent += OnTarget;
        stateMachine.InputReader.DodgeEvent += OnDodge;

        stateMachine.Animator.CrossFadeInFixedTime(TargetingBlendTreeHash,CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        if(stateMachine.InputReader.IsAttacking)
        {
            stateMachine.SwitchState(new PlayerAttackingState(stateMachine, 0));
            return;
        }

        if(stateMachine.InputReader.IsBlocking)
        {
            stateMachine.SwitchState(new PlayerBlockingState(stateMachine));
            return;
        }

        if(stateMachine.Targeter.CurrentTarget == null)
        {
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
            return;
        }

        Vector3 movement = CalculateMovement(deltaTime);

        Move(movement * stateMachine.TargetingMovementSpeed, deltaTime);

        UpdateAnimator(deltaTime);

        FaceTarget();
    }

    public override void Exit()
    {
        stateMachine.InputReader.TargetEvent -= OnTarget;
        stateMachine.InputReader.DodgeEvent -= OnDodge;
    }

    private Vector3 CalculateMovement(float deltaTime)
    {
        Vector3 movement = new Vector3();

        movement += stateMachine.transform.right * stateMachine.InputReader.MovementValue.x;
        movement += stateMachine.transform.forward * stateMachine.InputReader.MovementValue.y;

        return movement;
    }

    public void UpdateAnimator(float deltaTime)
    {
        if(stateMachine.InputReader.MovementValue.y == 0f)
        {
            stateMachine.Animator.SetFloat(TargetingForwardHash, 0f, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.y > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingForwardHash, value, 0.1f, deltaTime);
        }

        if(stateMachine.InputReader.MovementValue.x == 0f)
        {
            stateMachine.Animator.SetFloat(TargetingRightHash, 0f, 0.1f, deltaTime);
        }
        else
        {
            float value = stateMachine.InputReader.MovementValue.x > 0 ? 1f : -1f;
            stateMachine.Animator.SetFloat(TargetingRightHash, value, 0.1f, deltaTime);
        }
    }

    public void OnTarget()
    {
        stateMachine.Targeter.Cancel();

        stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
    }

    public void OnDodge()
    {
        if(stateMachine.InputReader.MovementValue == Vector2.zero) { return; }

        if(Time.time - stateMachine.PreviousDodgeTime < stateMachine.DodgeCooldown) { return; }
        
        stateMachine.SetDodgeTime(Time.time);

        stateMachine.SwitchState(new PlayerDodgingState(stateMachine, stateMachine.InputReader.MovementValue));
    }
}
