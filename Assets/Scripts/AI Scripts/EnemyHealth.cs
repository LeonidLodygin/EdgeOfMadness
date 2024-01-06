using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    float timer = 0;
    public int health=100;
    public Animator animator;
    public bool flag = false;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer>10 && !flag)
        {
            //TakeDamage(60);
            timer = 0;
            flag = true;
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
        if (0<health && health <= 50)
        {
            animator.SetTrigger("DamageTrig");
        }
    }
}
