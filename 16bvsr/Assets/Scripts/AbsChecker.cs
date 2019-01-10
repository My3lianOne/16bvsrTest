using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsChecker : MonoBehaviour
{
    private Collider2D col;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground"))
        {
            player.GetComponent<Rigidbody2D>().MovePosition(player.transform.position + Vector3.down * 5);
        }
    }
}
