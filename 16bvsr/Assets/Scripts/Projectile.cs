using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float lifetime;
    [SerializeField]
    private float moveSpd;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable ()
    {
        Invoke(nameof(Deactivate), lifetime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector2.right * transform.localScale.x * moveSpd;        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         gameObject.SetActive(false);
    }
    
    void Deactivate()
    {
         gameObject.SetActive(false);
    }
}