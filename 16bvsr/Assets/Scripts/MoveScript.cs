using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveScript : MonoBehaviour
{
    /// <summary>
    /// Модификатор уменьшения силы прыжка по Y;
    /// </summary>
    [SerializeField]
    [Tooltip("Модификатор уменьшения силы прыжка по Y")]
    private int modY = 4;

    [SerializeField]
    [Tooltip("Скорость перемещения")]
    float moveSpd = 0;

    private float jumpForce = 0;

    /// <summary>
    /// "Щуп" стен.
    /// </summary>
    [Tooltip("Щуп стен")]
    [SerializeField]
    GameObject wallChecker;
    
    /// <summary>
    /// "Щуп" стен.
    /// </summary>
    [Tooltip("Щуп пола")]
    [SerializeField]
    GameObject groundChecker;

    /// <summary>
    /// Проверка на застревание внутри коллайдера.
    /// </summary>
    [Tooltip("Щуп пола")]
    [SerializeField]
    GameObject absChecker;
    
    private Rigidbody2D rb;

    private Collider2D col;

    private Vector2 direction;


    [SerializeField]
    [Tooltip("Сопротивление (замедление) при обычном состоянии")]
    float normalDrag = 2;

    [SerializeField]
    [Tooltip("Скорость скольжения по стене (больше - медленнее)")]
    float climbDrag = 50;

    [SerializeField]
    [Tooltip("Обычная сила прыжка")]
    float normalJumpForce = 2;

    [SerializeField]
    [Tooltip("Сила прыжка при скольжении по стене")]
    float climbJumpForce = 50;

    bool isGrounded = true;
    bool canJump = true;

    float v;
    float h;

    
    [SerializeField]
    [Tooltip("Бустер гравитации при прыжке")]
    private float gravityMod;

    [SerializeField]
    [Tooltip("Обычная гравитация")]
    private float normalGravity;

    public bool IsGrounded
    {
        get
        {
            return Physics2D.Linecast(transform.position, groundChecker.transform.position,
                1 << LayerMask.NameToLayer("Ground"));
        }
    }
    public bool IsClimb
    {
        get { return Physics2D.Linecast(transform.position, wallChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")) && IsGrounded != true && Input.GetAxisRaw("Horizontal") != 0; }
    }
    
    public bool IsAbs
    {
        get { return Physics2D.Raycast(groundChecker.transform.position, transform.position, 1 << LayerMask.NameToLayer("Ground")); }
    }

    void Start()
    {

        Time.timeScale = 0.5f;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxis("Jump");

        if(Input.GetButtonUp("Jump"))
        {
            canJump = false;
        }

        if (IsClimb)
        {
            jumpForce = climbJumpForce;
            rb.drag = climbDrag;
            canJump = true;
        }
        else
        {
            rb.drag = normalDrag;
            jumpForce = normalJumpForce;            
        }

        if (IsGrounded)
        {
            rb.gravityScale = normalGravity;
            canJump = true;
        }
        else
        {
            rb.gravityScale = rb.gravityScale + gravityMod;
        }

    }

    private void FixedUpdate()
    {
        Flip(h);
        rb.AddForce(new Vector2(h, 0) * moveSpd);

        Vector2 direction = Vector2.zero;
        if (Input.GetButtonDown("Jump") && canJump && IsClimb)
        {
            direction = new Vector2(jumpForce * -transform.localScale.x, jumpForce / modY);
            rb.AddForce(direction, ForceMode2D.Impulse);
        }
        else if (Input.GetButton("Jump")&& canJump && IsClimb != true){
            direction = new Vector2(0, jumpForce * (1 - v));   
            rb.AddForce(direction, ForceMode2D.Impulse);
        }

    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {
            
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {            
            
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            col.enabled = false;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            col.enabled = true;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            col.enabled = false;
        }
    }

    /// <summary>
    /// Переворачивает персонажа в зависимости от направления движения
    /// </summary>
    /// <param name="h">Направление</param>
    private void Flip(float h)
    {
        if (h != 0)
            transform.localScale = new Vector3(h, transform.localScale.y, transform.localScale.z);
    }
}
