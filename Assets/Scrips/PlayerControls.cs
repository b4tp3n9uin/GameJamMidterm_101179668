using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public bool isJumping;
    public bool isRunning;
    public bool isInSafeZone;
    public float disapearTime = 1.3f;

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
    public GameObject BackCam;
    public InGameManager inGameManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        BackCam.SetActive(false);
        inGameManager = FindObjectOfType<InGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(MovementInput.magnitude > 0))
        {
            moveDirection = Vector3.zero;
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
        if (!BackCam.active)
        {
            // Move only when back camera is deactivated.
            MovementInput = val.Get<Vector2>();

            anim.SetFloat(moveXHash, MovementInput.x);
            anim.SetFloat(moveYHash, MovementInput.y);
        }
    }

    public void OnJump(InputValue val)
    {
        if (isJumping) return;

        if (isRunning) isRunning = false;

        if (!BackCam.active)
        {
            // Jump, only if back camera is deactivated
            FindObjectOfType<AudioManager>().Play("jump");
            anim.SetBool(jumpHash, true);
            isJumping = val.isPressed;
            rb.AddForce((transform.up + moveDirection) * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnRun(InputValue val)
    {
        if (!isJumping)
        {
            isRunning = val.isPressed;
        }
        anim.SetBool(runHash, isRunning);
    }

    public void OnLookback(InputValue val)
    {
        if (isInSafeZone)
        {
            // look back to see how much damage you caused! lol
            BackCam.SetActive(val.isPressed);
        }
    }

    public void OnPause(InputValue val)
    {
        inGameManager.PausePressed();
    }

    private void OnCollisionEnter(Collision other)
    {
        isJumping = false;
        anim.SetBool(jumpHash, false);

        if (other.gameObject.CompareTag("DeathZone"))
        {
            inGameManager.GameOver();
        }

        if (other.gameObject.CompareTag("Win"))
        {
            inGameManager.WinGame();
        }

        if (other.gameObject.CompareTag("Platform"))
        {
            //destroy platform after a second has passed.
            FindObjectOfType<AudioManager>().Play("platform");
            Destroy(other.gameObject, disapearTime);
        }

        int scoreAdd;

        if (other.gameObject.CompareTag("safezone"))
        {
            isInSafeZone = true;
        }
        else
        {
            isInSafeZone = false;
        }
        
        if (other.gameObject.layer == 6)
        {
            //Level 1
            scoreAdd = 5;
            inGameManager.IncrementScore(scoreAdd);
        }

        if (other.gameObject.layer == 7)
        {
            //Level 2
            scoreAdd = 10;
            inGameManager.IncrementScore(scoreAdd);
        }

        if (other.gameObject.layer == 8)
        {
            // SafeZone
            FindObjectOfType<AudioManager>().Play("safe");
        }

        if (other.gameObject.layer == 9)
        {
            // Level 3
            scoreAdd = 15;
            inGameManager.IncrementScore(scoreAdd);
        }

    }
}
