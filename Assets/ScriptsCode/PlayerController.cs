using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 7f;
    public float jumpForce = 20f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    private Vector2 move;
    private bool grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputValue value)
    {
        move = value.Get<Vector2>();
    }

    public void OnJump()
    {
        if (grounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            
            grounded = false;
            if (anim != null)
            {
                anim.SetBool("IsGrounded", false);
            }
        }
    }

    public void OnFire()
    {
        Shooter shooter = GetComponent<Shooter>();
        if (shooter != null)
        {
            shooter.Shoot();
        }
    }

    void Update()
    {
        rb.linearVelocity = new Vector2(move.x * speed, rb.linearVelocity.y);

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(move.x));
        }

        if (move.x != 0 && sr != null)
        {
            sr.flipX = move.x < 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = true;
            
            if (anim != null)
            {
                anim.SetBool("IsGrounded", true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            grounded = false;

            if (anim != null)
            {
                anim.SetBool("IsGrounded", false);
            }
        }
    }
}