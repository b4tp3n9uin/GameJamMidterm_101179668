using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public bool isMoving;
    public bool isJumping;
    public bool isRunning;

    [Header("Movements")]
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float jumpForce = 10;

    Vector2 MovementInput = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;

    public readonly int moveHash = Animator.StringToHash("Movement");
    public readonly int jumpHash = Animator.StringToHash("IsJumping");

    Rigidbody rb;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(MovementInput.magnitude > 0))
        {
            isMoving = false;
            moveDirection = Vector3.zero;

            anim.SetFloat(moveHash, 0.0f);
        }
        else
        {
            moveDirection = transform.forward * MovementInput.y + transform.right * MovementInput.x;
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);

            transform.position += movementDirection;

            float moveAnim = currentSpeed / runSpeed;
            anim.SetFloat(moveHash, moveAnim);
        }
        
        
        

        //transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);

    }

    public void OnMove(InputValue val)
    {
        MovementInput = val.Get<Vector2>();

        isMoving = true;
    }

    public void OnJump(InputValue val)
    {
        anim.SetBool(jumpHash, true);
        if (isJumping) return;

        if (isRunning) isRunning = false;
        
        isJumping = val.isPressed;
        rb.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        
    }

    public void OnRun(InputValue val)
    {
        if (!isJumping)
        {
            isRunning = val.isPressed;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        isJumping = false;
        anim.SetBool(jumpHash, false);

        if (other.gameObject.CompareTag("Platform"))
        {
            Destroy(other.gameObject, 2);
        }
    }
}
