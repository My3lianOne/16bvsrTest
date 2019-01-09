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

    private Rigidbody2D rb;

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

    public bool IsClimb
    {
        get { return Physics2D.Linecast(transform.position, wallChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")) && !isGrounded && Input.GetAxisRaw("Horizontal") != 0; }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
    }

    private void FixedUpdate()
    {
        Flip(h);
        rb.AddForce(new Vector2(h, 0) * moveSpd);

        if (Input.GetButton("Jump") && canJump)
        {
            Vector2 direction;
            if (IsClimb)
                direction = new Vector2(jumpForce * -transform.localScale.x, jumpForce / modY);
            else
                direction = new Vector2(0, jumpForce * (1 - v));
            rb.AddForce(direction, ForceMode2D.Impulse);
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {
            rb.gravityScale = normalGravity;
            isGrounded = true;
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {            
            rb.gravityScale = rb.gravityScale + gravityMod;
            isGrounded = false;
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
