
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class SimpleNPC : MonoBehaviour
{    
    [SerializeField]
    private float moveSpd;
    [SerializeField]
    private GameObject target;
    private NPCShooter shooter;

    private Rigidbody2D rb;

    private bool isIdle;

    [SerializeField]
    private float idleTime;

    private float idleTimer;

    private bool isGroundInFront;

    private bool isWallInFront;
    
    


    [SerializeField] private Transform groundChecker;
    [SerializeField] private Transform wallChecker;

    private void Start()
    {
        shooter = GetComponent<NPCShooter>();
        rb = GetComponent<Rigidbody2D>();
        isIdle = false;
        idleTimer = idleTime;
    }

    // Update is called once per frame
    void Update()
    {


        
        if (groundChecker)
        {
            isGroundInFront = Physics2D.Linecast(transform.position, groundChecker.position,
                1 << LayerMask.NameToLayer("Ground"));
        }
        else
        {
            isGroundInFront = true;
        }
        if (wallChecker)
        {
            isWallInFront = Physics2D.Linecast(transform.position, wallChecker.position,
                1 << LayerMask.NameToLayer("Ground"));
        }
        else
        {
            isWallInFront = false;
        }                        
        
        if (target && shooter)
        {
            shooter.Shoot();
            if (target.activeSelf == false)
            {
                target = null;
                isIdle = false;
            }
        }
        else if (isIdle)
        {
            rb.velocity = Vector2.zero;
            idleTimer -= Time.deltaTime;
            if (idleTimer <= 0)
            {
                idleTimer = 0;
                isIdle = false;
                Flip();
            }
        }
        else if (isWallInFront)
        {
            Flip();
        }
        else
        {
            rb.velocity = Vector2.right * (transform.localScale.x > 0 ? 1 : -1 ) * moveSpd;
            if (!isGroundInFront )
            {
                isIdle = true;
                idleTimer = idleTime;
            }
        }
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = other.gameObject;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            target = null;
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
            transform.localScale = new Vector3(scale.x * -1, scale.y, scale.z);
    }
}
