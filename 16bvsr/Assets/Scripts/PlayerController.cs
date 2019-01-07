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
    GameObject groundChecker;

    Rigidbody2D rb;

    public bool IsGrounded
    {
        get { return Physics2D.Linecast(transform.position, groundChecker.transform.position, 1 << LayerMask.NameToLayer("Ground")); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        Vector3 move = new Vector3(h * moveSpd * Time.deltaTime, 0, 0);

        transform.Translate(move);

        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
        }
    }
}
