using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float moveSpd;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke(nameof(Deactivate), 1);
    }

    void Start ()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.right * transform.localScale.x * moveSpd;        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
    
    void Deactivate()
    {
        gameObject.SetActive(false);
    }        
}