using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private GameController gameController;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (gameController)
            {
                gameController.LevelEnd();
            }
        }
    }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }
}
