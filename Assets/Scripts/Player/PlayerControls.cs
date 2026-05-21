using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerControls : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Animator anim;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float rotateSpeed = 4f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float gravityValue = -20f;

    // Movement State
    private Vector3 playerVelocity;
    [SerializeField] private bool isGrounded;
    private bool isMoving;
    private bool isJumping;
    private bool canControl = true;

    [Header("Knock Back Settings")]
    [SerializeField] private float knockBackStrength = 12f;
    [SerializeField] private float knockBackHeight = 0.5f;
    [SerializeField] private float knockBackDuration = 0.25f;
    [SerializeField] private float knockBackDecay = 10f;

    [Header("Input")]
    [SerializeField] private float moveHorizontal;
    [SerializeField] private float moveVertical;

    private Vector3 knockBackVelocity;
    private float knockBackTimer;
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
            isJumping = false;
        }

        float horizontal = 0f;
        float vertical = 0f;

        // knockback
        if (knockBackTimer > 0)
        {
            controller.Move(knockBackVelocity * Time.deltaTime);
            knockBackVelocity = Vector3.Lerp(knockBackVelocity, Vector3.zero, knockBackDecay * Time.deltaTime);
            knockBackTimer -= Time.deltaTime;
            return; // stop player input during knockback
        }
        else
        {
            if (canControl)
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");

                // Rotation
                transform.Rotate(0, horizontal * rotateSpeed, 0);

                // Jump
                if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
                {
                    isJumping = true;
                    AudioManager.Instance.PlayJump();
                    anim.SetTrigger("Jump");
                    playerVelocity.y = jumpForce;
                }
            }
        }

        // Horizontal movement (ALWAYS calculated)
        Vector3 move = transform.forward * vertical * moveSpeed;

        // Gravity (ALWAYS applied)
        playerVelocity.y += gravityValue * Time.deltaTime;

        // Final move (ALWAYS applied)
        Vector3 finalMove = move + playerVelocity;
        controller.Move(finalMove * Time.deltaTime);

        // Anim
        isMoving = Mathf.Abs(vertical) > 0;
        anim.SetBool("IsRunning", isMoving && !isJumping);
        anim.SetBool("IsGrounded", isGrounded);

        // Footsteps
        bool shouldPlayFootsteps =
            isGrounded &&
            isMoving &&
            !isJumping &&
            canControl &&
            GameManager.Instance.CurrentState == GameManager.GameState.Playing;

        if (shouldPlayFootsteps)
            AudioManager.Instance.StartFootsteps();
        else
            AudioManager.Instance.StopFootsteps();
    }


    public void OnFallOff()
    {
        anim.SetTrigger("Fall");
    }

    public void OnLevelFinished()
    {
        anim.SetBool("IsRunning", false); 
        anim.SetBool("IsGrounded", true);

        anim.SetTrigger("Victory");
    }

    public void KnockBack(Vector3 explosionPosition)
    {
        Vector3 direction = (transform.position - explosionPosition).normalized;

        // player go boom but up
        direction.y = knockBackHeight;
        direction.Normalize();

        knockBackVelocity = direction * knockBackStrength;
        knockBackTimer = knockBackDuration;

        anim.SetTrigger("KnockBack");
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += HandleGameState;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= HandleGameState;
    }

    private void HandleGameState(GameManager.GameState state)
    {
        if (state == GameManager.GameState.TimeUp ||
            state == GameManager.GameState.FellOff ||
            state == GameManager.GameState.LevelFinished)
        {
            canControl = false;
            anim.SetBool("IsRunning", false);
        }
    }

}