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

    private ItemDropper itemDropper;

    private bool isOutOfBuffer;
    
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
        itemDropper = GetComponentInParent<ItemDropper>();
    }

    private void OnDisable()
    {
        currentHealth = maxHealth;
        IsDie = false;
    }

    // Update is called once per frame
    void Update()
    { 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

        }
        else if (other.CompareTag("GameBuffer"))
        {
            isOutOfBuffer = false;
        }
        else
        {
            Hurt();
        }
    }
    
    
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GameBuffer"))
            isOutOfBuffer = true;
        Invoke(nameof(Deactivate), 10);            
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
                Die();
            }
        }
    }

    public void Die()
    {
        itemDropper.Drop();
        transform.parent.gameObject.SetActive(false);
    }

    void Deactivate()
    {
        if(isOutOfBuffer)
            transform.parent.gameObject.SetActive(false);
    }
}
