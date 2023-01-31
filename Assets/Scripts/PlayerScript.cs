using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    [Header("Physics")]
    public Rigidbody2D rb;
    public float jumpForce;
    public float rotationSpeed;
    public float walkingSpeed;
    [Header("Sprites")]
    public SpriteRenderer spriteRenderer;
    public Transform spriteTransform;
    public Sprite idleSprite;
    public Sprite jumpingSprite;

    private enum WalkingDirection
    {
        NONE,
        LEFT,
        RIGHT
    }

    private WalkingDirection walkingDirection = WalkingDirection.NONE;
    private bool jumpFlag = false;
    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(rb.velocity.y) > 0.01f)
            isGrounded = false;

        Debug.Log(isGrounded);
        //Debug.Log(Mathf.Abs(rb.velocity.y));

        #region Input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            jumpFlag = true;
            spriteRenderer.sprite = jumpingSprite;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            walkingDirection = WalkingDirection.RIGHT;
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            walkingDirection = WalkingDirection.LEFT;
            spriteRenderer.flipX = true;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.LeftArrow))
            walkingDirection = WalkingDirection.NONE;
        #endregion

        if (rb.velocity.y < -2f && spriteRenderer.sprite == jumpingSprite)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    void FixedUpdate()
    {
        #region Movement
        if (jumpFlag)
        {
            jumpFlag = false;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if (walkingDirection == WalkingDirection.NONE)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            spriteTransform.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime * (walkingDirection == WalkingDirection.RIGHT ? -1 : 1)));
            rb.velocity = new Vector2(walkingSpeed * (walkingDirection == WalkingDirection.RIGHT ? 1 : -1), rb.velocity.y);
        }
        #endregion
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        spriteRenderer.sprite = idleSprite;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }
}
