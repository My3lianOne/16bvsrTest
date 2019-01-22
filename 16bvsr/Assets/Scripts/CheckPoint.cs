using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeSelf == false)
        {
            player.SetActive(true);
            player.GetComponent<Rigidbody2D>().MovePosition(this.transform.position);
            
        }
    }
}
