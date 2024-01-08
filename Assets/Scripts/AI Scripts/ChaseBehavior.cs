using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : StateMachineBehaviour
{
    NavMeshAgent agent; // Bot
    Transform player; // Player
    float chaseRange = 17;  // Maximum distance to start chasing the player
    float attackRange = 10; // Distance at which the attack begins
    float longChaseRange = 25; // Distance to start chasing the player by sound

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get the NavMeshAgent component responsible for controlling the bot's navigation
        agent = animator.GetComponent<NavMeshAgent>();

        // Increase the speed of the bot during chasing
        agent.speed = 3;

        // Find the game object with the tag "Player" and get its Transform component
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Set that the bot should move towards the player
        agent.SetDestination(player.position);
        // Calculate the distance between the bot's position and the player's position
        float distance = Vector3.Distance(animator.transform.position, player.position);

        // Check if the player is within the attack distance,
        // if yes, set the attack state to true
        if (distance < attackRange)
        {
            animator.SetBool("IsAttacking", true);
        }

        // Check if the player is within the chasing distance,
        // if not, set the chasing state to false
        RaycastHit hit;

        // The cube creates a volume in which collisions are checked
        // If there is a successful collision within the specified distance (chaseRange), the collision data
        // will be stored in the hit variable
        if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
        {
            //Debug.Log(hit.transform.name);

            // If an object collides and it's not the player, reset the chasing flag
            if (!hit.transform.CompareTag("Player"))
            {
                if (!SoundDetection(distance, animator))
                {
                    animator.SetBool("IsChasing", false);
                }
            }
        }
        else
        {
            // If no collision occurred, reset the chasing flag
            animator.SetBool("IsChasing", false);
        }
    }

    private bool SoundDetection(float distance, Animator animator)
    {
        // Check if the player is within the distance for recognition by running
        if (distance < longChaseRange)
        {
            if (player.GetComponent<InputManager>().Run)
            {
                return true;
            }
        }
        return false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);

        // Set the bot's speed
        agent.speed = 2;
    }
}
