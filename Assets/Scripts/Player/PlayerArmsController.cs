using UnityEngine;
using UnityEngine.Animations.Rigging;

/// <summary>
/// Realization of player movement in the game world
/// </summary>
public class PlayerArmsController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private InputManager inputManager;
    [SerializeField] private TwoBoneIKConstraint constraint;
    [SerializeField] private Transform сameraRoot;
    [SerializeField] private Transform сamera;
    private float xRotation;
    private bool grounded;
    public float weaponAnimationSpeed;

    public float walkSpeed = 2f;
    private const float runSpeed = 6f;
    [SerializeField] private float upperLimit = -90f;
    [SerializeField] private float bottomLimit = 90f;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float jumpForce = 260f;
    [SerializeField] private float distanceToGround = 0.8f;
    [SerializeField] private LayerMask Ground;
    [SerializeField] private GameObject stepRayUpper;
    [SerializeField] private GameObject stepRayLower;
    [SerializeField] private GameObject weapon;
    private Vector3 originalPos;
    private Quaternion originalRot;
    [SerializeField] float stepHeight = 0.3f;
    [SerializeField] float stepSmooth = 0.1f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        var position = stepRayUpper.transform.position;
        stepRayUpper.transform.position = new Vector3(position.x, stepHeight, position.z);
        originalPos = weapon.transform.localPosition;
        originalRot = weapon.transform.localRotation;
    }
    
    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        Jump();
        Aiming();
        StepClimb();
    }
    
    private void LateUpdate()
    {
        CameraMovement();
    }
    
    /// <summary>
    /// Character movement by applying a force based on the difference between the current speed and the one received from the input manager
    /// </summary>
    private void Move()
    {
        var targetSpeed = inputManager.Run ? runSpeed : walkSpeed;
        if (inputManager.Movement == Vector2.zero) targetSpeed = 0;
        if (!grounded) return;
        var currentVelocity = new Vector3(inputManager.Movement.x, 0, inputManager.Movement.y);
        currentVelocity *= targetSpeed;
        currentVelocity = transform.TransformDirection(currentVelocity);
        var velocity = rigidbody.velocity;
        
        // Adjusting the speed of weapon animation to the player's movement speed
        weaponAnimationSpeed = targetSpeed;
        if (weaponAnimationSpeed > 1)
            weaponAnimationSpeed = 1;
        
        var velocityChange = new Vector3(currentVelocity.x - velocity.x, 0, currentVelocity.z - velocity.z);

        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    /// <summary>
    /// Realizing a jump by applying force to the player
    /// </summary>
    private void Jump()
    {
        if (grounded && inputManager.Jump)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    /// <summary>
    /// Function to check if the player is on the surface
    /// </summary>
    private void GroundCheck()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(rigidbody.worldCenterOfMass, Vector3.down, out hitInfo, distanceToGround + 0.1f, Ground))
        {
            grounded = true;
            return;
        }
        grounded = false;
    }

    /// <summary>
    /// Function for realizing the player's stepping over low obstacles
    /// </summary>
    private void StepClimb()
    {
        var moveDirection = rigidbody.velocity.normalized;
        moveDirection.y = 0;
        moveDirection.Normalize();
        RaycastHit hitLower;
        if (!Physics.Raycast(stepRayLower.transform.position, moveDirection,
                out hitLower, 0.1f)) return;
        RaycastHit hitUpper;
        if (!Physics.Raycast(stepRayUpper.transform.position,moveDirection,
                out hitUpper, 0.2f))
        {
            rigidbody.position -= new Vector3(0f, -stepSmooth, 0f);
        }
    }

    /// <summary>
    /// Function for controlling the player's camera
    /// </summary>
    private void CameraMovement()
    {
        var mouseX = inputManager.Look.x;
        var mouseY = inputManager.Look.y;
        сamera.position = сameraRoot.position;

        xRotation -= mouseY * sensitivity * Time.smoothDeltaTime;
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);
        
        сamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(0, mouseX * sensitivity * Time.smoothDeltaTime, 0));
    }

    /// <summary>
    /// Implementation of aiming by moving the weapon and changing the "weight" of the Two Bone IK constraint component
    /// </summary>
    private void Aiming()
    {
        var targetPos = new Vector3(-0.04f, -0.045f, 0.1f);
        var targetRot = new Quaternion(-0.00137547322f,-0.998535872f,0.0443145595f,0.0309934299f);
        constraint.weight = Mathf.MoveTowards(constraint.weight, inputManager.Aim ? 1 : 0, 0.05f);
        weapon.transform.localPosition =
            Vector3.MoveTowards(weapon.transform.localPosition, inputManager.Aim ? targetPos : originalPos, 0.005f);
        weapon.transform.localRotation = Quaternion.RotateTowards(weapon.transform.localRotation, inputManager.Aim ? targetRot : originalRot, 2f);
    }

}
