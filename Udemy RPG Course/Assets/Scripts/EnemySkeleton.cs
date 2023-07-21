using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkeleton : Entity
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        rb.velocity = new Vector2(facingDir * moveSpeed, 0);

        if (!isGrounded || isWallDetected)
            Flip();
    }
}
