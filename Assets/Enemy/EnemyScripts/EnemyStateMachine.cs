using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : StateMachine
{
   [field: SerializeField] public Animator Animator { get; private set; }
   [field: SerializeField] public CharacterController Controller { get; private set; }
   [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
   [field: SerializeField] public NavMeshAgent Agent { get; private set; }
   [field: SerializeField] public WeaponDamage Weapon { get; private set; }
   [field: SerializeField] public Health Health { get; private set; }

   [field: SerializeField] public float MovingSpeed { get; private set; }
   [field: SerializeField] public float PlayerChasingRange { get; private set; }
   [field: SerializeField] public float AttackRange { get; private set; }
   [field: SerializeField] public int AttackDamage { get; private set; }
   [field: SerializeField] public float AttacKnockback { get; private set; }

   public GameObject Player { get; private set; }

   private void OnEnable()
   {
      Health.OnTakeDamage += HandleTakeDamage;
   }

   private void Start()
   {
      Player = GameObject.FindGameObjectWithTag("Player");

      Agent.updatePosition = false;
      Agent.updateRotation = false;

      SwitchState(new EnemyIdleState(this));
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.red;

      Gizmos.DrawWireSphere(transform.position, PlayerChasingRange);
   }

   private void HandleTakeDamage()
    {
        SwitchState(new EnemyImpactState(this));
    }

   private void OnDisable()
   {
      Health.OnTakeDamage -= HandleTakeDamage;
   }
}
