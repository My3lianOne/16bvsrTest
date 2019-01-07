using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float moveSpd;

    [SerializeField]
    float jumpForce;

    [SerializeField]
    float bounceForce;

    [SerializeField]
    GameObject groundChecker;
    [SerializeField]
    GameObject wallChecker;

    Rigidbody2D rb;

    private Vector2 direction;


    float signDirection;
    float normalDrag = 2;

    float climbDrag = 10;

    public bool IsGrounded
    {
        get { return Physics2D.Linecast(transform.position, groundChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")); }
    }

    public bool IsClimb {
        get { return Physics2D.Linecast(transform.position, wallChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")) && !IsGrounded && Input.GetAxisRaw("Horizontal") != 0; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void Start()
    {
        
    }


    void Update(){
        direction = new Vector2(Input.GetAxisRaw("Horizontal"),0);
    }
    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        Flip(h);

            rb.AddForce( direction * moveSpd * Time.deltaTime, ForceMode2D.Impulse);

        if(Mathf.Abs(rb.velocity.x) > moveSpd/100f)
		{
			rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpd/100f, rb.velocity.y);
		}

        if(Input.GetButtonDown("Jump") && IsGrounded)
        {
            rb.velocity = new Vector2 (0, jumpForce);
            //rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        if(Input.GetButtonDown("Jump") && IsClimb)
        {
            //rb.velocity = new Vector2 (-transform.localScale.x * bounceForce, bounceForce/);
             rb.AddForce(new Vector2 (-transform.localScale.x, 1) * bounceForce, ForceMode2D.Impulse);
            Flip(-transform.localScale.x);
        }
    
        if(IsClimb) {
            rb.drag = climbDrag;
        }
        else {
            rb.drag = normalDrag;
        }
    
    }


    private void Flip(float h) {
        if (h != 0)
            transform.localScale = new Vector3(h, transform.localScale.y, transform.localScale.z);
    }
}
