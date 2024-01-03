using UnityEngine;

public class Idle_Behavior : StateMachineBehaviour
{
    float timer; // Variable to track time
    Transform player; // Player
    float chaseRange = 17; // Maximum distance to start chasing the player

    float longChaseRange = 25; // If the player is at this distance and moving 
    // the player has a special variable that indicates whether they are running or not

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0; // Reset the timer to zero when entering the state

        // Find the game object with the tag "Player" and get its Transform component
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Increase the timer by the time since the last frame
        timer += Time.deltaTime;

        // The bot starts patrolling after 2 seconds
        if (timer > 2)
        { animator.SetBool("IsPatroling", true); }

        // Calculate the distance between the bot's position and the player's position
        float distance = Vector3.Distance(animator.transform.position, player.position);

        RaycastHit hit;

        // The cube creates a volume in which collisions are checked
        // If there is a successful collision within the specified distance (chaseRange), the collision data
        // will be stored in the hit variable
        if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
        {
            // If the bot sees us (i.e., the ray hits the Player and the distance is appropriate), start chasing
            if (hit.transform.CompareTag("Player"))
            {
                animator.SetBool("IsChasing", true);
            }
        }

        // Check if the player is within the distance for recognition by running
        if (distance < longChaseRange)
        {
            // Check the Run field of the player
            if (player.GetComponent<InputManager>().Run)
            {
                animator.SetBool("IsChasing", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Code to run when exiting the state, if needed
    }
}
