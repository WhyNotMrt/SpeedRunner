using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public LayerMask platFormLayer;

    private Rigidbody2D rb;
    private BoxCollider2D bc;
    private Animator animator;

    private int playerWalkSpeed = 500;
    private int playerSprintSpeed = 800;
    private float horizontal;
    private Vector2 velocity = Vector2.zero;
    private float smouthTime = .1f;

    private int jumpHeight = 1000;
    private bool jump = false;

    private int wallJumpHeight = 800;
    private int wallJumpForce = 400;
    private int maxJumpHeight = 20;
    private bool jumpWall = false;
    private int wallSide = 0;
    private bool canJump = false;
    private float endurenceOnWall = 0;
    private float stayOnWallMaxTime = 0.5f;
    private float timeElapsedOnWall = 0;
    private float timeUntilCannotJump = 0.05f;

    // Start is called before the first frame update
    private void Start()
    {
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            animator.SetBool("IsGrounded", true);
            animator.SetBool("OnWall", false);
            canJump = false;
            wallSide = 0;
            endurenceOnWall = 0f;
            timeElapsedOnWall = 0f;

            if (Input.GetButtonDown("Jump"))
            {
                jump = true;
            }
        } 
        else
        {
            animator.SetBool("IsGrounded", false);

            if (IsOnWall())
            {
                animator.SetBool("OnWall", true);
                canJump = true;
            }
            else
            {
                Invoke(nameof(SetCanJumpToFalse), timeUntilCannotJump);
            }
        }

        if (Input.GetButtonDown("Jump") && canJump)
        {
            jumpWall = true;
        }

        animator.SetBool("IsMoving", rb.velocity.x > 0.01f || rb.velocity.x < -0.01f);
        animator.SetFloat("Direction", Mathf.Clamp(rb.velocity.x, -1f, 1f));
        animator.SetFloat("WallSide", wallSide);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnDrawGizmosSelected()
    {
        if (bc == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(bc.transform.position.x, bc.transform.position.y - bc.size.y / 2 - 0.1f), new Vector2(bc.size.x - 0.05f, 0.1f));
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(bc.transform.position.x - bc.size.x / 2 - 0.1f, bc.transform.position.y), new Vector2(0.1f, bc.size.y - 0.2f));
        Gizmos.DrawCube(new Vector2(bc.transform.position.x + bc.size.x / 2 + 0.1f, bc.transform.position.y), new Vector2(0.1f, bc.size.y - 0.2f));
    }

    private void MovePlayer()
    {
        if (!Mathf.Approximately(horizontal, 0f) && (wallSide == 0 || wallSide == 1 && horizontal > 0f || wallSide == -1 && horizontal < 0f || wallSide != 0 && !Mathf.Approximately(endurenceOnWall, 0f)))
        {
            int playerSpeed = Input.GetButton("Sprint") ? playerSprintSpeed : playerWalkSpeed;
            float horizontalVelocity = Mathf.Approximately(endurenceOnWall, 0f) ? horizontal * playerSpeed * Time.fixedDeltaTime : horizontal * playerSpeed * Time.fixedDeltaTime * endurenceOnWall;

            Vector2 newVelocity = new Vector2(horizontalVelocity, rb.velocity.y);

            rb.velocity = Vector2.SmoothDamp(rb.velocity, newVelocity, ref velocity, smouthTime);
        }

        if (jump)
        {
            rb.AddForce(new Vector2(0f, jumpHeight));
            jump = false;
        }

        if (jumpWall)
        {
            float horizontalJumpForce = wallSide * wallJumpForce;

            rb.AddForce(new Vector2(horizontalJumpForce, wallJumpHeight), ForceMode2D.Impulse);
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxJumpHeight);

            canJump = false;
            jumpWall = false;
            wallSide = 0;
            endurenceOnWall = 0f;
            timeElapsedOnWall = 0f;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(bc.transform.position.x, bc.transform.position.y - bc.size.y / 2 - 0.1f), new Vector2(bc.size.x - 0.05f, 0.1f), 0f, platFormLayer);
    }

    private bool IsOnWall()
    {
        bool colliderLeft = Physics2D.OverlapBox(new Vector2(bc.transform.position.x - bc.size.x / 2 - 0.1f, bc.transform.position.y), new Vector2(0.1f, bc.size.y - 0.2f), 0f, platFormLayer);
        bool colliderRight = Physics2D.OverlapBox(new Vector2(bc.transform.position.x + bc.size.x / 2 + 0.1f, bc.transform.position.y), new Vector2(0.1f, bc.size.y - 0.2f), 0f, platFormLayer);

        if (colliderLeft) wallSide = 1;
        if (colliderRight) wallSide = -1;

        if (colliderLeft || colliderRight) 
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            if (timeElapsedOnWall < stayOnWallMaxTime)
            {
                endurenceOnWall = Mathf.Lerp(stayOnWallMaxTime, 0.1f, timeElapsedOnWall / 1);
                timeElapsedOnWall += Time.deltaTime;
            }
            else
            {
                endurenceOnWall = 0.1f;
            }
            return true;
        }
        return false;
    }

    private void SetCanJumpToFalse()
    {
        animator.SetBool("OnWall", false);
        canJump = false;
        wallSide = 0;
    }
}
