using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health=100;
    public Animator animator;
    void Update()
    {
        if (health <= 0)
        {
            GetComponent<Collider>().enabled = false;
            animator.SetTrigger("DeathTrig");
            animator.SetBool("IsPatroling", false);
            animator.SetBool("IsChasing", false);
            animator.SetBool("IsAttacking", false);
            
        }
    }

    public void TakeDamage(int damage) 
    { 
        health -= damage;

        if (health <= 0)
        {
            animator.SetTrigger("DeathTrig");
            GetComponent<Collider>().enabled = false;
        }
    }
}
