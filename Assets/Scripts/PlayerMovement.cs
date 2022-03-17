using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Object References")]
    public Rigidbody2D rb;

    [Header("Horizontal Movement")]
    public float movementSpeed;

    [Header("Vertical Movement")]
    public float jumpForce = 5f;

    [Header("Grounded")]
    public Transform playerFeet;
    public LayerMask groundLayer;
    public float groundRadius = 0.2f;

    [Header("Wall Jump")]
    public float wallJumpTime = 0.2f;
    public float wallSlideSpeed = 0.3f;
    public float wallDistance = 0.5f;
    bool isWallSliding = false;
    RaycastHit2D WallCheck;
    float jumpTime;

    public Animator PlayerAnimator;


    bool isFacingRight = true;
    float mx;

    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

        if (Input.GetButtonDown("Jump") && isGrounded() || isWallSliding && Input.GetButtonDown("Jump"))
        {
            PlayerAnimator.SetBool("Jumping", true);
            Jump();
        }
    }

    private void FixedUpdate()
    {
        mx = Input.GetAxis("Horizontal");

        if (isGrounded())
        {
            PlayerAnimator.SetBool("Jumping", false);
        }
        if (mx < 0)
        {
            isFacingRight = false;
            transform.localRotation = Quaternion.Euler( 0, 180, 0);
        }
        else
        {
            isFacingRight = true;
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        PlayerAnimator.SetFloat("Speed", Mathf.Abs(mx));
        rb.velocity = new Vector2(mx * movementSpeed, rb.velocity.y);

        // Wall Jump
        if (isFacingRight)
        {
            WallCheck = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.green);
        }
        else
        {
            WallCheck = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, groundLayer);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.green);
        }

        if (WallCheck && !isGrounded() && mx != 0)
        {
            PlayerAnimator.SetBool("onWall", true);
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
            PlayerAnimator.SetBool("onWall", false);
        }

        if (isWallSliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, wallSlideSpeed, float.MaxValue));
        }

    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public bool isGrounded()
    {
        Collider2D groundCheck = Physics2D.OverlapCircle(playerFeet.position, 0.2f, groundLayer);
        
        if (groundCheck != null)
        {
            return true;
        }
        return false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Trap")
        {
            transform.position = new Vector2(-41.26f, -3.18f);
        }

        if (collision.gameObject.tag == "Finish point")
        {
            transform.position = new Vector2(-92.33f, 2.76f);
        }
    }
}