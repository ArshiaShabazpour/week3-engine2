using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController: MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckDistance = 0.12f; 

    private Rigidbody2D rb;
    private float moveInput;
    private bool wantsJump;
    private bool isGrounded;

    public PlayerState State { get; private set; } = PlayerState.Idle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        moveInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) wantsJump = true;
        UpdateState();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        Vector2 origin = groundCheck != null ? (Vector2)groundCheck.position : (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);
        isGrounded = hit.collider != null;

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (wantsJump && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            wantsJump = false;
        }
        else
        {
            wantsJump = false;
        }
    }

    void UpdateState()
    {
        if (State == PlayerState.Dead) return;

        if (!isGrounded)
            State = PlayerState.Jump;
        else if (Mathf.Abs(moveInput) > 0.1f)
            State = PlayerState.Move;
        else
            State = PlayerState.Idle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
            return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            HandleEnemyCollision(collision);
        }
    }

    private void HandleEnemyCollision(Collision2D collision)
    {
        var enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy == null)
        {
            GameManager.Instance.Lose();
            return;
        }

        ContactPoint2D cp = collision.contacts[0];
        bool stomp = (transform.position.y > cp.point.y) && rb.linearVelocity.y <= 0f;

        if (stomp)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.6f);
            enemy.OnStomped();
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        State = PlayerState.Dead;
        rb.linearVelocity = Vector2.zero;
        GameManager.Instance?.Lose();
    }
}
