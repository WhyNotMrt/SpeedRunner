using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public LayerMask platFormLayer;

    private Rigidbody2D rb;

    private float playerSpeed = 800f;
    private float horizontal;
    private Vector2 velocity = Vector2.zero;
    private float smouthTime = .05f;

    private float jumpHeight = 1000.0f;
    private bool jump = false;

    private float wallJumpHeight = 1200.0f;
    private float wallJumpForce = 400.0f;
    private float maxJumpHeight = 20.0f;
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
    }

    // Update is called once per frame
    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded()) jump = true;
        }

        if (!IsGrounded())
        {
            if (IsOnWall())
            {
                canJump = true;
            } 
            else
            {
                Invoke(nameof(SetCanJumpToFalse), timeUntilCannotJump);
            }
        } 
        else
        {
            canJump = false;
            wallSide = 0;
            endurenceOnWall = 0f;
            timeElapsedOnWall = 0f;
        }

        if (Input.GetButtonDown("Jump") && canJump)
        {
            jumpWall = true;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - gameObject.transform.localScale.y / 2 - 0.1f), new Vector2(gameObject.transform.localScale.x - 0.05f, 0.1f));
        Gizmos.color = Color.blue;
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x - gameObject.transform.localScale.x / 2 - 0.05f, gameObject.transform.position.y), new Vector2(0.05f, gameObject.transform.localScale.y - 0.2f));
        Gizmos.DrawCube(new Vector2(gameObject.transform.position.x + gameObject.transform.localScale.x / 2 + 0.05f, gameObject.transform.position.y), new Vector2(0.05f, gameObject.transform.localScale.y - 0.2f));
    }

    private void MovePlayer()
    {
        if (!Mathf.Approximately(horizontal, 0f) && (wallSide == 0 || wallSide == 1 && horizontal > 0f || wallSide == -1 && horizontal < 0f || wallSide != 0 && !Mathf.Approximately(endurenceOnWall, 0f)))
        {
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

    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - gameObject.transform.localScale.y / 2 - 0.1f), new Vector2(gameObject.transform.localScale.x - 0.05f, 0.1f), 0f, platFormLayer);
    }

    public bool IsOnWall()
    {
        bool colliderLeft = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x - gameObject.transform.localScale.x / 2 - 0.05f, gameObject.transform.position.y), new Vector2(0.05f, gameObject.transform.localScale.y - 0.2f), 0f, platFormLayer);
        bool colliderRight = Physics2D.OverlapBox(new Vector2(gameObject.transform.position.x + gameObject.transform.localScale.x / 2 + 0.05f, gameObject.transform.position.y), new Vector2(0.05f, gameObject.transform.localScale.y - 0.2f), 0f, platFormLayer);

        if (colliderLeft) wallSide = 1;
        if (colliderRight) wallSide = -1;

        if (colliderLeft || colliderRight) 
        {
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

    public void SetCanJumpToFalse()
    {
        canJump = false;
        wallSide = 0;
    }
}
