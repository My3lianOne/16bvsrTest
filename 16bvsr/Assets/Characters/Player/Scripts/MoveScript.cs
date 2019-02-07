using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveScript : MonoBehaviour
{
    
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
    [Tooltip("Щуп пола")]
    [SerializeField]
    GameObject [] wallCheckers;
    private Rigidbody2D rb;
    [SerializeField]
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

    [SerializeField] private bool isGrounded;
    [SerializeField]
    private bool jumpRequest;
    [SerializeField]
    private bool jumpPressed;
  
    private float h;    
    
    [SerializeField]
    private bool canMove;
        
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

    private bool bouncing;
    
    private Vector3 m_Velocity = Vector3.zero;
    
    private bool isIdle;
    public bool IsGrounded => isGrounded;

    public float groundRemember;
    [Range(0, 1f)] [SerializeField]
    public float groundRememberTime;
    
    public float climbRemember;
    [Range(0, 1f)] [SerializeField]
    public float climbRememberTime;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;

    private HealthController healthController;
//    public bool IsWallNear
//    {
//        get
//        {
//
////            return Physics2D.Linecast(transform.position, wallChecker.transform.position,
////                1 << LayerMask.NameToLayer("Ground"));
//        }
//    }

    private bool isWallNear;


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
        fallPause = true;
        animator = GetComponent<Animator>();
        healthController = GetComponentInChildren<HealthController>();
    }
    
    void Update()
    {
        GroundCheck();
        WallCheck();
        h = Input.GetAxisRaw("Horizontal");

        if (h != 0)
        {
            h = 1 * Mathf.Sign(h);
        }

        
        jumpPressed = Input.GetButton("Jump");

        if(Input.GetButtonDown("Jump"))
            jumpRequest = true;
     
        if (isWallNear && isGrounded == false)
        {
            
            if (h == transform.localScale.x && rb.velocity.y <= 0)
            {
                if(fallPause)
                    StartCoroutine(nameof(FallPause));
                if (Input.GetButton("Jump"))
                {
                    StopAllCoroutines();
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                }
            }
            else
            {
                isClimb = false;
                fallPause = true;
            }
        }
        else
        {
            isClimb = false;
            fallPause = true;
        }

        if (isWallNear || bouncing)
            canMove = false;
        else
            canMove = true;
        
        // Если сползаем по стене
        if (isClimb)
        {            
            rb.drag = climbDrag;
            rb.gravityScale = climbMultyplier;
            climbRemember = climbRememberTime;
        }
        // если находимся на земле
        else if (isGrounded)
        {
            rb.gravityScale = normalGravity;
            rb.drag = normalDrag;
            isClimb = false;
            groundRemember = groundRememberTime;
            climbRemember = 0;
        }        
        // если в воздухе
        else
        {
            rb.drag = airDrag;
            //rb.gravityScale = rb.gravityScale + gravityMod;
            groundRemember -= Time.deltaTime;
            climbRemember -= Time.deltaTime;
        }

        if (bouncing && rb.velocity.y < 0)
            bouncing = false;
        
        if (h == 0 && !isClimb && !bouncing && isGrounded) 
        {
            if(!IsInvoking())
                Invoke(nameof(SetIdleState), 3);
        }
        else
        {
            if(IsInvoking())
                CancelInvoke();
            isIdle = false;
        }

    }
    [Range(0, 1f)]
    [SerializeField] private float fCutJumpHeight;

    [Range(0, 1f)]
    [SerializeField] float horizontalDumpingStopping;
    [Range(0, 1f)]
    [SerializeField] float horizontalDumpingTurning;
    [Range(0, 1f)]
    [SerializeField] float horizontalDumpingBasic;
    private void FixedUpdate()
    {
        if (h != 0)
        {    
            if(!bouncing)
                Flip(h);
               
            if (canMove)
            {                
                /*Vector3 targetVelocity = new Vector2(h * 10f * Time.fixedDeltaTime * moveSpd, rb.velocity.y);
                // And then smoothing it out and applying it to the character
                rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);*/

                float horizontalVelocity = rb.velocity.x;
                horizontalVelocity += h;

                if (Mathf.Abs(h) < 0.01f)
                {
                    horizontalVelocity *= Mathf.Pow(1f - horizontalDumpingStopping, Time.deltaTime * 10f);
                }
                else if (Mathf.Sign(h) != Mathf.Sign(horizontalVelocity))
                {
                    horizontalVelocity *= Mathf.Pow(1f - horizontalDumpingTurning, Time.deltaTime * 10f);
                }
                else
                {
                    horizontalVelocity *= Mathf.Pow(1f - horizontalDumpingBasic, Time.deltaTime * 10f);
                }
                
                rb.velocity = new Vector2(horizontalVelocity, rb.velocity.y);
            }
                
        }
//        else if (!bouncing && (h == 0 && !isGrounded))
//            rb.velocity = new Vector2(0, rb.velocity.y);

        if (Input.GetButtonUp("Jump"))
        {
            if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * fCutJumpHeight);
            }
        }
        
        if (jumpRequest && (climbRemember > 0))
        {            
            bouncing = true;
            Flip(-transform.localScale.x);
            rb.velocity = (Vector2.up + new Vector2(transform.localScale.x, 1)) * (jumpForce*0.7f);
            //rb.AddForce((Vector2.up + new Vector2(transform.localScale.x, 0)) * jumpForce);
            jumpRequest = false;            
        }
        else if (jumpRequest && groundRemember > 0)
        {
//          rb.AddForce(Vector2.up * jumpForce);
//            
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequest = false;
        }
        
//
//        if (rb.velocity.y < 0 && isClimb)
//        {
//            rb.gravityScale = normalGravity;
//        }
//        else if (rb.velocity.y < 0)
//        {
//            rb.gravityScale = fallMultyplier;
//        } else if (rb.velocity.y > 0 && !jumpPressed)
//        {
//            rb.gravityScale = lowJumpMultiplier;
//        }
//        else
//        {
//            rb.gravityScale = normalGravity;
//        }        
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

        isGrounded = fails < groundCheckers.Length;
    }
    
    private void WallCheck()
    {
        int fails = 0;
        foreach (var checker in wallCheckers)
        {
            if (Physics2D.Linecast(transform.position, checker.transform.position,
                    1 << LayerMask.NameToLayer("Ground")) == false)
                fails++;
        }

        isWallNear = fails < wallCheckers.Length;
    }

    private void LateUpdate()
    {
        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool(Grounded, isGrounded);        
        animator.SetInteger("xVelocity", Convert.ToInt32(rb.velocity.x));
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
        isClimb = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(pauseTime);

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }


    public void Die()
    {
        healthController.Die();
    }
}
