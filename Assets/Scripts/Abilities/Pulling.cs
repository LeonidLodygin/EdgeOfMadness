using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Realization of player's pulling of enemies
/// </summary>
[CreateAssetMenu]
public class Pulling : Ability
{
    public override void Activate(GameObject parent, GameObject camera)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, 2000.0f))
        {
            if (hitInfo.transform.CompareTag("Enemy1"))
            {
                hitInfo.rigidbody.AddForce(-camera.transform.forward * (hitInfo.distance * 2f), ForceMode.Impulse);
            }
        }
        
    }
}
