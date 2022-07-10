using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Object
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private bool IsGroundedDebug;

    public bool isGrounded {get; set;}
    protected BoxCollider2D boxCollider2d;

    protected override void Awake()
    {
        base.Awake();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        IsGroundedCheck();
        IsGroundCheckDebug();
    }

    private void Dead()
    {
        Destroy(rigidbody2d);
        //局聪皋捞记 贸府
    }

    private void Debug_Dead()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Player Dead");
            Dead();
        }
    }

    private void IsGroundedCheck()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = raycastHit.collider != null ? true : false;
    }

    private void IsGroundCheckDebug()
    {
        if (IsGroundedDebug)
        {
            Color rayColor;

            if (isGrounded)
            {
                rayColor = Color.green;
            }
            else
            {
                rayColor = Color.red;
            }
            Debug.DrawRay(boxCollider2d.bounds.center + new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + groundCheckDistance), rayColor);
            Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, 0), Vector2.down * (boxCollider2d.bounds.extents.y + groundCheckDistance), rayColor);
            Debug.DrawRay(boxCollider2d.bounds.center - new Vector3(boxCollider2d.bounds.extents.x, boxCollider2d.bounds.extents.y + groundCheckDistance), Vector2.right * (boxCollider2d.bounds.size.y), rayColor);
        }
    }
}
