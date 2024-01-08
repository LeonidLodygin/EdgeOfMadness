using UnityEngine;
using UnityEngine.AI;
using System.IO;
using System.Collections.Generic;
using System;

[System.Serializable]
public class PatrolBehavior : StateMachineBehaviour
{
    float timer;
    NavMeshAgent agent;
    Transform player; // Player character
    float chaseRange = 17;  // Maximum distance to start chasing the player
    float longChaseRange = 25; // Distance to start chasing the player based on sound

    public float patrolRadius = 25; // Bot patrol radius
    public Vector3 patrolCenter; // Center of the patrol sphere
    List<Vector3> previousDetections = new List<Vector3>(); // Positions where the player was detected previously

    // Method to get the "average position" for starting patrol
    Vector3 GetAveragePosition()
    {
        float sumX = 0;
        float sumZ = 0;

        foreach (Vector3 position in previousDetections)
        {
            sumX += position.x;
            sumZ += position.z;
        }

        if (previousDetections.Count > 0)
        {
            float averageX = sumX / previousDetections.Count;
            float averageY = 0;
            float averageZ = sumZ / previousDetections.Count;

            return new Vector3(averageX, averageY, averageZ);
        }
        else
        {
            return Vector3.zero; // Or another default value
        }
    }

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;

        agent = animator.GetComponent<NavMeshAgent>();

        // Set the center of patrol to the initial position of the bot
        patrolCenter = agent.transform.position;

        // Check if the bot is active and enabled on the navigation mesh
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // Set the minimum distance in front of the bot before stopping
            agent.stoppingDistance = 1.0f;
        }

        // Find the player object with the "Player" tag and get its Transform component
        player = GameObject.FindGameObjectWithTag("Player").transform;
        LoadPlayerPosition();
        patrolCenter = GetAveragePosition();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Added condition to check activity and being on NavMesh
        if (agent.isActiveAndEnabled && agent.isOnNavMesh)
        {
            // Check if the path to the destination is completed
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;

                // Generate a random point within the radius from the center of patrol
                if (RandomPoint(patrolCenter, patrolRadius, out point))
                {
                    // Set the generated random point as a new destination for the bot
                    agent.SetDestination(point);
                }
            }

            timer += Time.deltaTime;

            // After 50 seconds, the bot takes a break from patrolling
            if (timer > 50)
            {
                animator.SetBool("IsPatrolling", false);
            }

            // Similar conditions for changing distance as in the ChaseBehavior script
            float distance = Vector3.Distance(animator.transform.position, player.position);

            // Create a spherical ray cast straight in the direction of the bot
            RaycastHit hit;

            // The cube creates a volume where collisions are checked
            // If there is a successful collision within the specified distance (chaseRange),
            // the collision data will be stored in the hit variable
            if (Physics.BoxCast(animator.transform.position, new Vector3(7f, 7f, 7f) / 2f, animator.transform.forward, out hit, Quaternion.identity, chaseRange))
            {
                // If the bot sees us (i.e., the ray hits the Player and at the appropriate distance), start chasing
                if (hit.transform.CompareTag("Player"))
                {
                    animator.SetBool("IsChasing", true);
                    SavePlayerPosition(player.transform.position);
                }
            }
            if (distance < longChaseRange)
            {
                if (player.GetComponent<InputManager>().Run)
                {
                    animator.SetBool("IsChasing", true);
                    SavePlayerPosition(player.transform.position);
                }
            }
        }
    }

    // Function to generate a random point within the specified patrol radius from the given central point
    // center: Central patrol point
    // range: Radius within which a random point should be generated
    // result: Resulting random point on the NavMesh (if successfully generated).
    // Returns true if the point is successfully generated and on the NavMesh, otherwise false.
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        // Generate a random point inside a sphere with a center at the specified central point and a radius
        Vector3 randomPoint = center + UnityEngine.Random.insideUnitSphere * range;

        // Check if the generated point is on the NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 3.0f, NavMesh.AllAreas))
        {
            // If successful, save the position of the point and return true
            result = hit.position;
            return true;
        }

        // If the point is not on the NavMesh, return false
        result = Vector3.zero;
        return false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
    }

    // Save the current player position when detected
    void SavePlayerPosition(Vector3 position)
    {
        // Serialize the object to a JSON string
        string json = JsonUtility.ToJson(position);

        // Add the JSON string to the existing file
        string filePath = Path.Combine(Application.persistentDataPath, "playerPosition.json");
        System.IO.File.AppendAllText(filePath, json + System.Environment.NewLine);
    }

    // Get the list of player positions from previous detections
    void LoadPlayerPosition()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "playerPosition.json");

        if (System.IO.File.Exists(filePath))
        {
            string[] lines = System.IO.File.ReadAllLines(filePath);

            foreach (string line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    try
                    {
                        Vector3 parsedVector = JsonUtility.FromJson<Vector3>(line);
                        previousDetections.Add(parsedVector);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
