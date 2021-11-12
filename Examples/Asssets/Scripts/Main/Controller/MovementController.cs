using UnityEngine;

public class MovementController : MonoBehaviour
{
    private Animator animator;
    private CharacterController controller;

    [Header("Насторйка скорости передвижения персонажа")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float speedSmoothTime;

    [Header("Насторйка гравитации")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float gravity;
    [Range(0, 1)][SerializeField] private float airControlPercent;

    private bool running = false;
    private bool jump;

    private float currentSpeed;
    private float speedSmootVelocity;
    private float velocityY;

    public bool Running
    {
        set
        {
            running = value;
        }
    }

    private Vector2 direction;
    public Vector2 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value.normalized;
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        if (jump)
        {                        
            Jump();            
        }
        Move();

        if (animator != null)
        {
            float animationSpeedPercent = (running ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
            animator.SetFloat("SpeedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        }
        
    }        

    private void Move()
    {       
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * direction.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmootVelocity, GetModifiedSmoothTime(speedSmoothTime));
        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = (transform.right * direction.x + transform.forward * direction.y) * currentSpeed + Vector3.up * velocityY;

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
            jump = false;
        }
    }

    private float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }
        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    public void OnJumpPressed()
    {
        jump = true;
    }

}

