using UnityEngine;

public class Attack_Behavior : StateMachineBehaviour
{
    float attackRange = 10; // The distance from which the bot initiates an attack
    Transform player; // Our main character 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Find the game object with the tag "Player" and get its Transform component
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Direct the bot's gaze towards the player
        animator.transform.LookAt(player);
        // The bot starts leaning when approached, so we freeze its position
        animator.transform.eulerAngles = new Vector3(0, animator.transform.eulerAngles.y, 0);
        // Calculate the distance between the bot's position and the player's position
        float distance = Vector3.Distance(animator.transform.position, player.position);
        if (distance > attackRange)
        {
            // If the distance is greater than attackRange, set the attack state to false
            animator.SetBool("IsAttacking", false);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
