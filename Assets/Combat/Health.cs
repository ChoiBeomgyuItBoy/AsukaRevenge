using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int health;

    public event Action OnTakeDamage;

    private void Start()
    {
        health = maxHealth;
    }

    public void DealDamage(int damage)
    {
        if(health == 0) { return; }

        OnTakeDamage?.Invoke();

        health = Mathf.Max(health - damage, 0);

        Debug.Log(health);
    }
}
