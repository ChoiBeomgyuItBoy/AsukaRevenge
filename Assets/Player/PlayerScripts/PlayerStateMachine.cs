using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public Targeter Targeter { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Attack[] Attacks { get; private set; }
    [field: SerializeField] public WeaponDamage Weapon { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }    
    [field: SerializeField] public Ragdoll Ragdoll { get; private set; } 
    [field: SerializeField] public LedgeDetector LedgeDetector { get; private set; } 

    [field: SerializeField] public float FreeLookMovementSpeed { get; private set; }
    [field: SerializeField] public float TargetingMovementSpeed { get; private set; }
    [field: SerializeField] public float RotationDamping { get; private set; }
    [field: SerializeField] public float DodgeDuration { get; private set; }
    [field: SerializeField] public float DodgeLength { get; private set; }
    [field: SerializeField] public float DodgeCooldown { get; private set; }
    [field: SerializeField] public float JumpForce { get; private set; }
    [field: SerializeField] public float PullUpLength { get; private set; }
    [field: SerializeField] public float PullForwardLength { get; private set; }
    [field: SerializeField] public float PullDuration { get; private set; }

    public Transform MainCameraTransform { get; private set; }
    public float PreviousDodgeTime { get; private set; } = Mathf.NegativeInfinity;

     private void OnEnable()
     {
        Health.OnTakeDamage += OnTakeDamage;
        Health.OnDie += OnDie;
     }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        MainCameraTransform = Camera.main.transform;

        SwitchState(new PlayerFreeLookState(this));
    }

    public void SetDodgeTime(float dodgeTime)
    {
        PreviousDodgeTime = dodgeTime;
    }

    private void OnTakeDamage()
    {
        SwitchState(new PlayerImpactState(this));
    }

    private void OnDie()
    {
        SwitchState(new PlayerDeadState(this));
    }

    private void OnDisable()
    {
        Health.OnTakeDamage -= OnTakeDamage;
        Health.OnDie -= OnDie;
    }
}
