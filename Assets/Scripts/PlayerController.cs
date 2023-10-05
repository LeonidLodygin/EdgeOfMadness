using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private InputManager inputManager;
    private Animator animator;
    private bool hasAnimator;
    private int xVelocityHash;
    private int yVelocityHash;
    [SerializeField] private Transform CameraRoot;
    [SerializeField] private Transform Camera;
    private Vector2 currentVelocity;
    private float xRotation;

    private const float walkSpeed = 2f;
    private const float runSpeed = 6f;
    [SerializeField] private float AnimationBlendSpeed = 8.9f;
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
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        CameraMovement();
    }

    private void Move()
    {
        if (!hasAnimator) return;
        float targetSpeed = inputManager.Run ? runSpeed : walkSpeed;
        if (inputManager.Movement == Vector2.zero) targetSpeed = 0.1f;

        currentVelocity.x = Mathf.Lerp(currentVelocity.x, inputManager.Movement.x * targetSpeed, AnimationBlendSpeed * Time.fixedDeltaTime);
        currentVelocity.y = Mathf.Lerp(currentVelocity.y, inputManager.Movement.y * targetSpeed, AnimationBlendSpeed * Time.fixedDeltaTime);

        var velocity = rigidbody.velocity;
        var xVelocityDifference = currentVelocity.x - velocity.x;
        var yVelocityDifference = currentVelocity.y - velocity.y;
        
        rigidbody.AddForce(transform.TransformVector(new Vector3(xVelocityDifference, 0, yVelocityDifference)), ForceMode.VelocityChange);
        
        animator.SetFloat(xVelocityHash, currentVelocity.x);
        animator.SetFloat(yVelocityHash, currentVelocity.y);
    }

    private void CameraMovement()
    {
        if (!hasAnimator) return;
        var mouseX = inputManager.Look.x;
        var mouseY = inputManager.Look.y;
        Camera.position = CameraRoot.position;

        xRotation -= mouseY * sensitivity * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, upperLimit, bottomLimit);
        
        Camera.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up, mouseX * sensitivity * Time.deltaTime);
    }
}
