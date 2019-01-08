using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
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
    [SerializeField]
    float normalDrag = 2;
    [SerializeField]
    float climbDrag = 50;

    [SerializeField]
    float normalJumpForce = 2;
    [SerializeField]
    float climbJumpForce = 50;


    [SerializeField]
    bool isGrounded = true;

    [SerializeField]
    bool canJump = true;

    [SerializeField]
    float v;
    [SerializeField]
    float h;

    public bool IsClimb
    {
        get { return Physics2D.Linecast(transform.position, wallChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")) && !isGrounded && Input.GetAxisRaw("Horizontal") != 0; }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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
                direction = new Vector2(jumpForce * -transform.localScale.x, jumpForce / 4);
            else
                direction = new Vector2(0, jumpForce * (1 - v));
            rb.AddForce(direction, ForceMode2D.Impulse);
        }

        /*if (Input.GetButton("Jump") && canJump && IsClimb)
        {
            
            rb.AddForce(new Vector2(jumpForce * -transform.localScale.x, 0), ForceMode2D.Impulse);
            rb.drag = 0;
            canJump = false;
        }
        */



    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {
            rb.drag = 10;
            rb.gravityScale = 9;
            isGrounded = true;
            canJump = true;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.transform.tag == "Ground")
        {
            rb.drag = 0;
            rb.gravityScale = rb.gravityScale + 5f;
            isGrounded = false;
        }
    }

    private void Flip(float h)
    {
        if (h != 0)
            transform.localScale = new Vector3(h, transform.localScale.y, transform.localScale.z);
    }

}
