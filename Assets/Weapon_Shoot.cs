using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Weapon_Shoot : MonoBehaviour
{
    public ParticleSystem _particleSystem;

    public float damage = 10f;
    public float range = 100f;
    int impactForce = 30000;
    Transform enemy;
    Transform player;

    private float nextTimeToFire = 10f;

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy1").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextTimeToFire)
        {
            nextTimeToFire = Time.time + 3;
            Shoot();
        }
    }

    void Shoot()
    {
        _particleSystem.Play();
        
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
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        };
    }
}
