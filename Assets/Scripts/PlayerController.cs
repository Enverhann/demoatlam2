using UnityEngine;

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
    public GameObject progress;
    public Material material;
    private float _fade = 0;
    float lerpSpeed = 1f;
    private float timer;
    public bool isStarting = true;

    public float sensitivityMultiplier;
    public float deltaThreshold;
    Vector2 firstTouchPosition;
    float finalTouchX;
    float finalTouchY;
    Vector2 currentTouchPosition;
    public float minXPos;
    public float maxXPos;
    public Vector2 touchDelta;
    void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _boxCollider2d = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        material.SetFloat("_Fade", _fade);
        if (isStarting)
        {
            timer = Mathf.Clamp01(timer + Time.deltaTime * lerpSpeed);
        }
        _fade = Mathf.Lerp(0, 1, timer);

        if (canMove && IsGrounded())
        {
            SwipeMovement();
        }
    }

    private void FixedUpdate()
    {
        if (jumpValue >= 48f && !facingRight && IsGrounded() && canMove)
        {
            float tempx = horizontalInput - walkSpeed;
            float tempy = jumpValue;
            _rb2d.velocity = new Vector2(tempx, tempy);
            jumpValue = 0;
            Invoke("ResetJump", 0.01f);
        }
        else if (jumpValue >= 48f && facingRight && IsGrounded() && canMove)
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
        Vector3 canvasScale = progress.transform.localScale;
        theScale.x *= -1;
        canvasScale.x *= -1f;
        transform.localScale = theScale;
        progress.transform.localScale = canvasScale;
    }
    void ResetValues()
    {
        _rb2d.velocity = new Vector2(0, 0);
        firstTouchPosition = Vector2.zero;
        finalTouchX = 0f;
        finalTouchY = 0;
        currentTouchPosition = Vector2.zero;
        touchDelta = Vector2.zero;
    }

    void SwipeMovement()
    {
        if (Input.GetMouseButtonDown(0))
        {
            firstTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            currentTouchPosition = Input.mousePosition;
            touchDelta = (currentTouchPosition - firstTouchPosition);

            if (firstTouchPosition == currentTouchPosition)
            {
                _rb2d.velocity = new Vector2(0, 0);
            }
            finalTouchX = transform.position.x;
            finalTouchY = transform.position.y;

            if (Mathf.Abs(touchDelta.x) >= 0 && touchDelta.y == 0 && jumpValue==0)
            {
                finalTouchX = (transform.position.x + (touchDelta.x * sensitivityMultiplier));
            }
            if (Mathf.Abs(touchDelta.y) >= deltaThreshold && touchDelta.x == 0)
            {
                jumpValue += 1.35f;
            }

            _rb2d.position = new Vector2(finalTouchX, transform.position.y);
            _rb2d.position = new Vector2(Mathf.Clamp(_rb2d.position.x, minXPos, maxXPos), _rb2d.position.y);

            firstTouchPosition = Input.mousePosition;

            if (touchDelta.x > 0 && !facingRight)
            {
                Flip();
            }else if(touchDelta.x < 0 && facingRight)
            {
                Flip();
            }
        }
        
        if (Input.GetMouseButtonUp(0))
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
            if (canJump && jumpValue==0)
            {
                ResetValues();
            }
           canJump = true;          
        }
    }
}
