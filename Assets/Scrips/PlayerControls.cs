using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public bool isJumping;
    public bool isRunning;
    public float disapearTime = 1;

    [Header("Movements")]
    public float walkSpeed = 5;
    public float runSpeed = 10;
    public float jumpForce = 10;

    Vector2 MovementInput = Vector2.zero;
    Vector3 moveDirection = Vector3.zero;

    public readonly int moveXHash = Animator.StringToHash("MovementX");
    public readonly int moveYHash = Animator.StringToHash("MovementY");
    public readonly int jumpHash = Animator.StringToHash("IsJumping");
    public readonly int runHash = Animator.StringToHash("IsRunning");

    Rigidbody rb;
    Animator anim;
    public GameObject Tim;

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
            moveDirection = Vector3.zero;

            //anim.SetFloat(moveHash, 0.0f);
        }
        else
        {
            moveDirection = transform.forward * MovementInput.y + transform.right * MovementInput.x;
            float currentSpeed = isRunning ? runSpeed : walkSpeed;

            Vector3 movementDirection = moveDirection * (currentSpeed * Time.deltaTime);
            transform.position += movementDirection;
            

        }

        
    }

    public void OnMove(InputValue val)
    {
        MovementInput = val.Get<Vector2>();

        anim.SetFloat(moveXHash, MovementInput.x);
        anim.SetFloat(moveYHash, MovementInput.y);
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
        anim.SetBool(runHash, isRunning);
    }

    private void OnCollisionEnter(Collision other)
    {
        isJumping = false;
        anim.SetBool(jumpHash, false);

        if (other.gameObject.CompareTag("Platform"))
        {
            Destroy(other.gameObject, disapearTime);
        }
    }
}
