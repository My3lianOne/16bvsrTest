using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private GameController gameController;
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameController.CurrentCheckPoint != this.gameObject)
                ActivateCheckPoint(); 
        }
        
    }

    public void ActivateCheckPoint()
    {
        gameController.SetCheckPoint(this.gameObject);
    }
}
