using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[CreateAssetMenu]
public class Rage : Ability
{
    public float rageVelocity;

    public override void Activate(GameObject parent, GameObject camera)
    {
        var controller = parent.GetComponent<PlayerArmsController>();
        controller.walkSpeed = rageVelocity;
        camera.GetComponent<PostProcessVolume>().enabled = true;
    }

    public override void Disable(GameObject parent, GameObject camera)
    {
        var controller = parent.GetComponent<PlayerArmsController>();
        controller.walkSpeed = 2f;
        camera.GetComponent<PostProcessVolume>().enabled = false;
    }
}
