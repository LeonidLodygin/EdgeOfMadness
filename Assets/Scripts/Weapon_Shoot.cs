using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.VFX;

public class Weapon_Shoot : MonoBehaviour
{
    public ParticleSystem _particleSystem;

    public float damage = 10f;
    public float range = 100f;
    int impactForce = 30000;
    Transform enemy;
    Transform player;
    Animator enemyAnimator;

    public VisualEffect muzzleFlash;
    public Transform firePoint;


    private float nextTimeToFire;
    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy1").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        enemyAnimator = enemy.GetComponent<Animator>();

    }
    // Update is called once per frame
    void Update()
    {
        nextTimeToFire += 1 * Time.deltaTime;
        if (nextTimeToFire>1.2 && enemyAnimator.GetBool("IsAttacking"))
        {
            nextTimeToFire = 0;
            Shoot();
        }
    }

    void Shoot()
    {
        muzzleFlash.Play();
        
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit))
        {
            Debug.Log(hit.transform.name);

            Health target = hit.transform.GetComponent<Health>();

            if (target != null)
            {
                target.TakeDamage(damage);
            }
            if (hit.rigidbody != null)
            {
                /// hit.rigidbody.AddForce(-hit.normal * impactForce);
                hit.rigidbody.AddForceAtPosition(-hit.normal * impactForce, hit.point);
            }
        };
    }



}
