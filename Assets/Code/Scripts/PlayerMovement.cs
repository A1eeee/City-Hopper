using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PhysicsMaterial2D BounMaterial2D, NormMaterial2D;
    public Animator animator;

    public TextMeshProUGUI timerText;

    public float elepsedTime;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpingPower = 0.0f;

    private const float GroundCheckRadius = 0.2f;
    private const float JumpPowerIncrement = 0.05f;

    private float horizontal;
    private float speed = 6.75f;
    private bool isFacingRight = true;

    // Update is called once per frame
    private void Update()
    {
        elepsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(elepsedTime / 60);
        int seconds = Mathf.FloorToInt(elepsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

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

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer()
    {
        PlayerData data = SaveSystem.LoadPlayer();

        elepsedTime = data.elepsedTime;

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];
        transform.position = position;
    }
}