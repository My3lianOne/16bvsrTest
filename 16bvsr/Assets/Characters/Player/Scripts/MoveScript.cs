using System;
using System.Collections;
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
    private float modY = 4;

    
    [SerializeField]

    private float pauseTime = 4;
    
    [SerializeField]
    [Tooltip("Скорость перемещения")]
    float moveSpd = 0;
    [SerializeField]
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
    GameObject [] groundCheckers;
    
    private Rigidbody2D rb;

    private Collider2D col;

    private Vector2 direction;

    [SerializeField]
    private bool isClimb;

    public bool IsClimbed => isClimb;

    [SerializeField]
    [Tooltip("Сопротивление (замедление) при обычном состоянии")]
    float normalDrag = 2;
    
    [SerializeField]
    [Tooltip("Сопротивление (замедление) в воздухе")]
    float airDrag = 2;
    
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
    private bool jumpRequest;
    [SerializeField]
    private bool jumpPressed;
    [SerializeField]
    private bool isBounce;
    float h;
    [SerializeField]
    private bool canBounce;
    [SerializeField]
    private bool canMove;
    
    [SerializeField]
    [Tooltip("Бустер гравитации при прыжке")]
    private float gravityMod;

    [SerializeField]
    [Tooltip("Обычная гравитация")]
    private float normalGravity;
    [SerializeField]
    [Range(1,10)]
    private float fallMultyplier = 2.5f;
    [SerializeField]
    [Range(0,10)]
    private float climbMultyplier = 2.5f;
    [SerializeField]
    [Range(1,10)]
    private float lowJumpMultiplier = 2f;
    private float gravity = 25;
    private bool fallPause;
    
    private Vector3 m_Velocity = Vector3.zero;
    
    private bool isIdle;
    public bool IsGrounded => isGrounded;
    
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    public bool IsWallNear
    {
        get
        {
            return Physics2D.Linecast(transform.position, wallChecker.transform.position,
                1 << LayerMask.NameToLayer("Ground"));
        }
    }


    #region Animator

    private Animator animator;
    private static readonly int JumpInput = Animator.StringToHash("jumpInput");
    private static readonly int Grounded = Animator.StringToHash("IsGrounded");
    private static readonly int CanJump = Animator.StringToHash("canJump");
    private static readonly int Running = Animator.StringToHash("Running");
    private static readonly int IsClimb = Animator.StringToHash("IsClimb");
    private static readonly int IsIdle = Animator.StringToHash("IsIdle");

    #endregion

    
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        fallPause = true;
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        GroundCheck();
        h = Input.GetAxisRaw("Horizontal");
        jumpPressed = Input.GetButton("Jump");
        // Получаем ось прыжка
        if(Input.GetButtonDown("Jump"))
            jumpRequest = true;

        



        if (IsWallNear == true && isGrounded == false)
        {
            if (h == transform.localScale.x && rb.velocity.y < 0)
            {
                
//                if (fallPause)
//                {
//                    StartCoroutine(nameof(FallPause));
//                }
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

        if (IsWallNear || canBounce)
            canMove = false;
        else
            canMove = true;
        
        // Если сползаем по стене
        if (isClimb)
        {
            
            rb.drag = climbDrag;
            rb.gravityScale = climbMultyplier;
            canBounce = true;
        }
        // если находимся на земле
        else if (isGrounded)
        {
            rb.gravityScale = normalGravity;
            rb.drag = normalDrag;
            isClimb = false;
            canBounce = false;
        }        
        // если в воздухе
        else
        {
            rb.drag = airDrag;
            //rb.gravityScale = rb.gravityScale + gravityMod;
            fallPause = true;
        } 
        
        if (canBounce && Input.GetButtonDown("Jump"))
        {
            ;
//            StopAllCoroutines();
//            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            
        }

        if (h == 0 && !isClimb && !isBounce && isGrounded) 
        {
            if(!IsInvoking())
                Invoke(nameof(SetIdleState), 3);
        }
        else
        {
            isIdle = false;
        }
    }


    private void FixedUpdate()
    {
        if (h != 0)
        {    
            if(!canBounce)
                Flip(h);
            if (canMove)
            {
                Vector3 targetVelocity = new Vector2(h * 10f * Time.fixedDeltaTime * moveSpd, rb.velocity.y);
                // And then smoothing it out and applying it to the character
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
            }
                
        }

        if (jumpRequest && isClimb)
        {
            Flip(-transform.localScale.x);
            rb.AddForce((Vector2.up + new Vector2(transform.localScale.x, 0))* jumpForce);
            jumpRequest = false;
            
        }
        else if (jumpRequest)
        {
            rb.AddForce(Vector2.up * jumpForce);
            jumpRequest = false;
        }
        

        if (rb.velocity.y < 0 && isClimb)
        {
            rb.gravityScale = normalGravity;
        }
        else if (rb.velocity.y < 0)
        {
            rb.gravityScale = fallMultyplier;
        } else if (rb.velocity.y > 0 && !jumpPressed)
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = normalGravity;
        }
        

    
//        if (isBounce)
//        {
//            direction = new Vector2(transform.localScale.x * jumpForce / modY, jumpForce * (1 - jumpInput));
//            //rb.velocity = new Vector2(-transform.localScale.x, modY)* jumpForce;
//            rb.AddForce(direction, ForceMode2D.Force);                              
//        }
//        else if (jump){
//            direction = transform.up * jumpForce * (1 - jumpInput);   
//            rb.AddForce(direction, ForceMode2D.Force);
//        }
//        rb.velocity = Vector2.zero;
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
            transform.localScale = new Vector3(h > 0 ? 1 : -1, transform.localScale.y, transform.localScale.z);
    }

    private void GroundCheck()
    {
        int fails = 0;
        foreach (var checker in groundCheckers)
        {
            if (Physics2D.Linecast(transform.position, checker.transform.position,
                    1 << LayerMask.NameToLayer("Ground")) == false)
                fails++;
        }

        isGrounded = fails < 3;
    }

    private void LateUpdate()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool(Grounded, isGrounded);
        animator.SetBool(CanJump, canJump);
        animator.SetInteger(Running, Convert.ToInt32(h));
        animator.SetBool(IsClimb, isClimb);
        animator.SetBool(IsIdle, isIdle);
    }

    void SetIdleState()
    {
        isIdle = true;
    }

    IEnumerator FallPause()
    {
        fallPause = false;
        
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(pauseTime);
        
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

}
