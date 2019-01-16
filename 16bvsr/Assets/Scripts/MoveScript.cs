using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    private Rigidbody2D rb;

    private Collider2D col;

    private Vector2 direction;

    [SerializeField]
    private bool isClimb;


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

    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canJump;


    [SerializeField] private bool jump;
    [SerializeField]
    private float jumpInput;
    [SerializeField]
    private bool jumpPressed;
    float h;

    
    [SerializeField]
    [Tooltip("Бустер гравитации при прыжке")]
    private float gravityMod;

    [SerializeField]
    [Tooltip("Обычная гравитация")]
    private float normalGravity;

    public bool IsGrounded
    {
        get { return isGrounded; }
    }
    public bool IsWallNear
    {
        get
        {
            return Physics2D.Linecast(transform.position, wallChecker.transform.position,
                1 << LayerMask.NameToLayer("Ground"));
        }
    }
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        h = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetButton("Jump");
        // Получаем ось прыжка
        jumpInput = Input.GetAxis("Jump");

        isGrounded = rb.IsTouchingLayers(1 << LayerMask.NameToLayer("Ground"));
        //Physics2D.Linecast(transform.position, groundChecker.transform.position, 1 << LayerMask.NameToLayer("Ground"));
        
        // если нажата кнопка прыжка, можно прыгать и ввод по оси не превышает 1
        if ( jumpPressed && canJump && jumpInput < 1)
        {
            jump = true;
        }
        else
        {
            canJump = false;
            jump = false;
        }

        if ((isGrounded && jumpInput == 0) || (isClimb && jumpInput == 0) )
        {
            canJump = true;
        }                   
        
        if (IsWallNear == true && isGrounded == false)
        {
            if (h >= transform.localScale.x && jumpInput == 0)
            {
                isClimb = true;
            }
            else
            {
                isClimb = false;
            }
        }
        else
        {
            isClimb = false;
        }
        
        // Если сползаем по стене
        if (isClimb)
        {
            jumpForce = climbJumpForce;
            rb.drag = climbDrag;
            rb.gravityScale = 9;
        }
        // если находимся на земле
        else if (isGrounded)
        {
            rb.gravityScale = normalGravity;
            rb.drag = normalDrag;
            isClimb = false;
            jumpForce = normalJumpForce;
        }        
        // если в воздухе
        else
        {
            rb.drag = normalDrag;
            jumpForce = normalJumpForce;
            rb.gravityScale = rb.gravityScale + gravityMod;
            isClimb = false;
        }   
    }

    private void FixedUpdate()
    {
        if (h != 0)
        {
            if(!IsWallNear)
                rb.AddForce(new Vector2(h, 0) * moveSpd);
            Flip(h);
        }
                            
        Vector2 direction = Vector2.zero;
        
        if (Input.GetButtonDown("Jump") && isClimb)
        {
            direction = new Vector2(jumpForce * -transform.localScale.x, jumpForce / modY);
            rb.AddForce(direction, ForceMode2D.Impulse);
        }
        else if (jump == true){
            direction = transform.up * jumpForce;   
            rb.AddForce(direction, ForceMode2D.Force);
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
        else if (other.CompareTag("Bound"))
        {
            SceneManager.LoadScene("LevelSwitcher");
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
