using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    private Rigidbody2D _rb2d;
    public LayerMask groundMask;
    public float walkSpeed = 8f;
    public bool canJump = true;
    public float jumpValue = 0.0f;
    private BoxCollider2D _boxCollider2d;
    public bool facingRight = true;
    public bool canMove = true;
    public Canvas canvas;
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (CrossPlatformInputManager.GetButtonDown("Jump") && IsGrounded() && canJump && canMove)
        {
            _rb2d.velocity = new Vector2(0.0f, _rb2d.velocity.y);
        }
        if (CrossPlatformInputManager.GetButtonUp("Jump") && canMove)
        {
            if (facingRight && IsGrounded())
            {
                float tempx = walkSpeed;
                float tempy = jumpValue;
                _rb2d.velocity = new Vector2(tempx, tempy);
                Invoke("ResetJump", 0.05f);
            }
            else if (!facingRight && IsGrounded() && canMove)
            {
                float tempx = -walkSpeed;
                float tempy = jumpValue;
                _rb2d.velocity = new Vector2(tempx, tempy);
                Invoke("ResetJump", 0.05f);
            }
            canJump = true;
        }
    }

    private void FixedUpdate()
    {
        horizontalInput=CrossPlatformInputManager.GetAxis("Horizontal");
        if (horizontalInput > 0 && !facingRight && canMove)
        {
            Flip();
        }
        else if (horizontalInput < 0 && facingRight && canMove)
        {
            Flip();
        }

        if (jumpValue == 0.0f && IsGrounded() && canMove)
        {
            _rb2d.velocity = new Vector2(horizontalInput * walkSpeed, _rb2d.velocity.y);
        }

        if (CrossPlatformInputManager.GetButton("Jump") && IsGrounded() && canJump && canMove)
        {
            jumpValue += 0.8f;
        }
        if (jumpValue >= 42f && !facingRight && IsGrounded() && canMove)
        {
            float tempx = horizontalInput - walkSpeed;
            float tempy = jumpValue;
            _rb2d.velocity = new Vector2(tempx, tempy);
            jumpValue = 0;
            Invoke("ResetJump", 0.01f);
        }

        else if (jumpValue >= 42f && facingRight && IsGrounded() && canMove)
        {
            float tempx = walkSpeed;
            float tempy = jumpValue;
            _rb2d.velocity = new Vector2(tempx, tempy);
            jumpValue = 0;
            Invoke("ResetJump", 0.01f);
        }
    }
    
    public void ResetJump()
    {
        jumpValue = 0;
        canJump = false;
    }
    private bool IsGrounded()
    {
        float extraHeightText = 0.1f;
        RaycastHit2D raycastHit = Physics2D.BoxCast(_boxCollider2d.bounds.center, _boxCollider2d.bounds.size, 0f, Vector2.down, extraHeightText, groundMask);
        Color rayColor;
        if (raycastHit.collider != null)
        {
            rayColor = Color.green;
            canJump = true;
        }
        else
        {
            rayColor = Color.red;
            canJump = false;
        }
        Debug.DrawRay(_boxCollider2d.bounds.center + new Vector3(_boxCollider2d.bounds.extents.x, 0), Vector2.down * (_boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(_boxCollider2d.bounds.center - new Vector3(_boxCollider2d.bounds.extents.x, 0), Vector2.down * (_boxCollider2d.bounds.extents.y + extraHeightText), rayColor);
        Debug.DrawRay(_boxCollider2d.bounds.center - new Vector3(_boxCollider2d.bounds.extents.x, _boxCollider2d.bounds.extents.y + extraHeightText), Vector2.right * (_boxCollider2d.bounds.extents.x + extraHeightText / 1.5f), rayColor);
        Debug.Log(raycastHit.collider);
        Debug.Log(raycastHit.collider);
        Debug.Log(raycastHit.collider);
        return raycastHit.collider != null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Platforms"))
        {
            jumpValue = 0;
        }
    }
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        Vector3 canvasScale = canvas.transform.localScale;
        theScale.x *= -1;
        canvasScale.x *= -1f;
        transform.localScale = theScale;
        canvas.transform.localScale = canvasScale;
    }
}