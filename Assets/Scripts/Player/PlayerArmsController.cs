using UnityEngine;

public class PlayerArmsController : MonoBehaviour
{
    private Rigidbody rigidbody;
    private InputManager inputManager;
    [SerializeField] private Transform сameraRoot;
    [SerializeField] private Transform сamera;
    private float xRotation;
    
    private const float walkSpeed = 2f;
    private const float runSpeed = 6f;
    [SerializeField] private float upperLimit = -90f;
    [SerializeField] private float bottomLimit = 90f;
    [SerializeField] private float sensitivity = 100f;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
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
        var targetSpeed = inputManager.Run ? runSpeed : walkSpeed;
        if (inputManager.Movement == Vector2.zero) targetSpeed = 0;

        //currentVelocity = Vector2.Lerp(currentVelocity, inputManager.Movement * targetSpeed, Time.fixedDeltaTime);
        var currentVelocity = new Vector3(inputManager.Movement.x, 0, inputManager.Movement.y);
        currentVelocity *= targetSpeed;
        currentVelocity = transform.TransformDirection(currentVelocity);

        var velocity = rigidbody.velocity;
        var velocityChange = new Vector3(currentVelocity.x - velocity.x, 0, currentVelocity.z - velocity.z);

        rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
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
