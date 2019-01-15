using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleNPC : MonoBehaviour
{


    [SerializeField] private float moveSpd;

    private GameObject target;
    
    private Transform fieldOfView;  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        target = Physics2D.OverlapArea(transform.up, fieldOfView.position, 1 << LayerMask.NameToLayer("Player")).gameObject;
        if (target)
        {
            transform.LookAt(target);
        }
    }



}
