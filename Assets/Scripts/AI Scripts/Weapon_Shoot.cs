using UnityEngine;
using UnityEngine.VFX;

public class Weapon_Shoot : MonoBehaviour
{
    Transform enemy; // Enemy
    Transform player; // Player
    Animator enemyAnimator; // Enemy's animator

    public VisualEffect muzzleFlash; // Visual effect for the muzzle flash
    public Transform firePoint; // Point from where the shots "fly out"

    private float nextTimeToFire; // Time until the next shot

    private void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy1").transform;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        enemyAnimator = enemy.GetComponent<Animator>();
    }

    void Update()
    {
        // Increase time since the last shot
        nextTimeToFire += 1 * Time.deltaTime;

        // Check if enough time has passed for the next shot
        if (nextTimeToFire > 1.2)
        {
            nextTimeToFire = 0;  // Reset time until the next shot
            Shoot(); // Initiate the shot
        }
    }

    // Method for performing a shot, including the visual flash effect and hit detection
    void Shoot()
    {
        // Play the visual flash effect
        muzzleFlash.Play();

        // Send a ray determining the hit
        RaycastHit hit;
        if (Physics.Raycast(enemy.transform.position, enemy.transform.forward, out hit))
        {

            // Get the Health component of the target, if available
            Health target = hit.transform.GetComponent<Health>();

            // If the target has a Health component, inflict damage (randomly determined)
            if (target != null)
            {
                // Determine damage using the DetectionDamage method
                float damage = DetectionDamage();
                // Inflict damage
                target.TakeDamage(damage);
            }
        };
    }

    // Method to determine damage
    float DetectionDamage()
    {
        int range = Random.Range(1, 10);
        if (range <= 1)
        {
            // Headshot
            return 50f;
        }
        else if (range <= 4)
        {
            // Body shot
            return 25f;
        }
        else
        {
            // Limb shot
            return 10f;
        }
    }
}
