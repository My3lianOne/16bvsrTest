using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private int poolCapacity;

    [SerializeField] private Transform shootingPlace;
    
    List<GameObject> pool = new List<GameObject>();

    [SerializeField] private float shootForce; 

    [SerializeField] private GameObject projectile;
    // Start is called before the first frame update
    void Awake()
    {
        for (int i = 0; i < poolCapacity; i++)
        {
            GameObject obj = GameObject.Instantiate(projectile);
            obj.SetActive(false);
            pool.Add(obj);
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            SpawnProjectile();
        }
    }
    
    

    GameObject SpawnProjectile()
    {
        foreach (var proj in pool)
        {
            if (proj.activeSelf == false)
            {   
                proj.SetActive(true);  
                proj.transform.position = shootingPlace.positiond;
                proj.transform.localScale = transform.localScale;
                
                
                return proj;
            }
        }

        return null;
    }


    
    
    
}
