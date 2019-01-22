using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn;

    [SerializeField] private float cooldown;

    private GameObject instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = Instantiate(objectToSpawn);
        instance.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        /*if (instance.activeSelf == false)
        {
            Invoke(nameof(Spawn), cooldown);
        }*/
    }

    void Spawn()
    {
        instance.SetActive(true);
        instance.transform.position = this.transform.position;
    }
    
    void DeSpawn()
    {
        instance.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
            if(instance.activeSelf == false)
                Spawn();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if(instance.activeSelf == true)
            DeSpawn();
    }
}
