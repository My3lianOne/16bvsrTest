using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    [SerializeField] private GameObject target;

    private Vector3 cameraPos;
    private Vector3 targetPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraPos = transform.position;
        targetPos = target.transform.position;
        
        Vector2 pos = new Vector2(targetPos.x, cameraPos.y);
        

        if (target.CompareTag("Player"))
        {     
            if (target.GetComponent<MoveScript>().IsGrounded)
            pos.y = targetPos.y;
        }

        transform.position = pos;
    }
}
