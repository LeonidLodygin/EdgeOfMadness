using UnityEngine;

/// <summary>
/// Player weapon processing, including animation and property implementation
/// </summary>
public class WeaponController : MonoBehaviour
{
    [SerializeField] private PlayerArmsController controller;
    
    public float swayAmount;
    public float swaySmoothing;
    public float swayResetSmoothing;
    public float swayClampX;
    public float swayClampY;
    public float movementSwayX;
    public float movementSwayY;
    public float movementSwaySmoothing;
    public float swayAmountA = 1f;
    public float swayAmountB = 2f;
    public float swayScale = 100f;
    public float swayLerpSpeed = 14f;
    public float swayTime;
    public Vector3 swayPosition;
    
    
    
    private InputManager inputManager;
    public Animator weaponAnimator;
    public Transform weaponSway;

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

        WeaponSway();
        WeaponRotation();
        WeaponAnimator();
    }

    /// <summary>
    /// Realization of weapon rotation following the player's gaze
    /// </summary>
    private void WeaponRotation()
    {
        weaponAnimator.speed = controller.weaponAnimationSpeed;
         
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
    
    /// <summary>
    /// Switching weapon animations depending on whether the player is running or not
    /// </summary>
    private void WeaponAnimator()
    {
        weaponAnimator.SetBool("isRunning", inputManager.Run);
    }

    /// <summary>
    /// Realization of smooth rocking of the weapon at rest
    /// </summary>
    private void WeaponSway()
    {
        var targetPosition = LissajousCurve(swayTime, swayAmountA, swayAmountB) / swayScale;

        swayPosition = Vector3.Lerp(swayPosition, targetPosition, Time.smoothDeltaTime * swayLerpSpeed);
        swayTime += Time.deltaTime;
        if (swayTime > 6.3f)
        {
            swayTime = 0;
        }
        
        weaponSway.localPosition = swayPosition;
    }

    /// <summary>
    /// Calculating the Lissajous figure
    /// </summary>
    /// <param name="time">Current time</param>
    /// <param name="a">Parameter affecting scaling along the axis</param>
    /// <param name="b">Parameter affecting frequency and phase of oscillation</param>
    /// <returns>Current position on Lissajous figure</returns>
    private static Vector3 LissajousCurve(float time, float a, float b)
    {
        return new Vector3(Mathf.Sin(time), a * Mathf.Sin(b * time + Mathf.PI));
    }
}
