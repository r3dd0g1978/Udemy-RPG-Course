using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    [Header("Player Detection")]
    [SerializeField] private float checkPlayerDistance;
    [SerializeField] private LayerMask whatIsPlayer;
    private RaycastHit2D isPlayerDetected;

    private bool isAttacking;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlayerDetected)
        {
            if (isPlayerDetected.distance > 1)
            {
                rb.velocity = new Vector2(moveSpeed * 2f * facingDir, 0);
                print("I see the player");
                isAttacking = false;
            }
            else
            {
                print("ATTACKING the " + isPlayerDetected.collider.gameObject.name);
                isAttacking = true;
            }
        }


        if (!isGrounded || isWallDetected)
            Flip();

        Movement();
    }

    private void Movement()
    {
        if (!isAttacking)
            rb.velocity = new Vector2(facingDir * moveSpeed, 0);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        isPlayerDetected = Physics2D.Raycast(transform.position, Vector2.right, checkPlayerDistance * facingDir, whatIsPlayer);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + checkPlayerDistance * facingDir, transform.position.y));
    }
}
