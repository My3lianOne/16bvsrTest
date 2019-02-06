using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    
    public delegate void LevelEvents();

    private MoveScript player;
    public event LevelEvents LevelEnd; 
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<MoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    

    void SpawnBoss()
    {
        
    }

    void PreEndAction()
    {
        player.GetComponent<Animator>();
    }



}
