using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;
    
    public Vector2 Movement { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Run { get; private set; }

    private InputActionMap currentMap;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;

    private void Awake()
    {
        HideCursor();
        currentMap = PlayerInput.currentActionMap;
        moveAction = currentMap.FindAction("Movement");
        lookAction = currentMap.FindAction("Look");
        runAction = currentMap.FindAction("Run");

        moveAction.performed += OnMove;
        lookAction.performed += OnLook;
        runAction.performed += OnRun;
        
        moveAction.canceled += OnMove;
        lookAction.canceled += OnLook;
        runAction.canceled += OnRun;
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
