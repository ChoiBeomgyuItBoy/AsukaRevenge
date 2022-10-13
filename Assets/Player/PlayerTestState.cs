using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    private float time = 0f;
    public PlayerTestState(PlayerStateMachine stateMachine) : base(stateMachine){}

    public override void Enter()
    {
    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMovement();

        // Camera Mouse and Input related movement:
        // Get the Main Camera,  Delta Mouse, and Input Action values - multiply them by deltaTime 
        stateMachine.Controller.Move(movement * stateMachine.FreeLookMovementSpeed * deltaTime); 

        if(stateMachine.InputReader.MovementValue == Vector2.zero) 
        { 
            stateMachine.Animator.SetFloat("FreeLookSpeed", 0f, 0.1f, Time.deltaTime);
            return;
        }

        stateMachine.Animator.SetFloat("FreeLookSpeed", 1f, 0.1f, Time.deltaTime);
        stateMachine.transform.rotation = Quaternion.LookRotation(movement);
    }

    public override void Exit()
    {
    }

    private Vector3 CalculateMovement()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right= stateMachine.MainCameraTransform.right;

        forward.y = 0f; 
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }
}
