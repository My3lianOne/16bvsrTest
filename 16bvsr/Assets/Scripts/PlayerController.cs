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

    float climbDrag = 50;

    [SerializeField]
    bool isGrounded = true;

    [SerializeField]
    bool canJump = true;

    [SerializeField]
    float v;
    [SerializeField]
    float h;



    /*
    public bool IsGrounded
    {
        get { return Physics2D.Linecast(transform.position, groundChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")); }
    }
    */

    public bool IsClimb {
        get { return Physics2D.Linecast(transform.position, wallChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")) && !isGrounded && Input.GetAxisRaw("Horizontal") != 0; }
    }
    

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {
            rb.drag = 10;
            isGrounded = true;
            canJump = true;

        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {
            rb.drag = 0;
            isGrounded = false;
        }
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
        v = Input.GetAxis("Jump");

    }
    void FixedUpdate()
    {
        

        if (v >= 1) canJump = false;
        
        h = Input.GetAxisRaw("Horizontal");
        Flip(h);

        rb.AddForce(direction * moveSpd);

        if (Input.GetButton("Jump") && IsClimb) 
        {
            //rb.velocity = new Vector2 (-transform.localScale.x * bounceForce, bounceForce);
            rb.AddForce(new Vector2(-transform.localScale.x, 0) * bounceForce);
            Flip(-transform.localScale.x);
        }

        if (Mathf.Abs(rb.velocity.x) > moveSpd/100f)
		{
			rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * moveSpd/100f, rb.velocity.y);
		}

        if (v > 0 && v < 1 && canJump)
        {
            //rb.AddForce(Vector2.up * (jumpForce - 1) * (1 - v), ForceMode2D.Impulse);
            rb.velocity = new Vector2(0, jumpForce);
        }



        if (Input.GetButtonUp("Jump"))
        {
            canJump = false;
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
