using UnityEngine;

/// <summary>
/// Implementation of short teleportation of the player to the side of the gaze
/// </summary>
[CreateAssetMenu]
public class Blink : Ability
{
    public GameObject obj;
    
    public override void Activate(GameObject parent, GameObject camera)
    {
        /*RaycastHit hitInfo;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);
        GameObject pointer = Instantiate(obj, Vector3.zero, camera.transform.rotation);
        Debug.Log("Объект создан");
        while (GetState() == State.active)
        {
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hitInfo, 2000.0f))
            {
                //pointer.SetActive(true);
                pointer.transform.position = hitInfo.point;
            }
        }
        Destroy(pointer);*/
        parent.transform.position = Vector3.zero;
    }
}
