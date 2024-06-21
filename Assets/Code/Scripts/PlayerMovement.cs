using System.Collections;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public PhysicsMaterial2D BounMaterial2D, NormMaterial2D;
    public Animator animator;
    public GameObject footstep;
    public GameObject jumpSound;
    public GameObject collisionSound;

    public TextMeshProUGUI timerText;

    public float elepsedTime;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpingPower = 0.0f;

    private const float GroundCheckRadius = 0.2f;
    private const float JumpPowerIncrement = 0.3f;
    private const float MaxJumpPower = 25f; // Maximum jumping power

    private float horizontal;
    private float speed = 6.75f;
    private bool isFacingRight = true;
    private bool isFootstepActive = false;
    private bool isJumping = false;

    void Start()
    {
        footstep.SetActive(false);
        jumpSound.SetActive(false);
        collisionSound.SetActive(false);
    }

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
            StopFootsteps();
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
            isJumping = true;
        }
        else if (Input.GetKeyUp("space") && IsGrounded())
        {
            rb.velocity = new Vector2(horizontal * speed, jumpingPower);
            StartCoroutine(ResetJump());
            animator.SetBool("IsKeyDownSpace", false);
            PlayJumpSound();
        }

        if (jumpingPower >= MaxJumpPower && IsGrounded())
        {
            animator.SetBool("IsKeyDownSpace", false);
            rb.velocity = new Vector2(horizontal * speed, jumpingPower);
            StartCoroutine(ResetJump());
            PlayJumpSound();
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
                if (isJumping)
                {
                    PlayJumpSound();
                    isJumping = false;
                }
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

        HandleFootsteps();
    }

    private void HandleFootsteps()
    {
        if (IsGrounded() && !Input.GetKey("space"))
        {
            if ((Input.GetKeyDown("a") || Input.GetKeyDown("d") || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) && !isFootstepActive)
            {
                footsteps();
            }
            if ((Input.GetKeyUp("a") || Input.GetKeyUp("d") || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)) && isFootstepActive)
            {
                StopFootsteps();
            }
        }
        else
        {
            StopFootsteps();
        }
    }

    private void footsteps()
    {
        footstep.SetActive(true);
        isFootstepActive = true;
    }

    private void StopFootsteps()
    {
        footstep.SetActive(false);
        isFootstepActive = false;
    }

    private void PlayJumpSound()
    {
        jumpSound.SetActive(true);
        StartCoroutine(StopJumpSound());
    }

    private IEnumerator StopJumpSound()
    {
        yield return new WaitForSeconds(1f); // Adjust the duration as needed
        jumpSound.SetActive(false);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayCollisionSound();
    }

    private void PlayCollisionSound()
    {
        collisionSound.SetActive(true);
        StartCoroutine(StopCollisionSound());
    }

    private IEnumerator StopCollisionSound()
    {
        yield return new WaitForSeconds(1f); // Adjust the duration as needed
        collisionSound.SetActive(false);
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
