using System;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    public delegate void PlayerDie();

    public event PlayerDie PlayerDieEvent;
    
    
    
    [Tooltip("Максимальные HP")]
    [SerializeField]
    private int maxHealth;   
    
    [Tooltip("Текущие HP")]
    [SerializeField]
    private int currentHealth;

    [SerializeField] private int lives;

    /// <summary>
    /// Неуязвимый?
    /// </summary>
    private bool isInvulnerable;

    [Tooltip("Неуязвимость после повреждения")]
    [SerializeField]
    private bool invulnerableAfterHurt;
    
    [Tooltip("Время неуязвимости")]
    [SerializeField]
    private float invulnerabilityTime;

    [SerializeField] private GameObject gameController;
    // Временно.
    [SerializeField]
    private GameObject haloOfInvulnerability;        
    
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

    private void OnTriggerEnter2D(Collider2D other)
    {
            if(other.CompareTag("Enemy")) 
                Hurt();
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Bound"))
            Die();
    }

    /// <summary>
    /// Уменьшает количество HP на 1.
    /// </summary>
    public void Hurt()
    {
        if (currentHealth > 0 && !isInvulnerable)
        {
            currentHealth--;
            if (currentHealth <= 0)
            {
                Die();
            }
        }

        if (invulnerableAfterHurt)
        {            
            StartCoroutine(nameof(SetInvulnerable), invulnerabilityTime);
        }
    }

    /// <summary>
    /// Лечит на указанное количество поинтов.
    /// </summary>
    /// <param name="value">Количество поинтов</param>
    public void Heal(int value)
    {
        currentHealth += value;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    /// <summary>
    /// Увеливает максимальное количество HP на указанное количество поинтов.
    /// </summary>
    /// <param name="value">Количество поинтов</param>
    public void IncreaseMaxHealth(int value) => maxHealth += value;

    /// <summary>
    /// Делает неуязвимым на указанное время.
    /// </summary>
    private IEnumerator SetInvulnerable(float time)
    {
        isInvulnerable = true;

        if (haloOfInvulnerability) haloOfInvulnerability.SetActive(true);
        
        
        yield return new WaitForSeconds(time);

        isInvulnerable = false;

        if (haloOfInvulnerability) haloOfInvulnerability.SetActive(false);
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        IsDie = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        haloOfInvulnerability.SetActive(false);        
    }


    private void Die()
    {
        IsDie = true; 
        PlayerDieEvent?.Invoke();    
        transform.parent.gameObject.SetActive(false);
    }
}    
