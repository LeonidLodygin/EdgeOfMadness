using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private InputManager inputManager;
    private Animator animator;
    private bool hasAnimator;
    private int xVelocityHash;
    private int yVelocityHash;
    private int crouchHash;

    [SerializeField] private Transform сameraRoot;
    [SerializeField] private Transform сamera;
    private Vector2 currentVelocity;
    private float xRotation;

    private const float crouchSpeed = 1.5f;
    private const float walkSpeed = 2f;
    private const float runSpeed = 6f;
    [SerializeField] private float animationBlendSpeed = 8.9f;
    [SerializeField] private float upperLimit = -90f;
    [SerializeField] private float bottomLimit = 90f;
    [SerializeField] private float sensitivity = 100f;

    private void Start()
    {
        hasAnimator = TryGetComponent(out animator);
        rigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();

        xVelocityHash = Animator.StringToHash("xVelocity");
        yVelocityHash = Animator.StringToHash("yVelocity");
        crouchHash = Animator.StringToHash("Crouch");
    }

    private void FixedUpdate()
    {
        Move();
        Crouch();
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    private void Move()
    {
        if (!hasAnimator) return;
        var targetSpeed = inputManager.Run ? runSpeed : inputManager.Crouch ? crouchSpeed : walkSpeed;
        if (inputManager.Movement == Vector2.zero) targetSpeed = 0;

        currentVelocity = Vector2.Lerp(currentVelocity, inputManager.Movement * targetSpeed, animationBlendSpeed * Time.fixedDeltaTime);

        var velocity = rigidbody.velocity;
        var xVelocityDifference = currentVelocity.x - velocity.x;
        var yVelocityDifference = currentVelocity.y - velocity.y;   
    
        rigidbody.AddForce(transform.TransformVector(new Vector3(xVelocityDifference, 0, yVelocityDifference)), ForceMode.VelocityChange);

        animator.SetFloat(xVelocityHash, currentVelocity.x);
        animator.SetFloat(yVelocityHash, currentVelocity.y);
    }

    private void Crouch() => animator.SetBool(crouchHash, inputManager.Crouch);

    private void CameraMovement()
    {
        if (!hasAnimator) return;
        var mouseX = inputManager.Look.x;
        var mouseY = inputManager.Look.y;
        сamera.position = сameraRoot.position;

        xRotation -= mouseY * sensitivity * Time.smoothDeltaTime;
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);
        
        сamera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(0, mouseX * sensitivity * Time.smoothDeltaTime, 0));
    }
}
