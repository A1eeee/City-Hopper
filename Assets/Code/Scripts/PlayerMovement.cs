using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PhysicsMaterial2D BounMaterial2D, NormMaterial2D;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpingPower = 0.0f;

    private float horizontal;
    private float speed = 6.75f;
    private bool isFacingRight = true;

    // TODO: Add a jump sound effect
    // TODO: No bouncing when landing on the ground
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (jumpingPower == 0.0f && IsGrounded())
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }

        if (!IsGrounded())
        {
            rb.sharedMaterial = BounMaterial2D;
        }
        else
        {
            rb.sharedMaterial = NormMaterial2D;
        }

        if (Input.GetKey("space") && IsGrounded())
        {
            jumpingPower += 0.05f;
        }

        if (Input.GetKeyDown("space") && IsGrounded())
        {
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }

        if (jumpingPower >= 24f && IsGrounded())
        {
            float tempy = jumpingPower;
            rb.velocity = new Vector2(horizontal * speed, tempy);
            Invoke("ResetJump", 0.2f);
        }

        if (Input.GetKeyUp("space"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(horizontal * speed, jumpingPower);
                jumpingPower = 0.0f;
            }
        }

        Flip();
    }

    void ResetJump()
    {
        jumpingPower = 0.0f;
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0 || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
