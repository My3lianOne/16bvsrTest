
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
    [SerializeField] 
    private Transform fieldOfView;

    private NPCShooter shooter;

    private Rigidbody2D rb;

    private bool isIdle;

    [SerializeField]
    private float idleTime;

    private float idleTimer;
    
    


    [SerializeField] private Transform groundChecker;

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
        if (target)
        {
            shooter.Shoot();
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
        else
        {
            rb.velocity = Vector2.right * (transform.localScale.x > 0 ? 1 : -1 ) * moveSpd;
            if (!Physics2D.Linecast(transform.position, groundChecker.position, 1 << LayerMask.NameToLayer("Ground")))
            {
                isIdle = true;
                idleTimer = idleTime;
            }
    
        }
        
        
        /*else if (isPatrol)
        {
            if (!Physics2D.Linecast(transform.position, groundChecker.position, 1 << LayerMask.NameToLayer("Ground")))
                isPatrol = false;
            rb.velocity = Vector2.right * (transform.localScale.x > 0 ? 1 : -1 ) * moveSpd;
            patrolTimer -= Time.deltaTime;
            if (patrolTimer <= 0)
            {
                patrolTimer = 0;
                isPatrol = false;
            }
        }
        else
        {
            patrolTimer -= Time.deltaTime;
            if (patrolTimer >= patrolTime)
            {
                patrolTimer = patrolTime;
                isPatrol = true;
                Flip();
            }
            rb.velocity = Vector2.zero;
        }*/
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
