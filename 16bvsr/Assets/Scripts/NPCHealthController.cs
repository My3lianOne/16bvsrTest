using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCHealthController : MonoBehaviour
{
    [Tooltip("Максимальные HP")]
    [SerializeField]
    private int maxHealth;   
    
    [Tooltip("Текущие HP")]
    [SerializeField]
    private int currentHealth;

    [SerializeField] private GameObject gameController;
    // Временно.
     
    
    public enum ENEMY
    {
        Player,
        NPC
    }

    [SerializeField] private ENEMY whoIsEnemy;
    public bool IsDie { get; private set; }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        IsDie = false;
        gameController = GameObject.FindWithTag("GameController");        
    }

    // Update is called once per frame
    void Update()
    {
        // Костыль. Сделать связь с геймконтроллером.
        if (IsDie)
        {
            if (gameController)
            {
                
            }
            
            transform.parent.gameObject.SetActive(false);
        }    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            return;
        }
        else
        {
            Hurt();
        }
    }

    /// <summary>
    /// Уменьшает количество HP на 1.
    /// </summary>
    public void Hurt()
    {
        if (currentHealth > 0)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                IsDie = true;
            }
        }
    }
}
