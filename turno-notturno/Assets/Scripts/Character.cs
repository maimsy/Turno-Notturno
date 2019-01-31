using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    [SerializeField] int startingHealth = 5;

    protected bool dead = false;

    protected int healthRemaining;
    public float PokeTime = 0.5f;

    public int GetHealth()
    {
        return healthRemaining;
    }

    protected void InitCharacter()
    {
        healthRemaining = startingHealth;
    }

    public virtual void TakeDamage(int damage, Transform attacker)
    {
        healthRemaining -= damage;
        Debug.Log("Hit character. Health remaining: " + healthRemaining);
        if (healthRemaining <= 0 && !dead)
        {
            Die(attacker);
        }
    }

    public virtual void Die(Transform attacker)
    {
        Debug.Log("Character died!");
        dead = true;
    }
}
