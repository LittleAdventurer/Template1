using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : Object, IGameOverObsever
{
    const bool LEFT = true;
    const bool RIGHT = false;

    [SerializeField] private float accelation, airAccelation;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float groundDragValue;
    [SerializeField] private float jumpForce;
    [SerializeField] private float longJumpAcceleation;
    [SerializeField] private float shortJumpAcceleation;

    private bool isLongJumping;

    private Player player;
    protected BoxCollider2D boxCollider2d;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        SpriteRotate();
        
        Accelation();
        GroundDrag();
        MaxSpeed();
        Jump();
        ShortLongJump();
    }

    private void SpriteRotate()     //바라보는 방향에 따라 스프라이트 좌우반전
    {
        float x = Input.GetAxisRaw("Horizontal");

        if(x == 1)
        {
            spriteRenderer.flipX = RIGHT;
        }
        else if(x == -1)
        {
            spriteRenderer.flipX = LEFT;
        }
    }

    void Accelation()
    {
        float x = Input.GetAxis("Horizontal");

        if (player.isGrounded)
        {
            rigidbody2d.AddForce(Vector2.right * x * accelation * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            rigidbody2d.AddForce(Vector2.right * x * airAccelation * Time.deltaTime, ForceMode2D.Force);
        }
    }

    private void GroundDrag()
    {
        float x = Input.GetAxisRaw("Horizontal");

        if (player.isGrounded && Mathf.Sign(rigidbody2d.velocity.x) != x)
        {
            rigidbody2d.AddForce(Vector2.left * rigidbody2d.velocity.x * groundDragValue * Time.deltaTime, ForceMode2D.Force);
        }
        
    }

    private void MaxSpeed()
    {
        if (Mathf.Abs(rigidbody2d.velocity.x) > maxSpeed)
        {
            rigidbody2d.velocity = new Vector2(Mathf.Sign(rigidbody2d.velocity.x) * maxSpeed, rigidbody2d.velocity.y);
        }
    }

    private void Jump()
    {
        if(player.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2d.velocity += Vector2.up * jumpForce;
            isLongJumping = true;
        }
    }

    private void ShortLongJump()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            isLongJumping = false;
        }

        if (isLongJumping && rigidbody2d.velocity.y > 0)
        {
            rigidbody2d.AddForce(Vector2.up * rigidbody2d.velocity.y * longJumpAcceleation * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            rigidbody2d.AddForce(Vector2.down * rigidbody2d.velocity.y * shortJumpAcceleation * Time.deltaTime, ForceMode2D.Force);
        }
    }

    public void GameOver()
    {
        Debug.Log("PlayerControl_GameOver");
    }
}
