using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PhysicsMaterial2D BounMaterial2D, NormMaterial2D, NoPlayerBounce2D;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpingPower = 0.0f;

    private const float GroundCheckRadius = 0.2f;
    private const float JumpPowerIncrement = 0.05f;

    private float horizontal;
    private float speed = 6.75f;
    private bool isFacingRight = true;

    // TODO: Add a jump sound effect
    // TODO: Split in two Documents, like PlayerMovement and PlayerController = Is on Ground, can Jump etc.
    // TODO: No bouncing when landing on the ground
    // Update is called once per frame
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        animator.SetFloat("Speed", Mathf.Abs(horizontal));

        if (jumpingPower == 0.0f && IsGrounded())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (!IsGrounded())
        {
            rb.sharedMaterial = BounMaterial2D;
            animator.SetBool("IsJumping", true);
        }
        else
        {
            rb.sharedMaterial = NormMaterial2D;
            animator.SetBool("IsJumping", false);
        }

        if (Input.GetKey("space") && IsGrounded())
        {
            jumpingPower += JumpPowerIncrement;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
            animator.SetBool("IsKeyDownSpace", true);
        }
        else if (Input.GetKeyUp("space") && IsGrounded())
        {
            rb.velocity = new Vector2(horizontal * speed, jumpingPower);
            StartCoroutine(ResetJump());
            animator.SetBool("IsKeyDownSpace", false);
        }

        if (jumpingPower >= 25f && IsGrounded())
        {
            animator.SetBool("IsKeyDownSpace", false);
            float tempy = jumpingPower;
            rb.velocity = new Vector2(horizontal * speed, tempy);
            StartCoroutine(ResetJump());
        }

        if (Input.GetKeyUp("space") || !IsGrounded())
        {
            BounMaterial2D.bounciness = 0.5f;
            animator.SetBool("IsKeyDownSpace", false);

            if (IsGrounded())
            {
                BounMaterial2D.bounciness = 0.0f;
                rb.velocity = new Vector2(horizontal * speed, jumpingPower);
                jumpingPower = 0.0f;
            }
        }

        if (horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (horizontal < 0 && isFacingRight)
        {
            Flip();
        }
    }

    private IEnumerator ResetJump()
    {
        yield return new WaitForSeconds(0.2f);
        jumpingPower = 0.0f;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, groundLayer);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}