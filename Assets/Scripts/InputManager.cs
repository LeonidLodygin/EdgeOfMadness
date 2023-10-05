using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    
    public Vector2 Movement { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Run { get; private set; }
    public bool Crouch { get; private set; }

    private InputActionMap currentMap;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction crouchAction;

    private void Awake()
    {
        HideCursor();
        currentMap = playerInput.currentActionMap;
        moveAction = currentMap.FindAction("Movement");
        lookAction = currentMap.FindAction("Look");
        runAction = currentMap.FindAction("Run");
        crouchAction = currentMap.FindAction("Crouch");

        moveAction.performed += OnMove;
        lookAction.performed += OnLook;
        runAction.performed += OnRun;
        crouchAction.performed += OnCrouch;

        moveAction.canceled += OnMove;
        lookAction.canceled += OnLook;
        runAction.canceled += OnRun;
        crouchAction.canceled += OnCrouch;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Movement = context.ReadValue<Vector2>();
    }
    private void OnLook(InputAction.CallbackContext context)
    {
        Look = context.ReadValue<Vector2>();
    }
    private void OnRun(InputAction.CallbackContext context)
    {
        Run = context.ReadValueAsButton();
    }
    private void OnCrouch(InputAction.CallbackContext context)
    {
        Crouch = context.ReadValueAsButton();
    }

    private void OnEnable()
    {
        currentMap.Enable();
    }
    private void OnDisable()
    {
        currentMap.Disable();
    }
    private void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
