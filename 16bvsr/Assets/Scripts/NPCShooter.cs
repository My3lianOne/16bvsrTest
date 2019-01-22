using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCShooter : MonoBehaviour
{
    [SerializeField] private int poolCapacity;

    [SerializeField] private Transform shootingPlace;
    
    List<GameObject> pool = new List<GameObject>();
 

    [SerializeField] private GameObject projectile;

    [SerializeField] private float cooldown;
    private bool canShoot;
    private Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        canShoot = true;
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

    }

    public void Shoot()
    {
        if (canShoot)
        {
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
                proj.transform.localScale = transform.localScale;

                StartCoroutine(Recharge());
                return proj;
            }
        }
        return null;
    }


    private IEnumerator Recharge()
    {
        canShoot = false;

        yield return new WaitForSeconds(cooldown);
        
        canShoot = true;
    }

    private void OnEnable()
    {
        canShoot = true;
    }
}
