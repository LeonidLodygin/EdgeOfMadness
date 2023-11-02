using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float swayAmount;
    public float swaySmoothing;
    public float swayResetSmoothing;
    public float swayClampX;
    public float swayClampY;
    public float movementSwayX;
    public float movementSwayY;
    public float movementSwaySmoothing;
    
    
    private InputManager inputManager;

    private bool isInitialised;
    
    private Vector3 newWeaponRotation;
    private Vector3 newWeaponRotationVelocity;
    private Vector3 targetWeaponRotation;
    private Vector3 targetWeaponRotationVelocity;
    
    private Vector3 newWeaponMovementRotation;
    private Vector3 newWeaponMovementRotationVelocity;
    private Vector3 targetWeaponMovementRotation;
    private Vector3 targetWeaponMovementRotationVelocity;
    
    public void Initialise(InputManager manager)
    {
        inputManager = manager;
        isInitialised = true;
    }

    private void Start()
    {
        newWeaponRotation = transform.localRotation.eulerAngles;
    }

    private void LateUpdate()
    {
        if (!isInitialised) return;
         
        var mouseX = inputManager.Look.x;
        var mouseY = inputManager.Look.y;

        targetWeaponRotation.x -= mouseY * swayAmount * Time.smoothDeltaTime;
        targetWeaponRotation.y += mouseX * swayAmount * Time.smoothDeltaTime;
        targetWeaponRotation.x = Mathf.Clamp(targetWeaponRotation.x, -swayClampX, swayClampX);
        targetWeaponRotation.y = Mathf.Clamp(targetWeaponRotation.y, -swayClampY, swayClampY);

        targetWeaponRotation = Vector3.SmoothDamp(targetWeaponRotation, Vector3.zero, ref targetWeaponRotationVelocity, swayResetSmoothing);
        newWeaponRotation = Vector3.SmoothDamp(newWeaponRotation, targetWeaponRotation, ref newWeaponRotationVelocity, swaySmoothing);
        
        targetWeaponMovementRotation.z = -movementSwayX * inputManager.Movement.x;
        targetWeaponMovementRotation.x = movementSwayY * inputManager.Movement.y;
        
        targetWeaponMovementRotation = Vector3.SmoothDamp(targetWeaponMovementRotation, Vector3.zero, ref targetWeaponMovementRotationVelocity, swayResetSmoothing);
        newWeaponMovementRotation = Vector3.SmoothDamp(newWeaponMovementRotation, targetWeaponMovementRotation, ref newWeaponMovementRotationVelocity, movementSwaySmoothing);
        
        
        transform.localRotation = Quaternion.Euler(newWeaponRotation + newWeaponMovementRotation);
    }
}
