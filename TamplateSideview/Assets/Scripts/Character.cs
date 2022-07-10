using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //상수
    const bool RIGHT = true;
    const bool LEFT = false;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float accelation, airAccelation;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float groundDragValue;
    [SerializeField] private float jumpForce;
    [SerializeField] private float longJumpAcceleation;
    [SerializeField] private float shortJumpAcceleation;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private bool IsGroundedDebug;

    private bool isLongJumping;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SpriteRotate();

        IsGroundCheckDebug();

        Move_addforce();
        GroundDrag();
        MaxSpeed();
        Jump();
        ShortLongJump();

        Debug_Dead();
    }

    private void LateUpdate()
    {

    }

    private void SpriteRotate()     //바라보는 방향에 따라 스프라이트 좌우반전
    {
        float x = Input.GetAxisRaw("Horizontal");

        if(x == 1)
        {
            spriteRenderer.flipX = LEFT;
        }
        else if(x == -1)
        {
            spriteRenderer.flipX = RIGHT;
        }
    }

    private void Move_transform()
    {
        float x = Input.GetAxis("Horizontal");

        transform.Translate(Vector2.right * x * Time.deltaTime * moveSpeed);      //for Update()
        transform.Translate(Vector2.right * x * Time.fixedDeltaTime * moveSpeed);       //for FixedUpdate()
    }

    private void Move_velocity()
    {
        float x = Input.GetAxis("Horizontal");

        rigidbody2d.velocity =  new Vector2(x * moveSpeed, rigidbody2d.velocity.y);
    }

    private void Move_addforce()
    {
        float x = Input.GetAxis("Horizontal");

        if (IsGrounded())
        {
            rigidbody2d.AddForce(Vector2.right * x * accelation * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            rigidbody2d.AddForce(Vector2.right * x * airAccelation * Time.deltaTime, ForceMode2D.Force);
        }
    }
    
    private void MaxSpeed()
    {
        if (Mathf.Abs(rigidbody2d.velocity.x) > maxSpeed)
        {
            rigidbody2d.velocity = new Vector2(Mathf.Sign(rigidbody2d.velocity.x) * maxSpeed, rigidbody2d.velocity.y);
        }
    }

    private void GroundDrag()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float debugFloat = rigidbody2d.velocity.x;
        float sign = Mathf.Sign(rigidbody2d.velocity.x);

        if (IsGrounded() && Mathf.Sign(rigidbody2d.velocity.x) != x)
        {
            rigidbody2d.AddForce(Vector2.left * rigidbody2d.velocity.x * groundDragValue * Time.deltaTime, ForceMode2D.Force);
        }
        
    }

    private void Jump()
    {
        if(IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2d.velocity += Vector2.up * jumpForce;
            isLongJumping = true;
        }
    }

    private void ShortLongJump()
    {
        if (isLongJumping && rigidbody2d.velocity.y > 0 && Input.GetKey(KeyCode.Space))
        {
            rigidbody2d.AddForce(Vector2.up * rigidbody2d.velocity.y * longJumpAcceleation * Time.deltaTime, ForceMode2D.Force);
        }
        else
        {
            rigidbody2d.AddForce(Vector2.down * rigidbody2d.velocity.y * shortJumpAcceleation * Time.deltaTime, ForceMode2D.Force);
            isLongJumping = false;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, groundCheckDistance, groundLayer);

        return raycastHit.collider != null;
    }

    private void IsGroundCheckDebug()
    {
        if (IsGroundedDebug)
        {
            Color rayColor;

            if (IsGrounded())
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

    private void Dead()
    {
        //애니메이션

        Destroy(rigidbody2d);
    }

    private void Debug_Dead()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("Player Dead");
            Dead();
        }
    }

}
