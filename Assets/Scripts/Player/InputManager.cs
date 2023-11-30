using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;


public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    
    public Vector2 Movement { get; private set; }
    public Vector2 Look { get; private set; }
    public bool Run { get; private set; }
    public bool Jump { get; private set; }

    public bool Aim { get; private set; }
    //public bool Crouch { get; private set; }
    
    public bool Rage { get; private set; }
    public bool Pulling { get; private set; }
    public bool Blink { get; private set; }

    private InputActionMap currentMap;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction runAction;
    private InputAction jumpAction;
    private InputAction aimAction;
    //private InputAction crouchAction;
    
    private InputAction rageAction;
    private InputAction pullAction;
    private InputAction blinkAction;
    
    public WeaponController currentWeapon;

    private void Awake()
    {
        HideCursor();
        currentMap = playerInput.currentActionMap;
        moveAction = currentMap.FindAction("Movement");
        lookAction = currentMap.FindAction("Look");
        runAction = currentMap.FindAction("Run");
        jumpAction = currentMap.FindAction("Jump");
        aimAction = currentMap.FindAction("Aim");
        rageAction = currentMap.FindAction("RageAbility");
        pullAction = currentMap.FindAction("PullAbility");
        blinkAction = currentMap.FindAction("BlinkAbility");
        //crouchAction = currentMap.FindAction("Crouch");

        moveAction.performed += OnMove;
        lookAction.performed += OnLook;
        runAction.performed += OnRun;
        jumpAction.performed += OnJump;
        aimAction.performed += OnAim;
        rageAction.performed += OnRage;
        pullAction.performed += OnPulling;
        blinkAction.performed += OnBlink;
        //crouchAction.performed += OnCrouch;

        moveAction.canceled += OnMove;
        lookAction.canceled += OnLook;
        runAction.canceled += OnRun;
        jumpAction.canceled += OnJump;
        aimAction.canceled += OnAim;
        rageAction.canceled += OnRage;
        pullAction.canceled += OnPulling;
        blinkAction.canceled += OnBlink;
        //crouchAction.canceled += OnCrouch;

        if (currentWeapon) { currentWeapon.Initialise(this);}
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
    private void OnJump(InputAction.CallbackContext context)
    {
        Jump = context.ReadValueAsButton();
    }
    private void OnAim(InputAction.CallbackContext context)
    {
        Aim = context.ReadValueAsButton();
    }
    private void OnRage(InputAction.CallbackContext context)
    {
        Rage = context.ReadValueAsButton();
    }
    private void OnPulling(InputAction.CallbackContext context)
    {
        Pulling = context.ReadValueAsButton();
    }
    private void OnBlink(InputAction.CallbackContext context)
    {
        Blink = context.ReadValueAsButton();
    }
    
    //private void OnCrouch(InputAction.CallbackContext context)
    //{
    //    Crouch = context.ReadValueAsButton();
    //}

    private void OnEnable()
    {
        currentMap.Enable();
    }
    private void OnDisable()
    {
        currentMap.Disable();
    }
    private static void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
