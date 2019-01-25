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

    private Animator anim;

    private MoveScript moveScript;
    // Start is called before the first frame update
    void Awake()
    {

        anim = GetComponent<Animator>();
        
        for (int i = 0; i < poolCapacity; i++)
        {
            GameObject obj = GameObject.Instantiate(projectile);
            obj.SetActive(false);
            pool.Add(obj);
        }

        moveScript = GetComponent<MoveScript>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            anim.SetTrigger("Shoot");
            SpawnProjectile();
        }
    }
    
    

    public GameObject SpawnProjectile()
    {
        foreach (var proj in pool)
        {
            if (proj.activeSelf == false)
            {   
                proj.SetActive(true);  
                proj.transform.position = shootingPlace.position;
                if (moveScript.IsClimbed)
                {
                    proj.transform.localScale = new Vector3(transform.localScale.x * -1, 1,1);
                }
                else
                {
                    proj.transform.localScale = transform.localScale;
                }
                                
                return proj;
            }
        }
        return null;
    }  
}
