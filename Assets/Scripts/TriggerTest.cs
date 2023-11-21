using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TriggerTest : MonoBehaviour
{
    [SerializeField] private PostProcessVolume volume;
    private LensDistortion lensDistortion;
    private float multiplyer; 
    private void OnTriggerStay(Collider other)
    {
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

    private void OnTriggerExit(Collider other)
    {
        volume.enabled = false;
    }
}
