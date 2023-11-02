using UnityEngine;

public class PlayerArmsController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private InputManager inputManager;
    [SerializeField] private Transform сameraRoot;
    [SerializeField] private Transform сamera;
    private float xRotation;
    private bool grounded;
    public float weaponAnimationSpeed;

    private const float walkSpeed = 2f;
    private const float runSpeed = 6f;
    [SerializeField] private float upperLimit = -90f;
    [SerializeField] private float bottomLimit = 90f;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float jumpForce = 260f;
    [SerializeField] private float distanceToGround = 0.8f;
    [SerializeField] private LayerMask Ground;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }
    
    private void FixedUpdate()
    {
        GroundCheck();
        Move();
        Jump();
    }
    
    private void LateUpdate()
    {
        CameraMovement();
    }
    
    private void Move()
    {
        var targetSpeed = inputManager.Run ? runSpeed : walkSpeed;
        if (inputManager.Movement == Vector2.zero) targetSpeed = 0;
        if (!grounded) return;
        var currentVelocity = new Vector3(inputManager.Movement.x, 0, inputManager.Movement.y);
        currentVelocity *= targetSpeed;
        currentVelocity = transform.TransformDirection(currentVelocity);
        var velocity = rigidbody.velocity;
        weaponAnimationSpeed = targetSpeed;
        if (weaponAnimationSpeed > 1)
            weaponAnimationSpeed = 1;
        var velocityChange = new Vector3(currentVelocity.x - velocity.x, 0, currentVelocity.z - velocity.z);

        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    private void Jump()
    {
        if (grounded && inputManager.Jump)
        {
            rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

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
}
