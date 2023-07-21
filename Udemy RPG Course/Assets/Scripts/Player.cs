using UnityEngine;

public class Player : Entity
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;

    [Header("Attack")]
    [SerializeField] private float comboCooldown;
    private float comboTimer;
    private bool isAttacking;
    private int comboCounter = 0;

    [Header("Dash")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCooldown;
    private float dashTimer;
    private float dashCooldownTimer;

    private float xInput, yInput;

    protected override void Start()
    {
        base.Start();

        if (wallCheck == null)
            wallCheck = transform;
    }

    protected override void Update()
    {
        base.Update();

        Timers();
        CheckInput();
        Movement();
        AnimatorController();
        FlipController();
    }

    private void Dash()
    {
        if (dashCooldownTimer < 0 && !isAttacking)
        {
            dashCooldownTimer = dashCooldown;
            dashTimer = dashDuration;
        }
    }

    private void Timers()
    {
        dashCooldownTimer -= Time.deltaTime;
        dashTimer = dashTimer - Time.deltaTime;
        comboTimer = comboTimer - Time.deltaTime;
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.LeftShift))
            Dash();

        if (Input.GetKeyDown(KeyCode.Mouse0))
            TriggerAttackEvent();
    }

    private void TriggerAttackEvent()
    {
        if (!isGrounded)
            return;

        if (comboTimer < 0)
            comboCounter = 0;

        isAttacking = true;
        comboTimer = comboCooldown;
    }

    private void Movement()
    {
        if (isAttacking)
            rb.velocity = Vector2.zero;
        else if (dashTimer > 0)
            rb.velocity = new Vector2(facingDir * dashSpeed, 0);
        else
            rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if (isGrounded)
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void AnimatorController()
    {
        //isMoving = true, if x does not = 0. Boolean values: 1 = true, 0 = false
        bool isMoving = rb.velocity.x != 0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDashing", dashTimer > 0);
        anim.SetBool("isAttacking", isAttacking);
        anim.SetInteger("comboCounter", comboCounter);
    }

    public void AttackOver()
    {
        comboCounter++;
        isAttacking = false;
        if (comboCounter > 2)
            comboCounter = 0;
    }

    private void FlipController()
    {
        if (rb.velocity.x > 0 && !facingRight)
            Flip();
        else if (rb.velocity.x < 0 && facingRight)
            Flip();
    }

}
