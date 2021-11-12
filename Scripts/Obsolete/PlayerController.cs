using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour/*, GameControls.IPlayerActions*/
{
    public GameObject blindScreen;
    public GameManager gameManager;

    [Header("Насторйка скорости передвижения персонажа")]
    public float walkSpeed;
    public float runSpeed;
    public float speedSmoothTime;

    
    float speedSmootVelocity;
    float currentSpeed;

    [Header("Насторйка поворота персонажа")]
    [SerializeField] private float turnSmoothTime;
    float turnSmoothVelocity;

    [Header("Насторйка гравитации")]
    public float jumpHeight;
    public float gravity;
    [Range(0, 1)] public float airControlPercent;
    float velocityY;

    private Animator animator;
    private Transform mainCamera;
    private CharacterController controller;
    Keyboard keyboard = Keyboard.current;


    private GameControls playerControls;
    Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }
    /*
    public void OnMovement(InputAction.CallbackContext context)
    {
        Vector2 inputDirection = context.ReadValue<Vector2>();
        Vector2 input = new Vector2(inputDirection.x, inputDirection.y);
        Direction = input.normalized;
    }*/

    void Awake()
    {
        playerControls = new GameControls();
        animator = GetComponent<Animator>();
        mainCamera = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        //playerControls.Player.SetCallbacks(this);        
    }

    void Update()
    {
        Inputs();

        bool running = keyboard.leftShiftKey.isPressed;
        Move(running);

        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("SpeedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    private void OnEnable()
    {        
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Inputs()
    {
        if (keyboard.eKey.isPressed)
        {
            gameManager.CurrentLayer += 1;
        }

        if (keyboard.qKey.isPressed)
        {
            gameManager.CurrentLayer -= 1;
        }

        if (keyboard.bKey.isPressed)
        {
            BlindScreenToggle();
        }

        if (keyboard.spaceKey.isPressed)
        {
            Jump();
        }
    }

   

    private void BlindScreenToggle()
    {     
        if (blindScreen.activeInHierarchy)
        {
            blindScreen.SetActive(false);
        }
        else
        {
            blindScreen.SetActive(true);
        }        
    }

    private void Move(bool running)
    {
       if (Direction != Vector2.zero)
       {
            float targetRotation = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
       }
       
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * direction.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmootVelocity, GetModifiedSmoothTime(speedSmoothTime));     
        
        velocityY += Time.deltaTime * gravity;

        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }
        
    }    

    private void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }
        if (airControlPercent ==0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

}

