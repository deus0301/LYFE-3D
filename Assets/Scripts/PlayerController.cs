using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //movement variables
    private Vector2 input;
    private CharacterController controller;
    private Vector3 dir;
    [SerializeField] private float speed = 5f;

    //rotation variables
    [SerializeField] private Transform cameraTransform;
    private float currentVelocity;

    // Gravity and jumping
    private float gravity = -9.81f;
    [SerializeField] private float gravMultiplier = 3.0f;
    [SerializeField] private float jumpPower;
    private float velocity;

    //Shrinking mechanic
    private bool isMoving;
    public bool shrink;
    private Vector3 idle = new(0.1f, 0.1f, 0.1f);
    [SerializeField] private float shrinkSpeed = -0.1f;

    void Awake()
    {
        //Assigning player character controller to controller object
        controller = GetComponent<CharacterController>();
        cameraTransform = GameObject.Find("Camera pivot").transform;
    }

    void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
        if (shrink && isMoving && transform.localScale.y > 0.01f)
        {
            Vector3 shrinkSize = new(shrinkSpeed, shrinkSpeed, shrinkSpeed);
            transform.localScale -= shrinkSize * Time.deltaTime;
        }
    }

    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            speed = 10f;
        }
        else if (context.canceled)
        {
            speed = 5f;
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!isGrounded()) return;

        velocity += jumpPower;
    }
    private void ApplyGravity()
    {
        if (isGrounded() && velocity < 0.0f)
        {
            velocity = -1.0f;
        }
        else
        {
            velocity += gravity * gravMultiplier * Time.deltaTime;
        }
        dir.y = velocity;
    }
    private void ApplyRotation()
    {
        //Preventing the player from automatically snapping to the forward direction while moving
        if (input.sqrMagnitude == 0) return;

        //Setting player rotation
        float target = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, target, ref currentVelocity, 0.05f);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }
    private void ApplyMovement()
    {
        //Moving the Player
        controller.Move(speed * Time.deltaTime * dir);
    }
    public void Move(InputAction.CallbackContext context)
    {
        //Taking player input and accordinly setting the direction in which player needs to move
        input = context.ReadValue<Vector2>();
        Vector3 inputDir = new Vector3(input.x, 0.0f, input.y);

    if (inputDir.magnitude > 0.1f)
    {
        isMoving = true;

        // Convert to camera-relative movement
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        dir = camForward * inputDir.z + camRight * inputDir.x;
    }
    else
    {
        isMoving = false;
        dir = Vector3.zero;
    }

    }

    private bool isGrounded() => controller.isGrounded;

}