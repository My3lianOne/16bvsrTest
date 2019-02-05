using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float lifetime;
    [SerializeField]
    private float moveSpd;
    private Vector3 m_Velocity = Vector3.zero;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;
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
    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector2(transform.localScale.x * 10f * Time.fixedDeltaTime * moveSpd, rb.velocity.y);

        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);     
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