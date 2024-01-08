using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Implementation of a trigger for a dead body that affects the mental state of the player
/// </summary>
public class TriggerDeadBody : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume;
    private LensDistortion lensDistortion;
    private float multiplyer; 
    
    /// <summary>
    /// Check if the player sees the object and apply post processing to the image received by the player
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        if (GetComponent<Renderer>().isVisible)
        {
            volume.enabled = true;
            volume.profile.TryGetSettings(out lensDistortion);
            multiplyer = Mathf.MoveTowards(multiplyer, 100, 0.01f);
            lensDistortion.intensity.value = Mathf.Sin(Time.realtimeSinceStartup) * multiplyer;

        }
        else
        {
            multiplyer = Mathf.MoveTowards(multiplyer, 0, 0.01f);
            lensDistortion.intensity.value = Mathf.Sin(Time.realtimeSinceStartup) * multiplyer;
        }
    }

    /// <summary>
    /// Function to disable postprocessing when leaving the trigger zone
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        volume.enabled = false;
    }
}
