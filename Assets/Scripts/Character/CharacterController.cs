using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [SerializeField]
    private float jumpTime = 0.3f;
    [SerializeField]
    private float jumpForce = 8f;
    [SerializeField]
    private float fallMultiplier = 1f;
    [SerializeField]
    private float jumpMultiplier = 1f;

    [SerializeField]
    private List<string> attackAnimations = new List<string> {PLAYER_ATTACK, PLAYER_ATTACK_2};
    [SerializeField]
    private float comboTimeThreshold = 1f;

    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundLayer;

    private float speed = 6f;
    private float xAxis;

    private bool isJumpPressed;
    private bool isJumping;
    private float jumpCount;

    private bool isAttackPressed;
    private bool isAttacking = false;
    private bool isDoubleAttacking;
    private bool isAnimatingAttack;
    private int clickCount;
    private float lastClickTime;
    private int currentAttackAnimationIndex = 0;

    private string currentState;

    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 vecGravity;

    const string PLAYER_IDLE = "Rogue_idle_01";
    const string PLAYER_RUN = "Rogue_run_01";
    const string PLAYER_JUMP = "Rogue_idle_01";
    const string PLAYER_ATTACK = "Rogue_attack_03";
    const string PLAYER_ATTACK_2 = "Rogue_attack_02";
    const string PLAYER_DEATH = "Rogue_death_01";
    private const int MaxJumpCount = 1;
    private const float GroundCheckCapsuleSizeX = 15.5f;
    private const float GroundCheckCapsuleSizeY = 0.82f;

    void Start()
    {
        vecGravity = new Vector2(0, -Physics2D.gravity.y);
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        ChangeAnimationState(PLAYER_IDLE);
    }

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space))
        { 
            isJumpPressed = true;
        }

        if (Input.GetMouseButtonDown(0))
        {
            isAttackPressed = true;
            lastClickTime = Time.time;
        }
    }

    void FixedUpdate() {
        HandleJumping();
        HandleMovement();
        HandleAttack();
    }

    void ChangeAnimationState(string newState){
        if (currentState == newState) return;

        animator.Play(newState);
        currentState = newState;
    }

    bool isGrounded() {
        return Physics2D.OverlapCapsule(groundCheck.position, new Vector2(GroundCheckCapsuleSizeX, GroundCheckCapsuleSizeY), CapsuleDirection2D.Horizontal, 0, groundLayer);
    }

    void HandleJumping()
    {
        if (isJumpPressed && isGrounded()) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
            jumpCount = 0;
        }

        if (rb.velocity.y > 0 && isJumping){
            jumpCount += Time.deltaTime;
            if (jumpCount > jumpTime) isJumping = false;

            rb.velocity += vecGravity * jumpMultiplier * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            isJumping = false;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
        isJumpPressed = false;
    }

    void HandleMovement()
    {
        Vector2 vel = new Vector2(0, rb.velocity.y);

        if (xAxis < 0)
        {
            vel.x = -speed;
            transform.localScale = new Vector2(-0.25f, 0.25f);
        }
        else if (xAxis > 0)
        {
            vel.x = speed;
            transform.localScale = new Vector2(0.25f, 0.25f);
        }
        else
        {
            vel.x = 0;
        }

        if(!isAttacking){
            if (xAxis != 0 && isGrounded())
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }
        

        rb.velocity = vel;
    }

    bool isCurrentAnimationAttack {
    get {
        return attackAnimations.Contains(currentState);
    }
}

    void HandleAttack()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (isAttackPressed && !isAttacking)
        {
            isAttackPressed = false;
            isAttacking = true;
            clickCount++;
            isAnimatingAttack = false;

            // Check if enough time has passed to reset the click count
            if (Time.time - lastClickTime > comboTimeThreshold)
            {
                clickCount = 0;
            }

            if (clickCount > attackAnimations.Count)
            {
                clickCount = 0;
            }

            ChangeAnimationState(attackAnimations[clickCount]);
            currentAttackAnimationIndex = clickCount;
        }

        if (stateInfo.IsName(attackAnimations[currentAttackAnimationIndex]) && !isAnimatingAttack)
        {
            isAnimatingAttack = true;

            if (clickCount == 2)
            {
                isDoubleAttacking = true;
                clickCount = 0;
                Invoke("PlaySecondAttackAnimation", stateInfo.length);
            }
        }

        if (isAnimatingAttack && stateInfo.normalizedTime >= 1)
        {
            isAttacking = false;
            isAnimatingAttack = false;
            clickCount = 0;
            ChangeAnimationState(PLAYER_IDLE);
        }
    }

    void PlaySecondAttackAnimation()
    {
        ChangeAnimationState(attackAnimations[clickCount]);
        currentAttackAnimationIndex = 1;
        isAnimatingAttack = false;
    }
}
