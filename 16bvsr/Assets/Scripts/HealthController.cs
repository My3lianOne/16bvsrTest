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

    public int MaxHealth
    {
        get => maxHealth;
        set
        {
            maxHealth = value;
            MaxHealthChanged?.Invoke(MaxHealth);
        }
    }
    
    [Tooltip("Текущие HP")]
    [SerializeField]
    private int currentHealth;
    

    [SerializeField]
    private int Health
    {
        get => currentHealth;
        set
        {
            currentHealth = value;
            HealthChanged?.Invoke(currentHealth);
        }
    }  
    
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


    public delegate void HealthState(int count);

    public event HealthState HealthChanged;
    public event HealthState MaxHealthChanged;

    private Rigidbody2D rb;
    private Animator anim;
    private static readonly int Hurt1 = Animator.StringToHash("Hurt");

    [Range(0, 10f)]
    [SerializeField]
    private float punchForce = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        Health = MaxHealth;
        IsDie = false;
        rb = GetComponentInParent<Rigidbody2D>();
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
        if (Health > 0 && !isInvulnerable)
        {
            Health--;
            anim.SetTrigger(Hurt1);
            rb.AddForce(new Vector2(-transform.localScale.x, 1) * punchForce, ForceMode2D.Impulse);
            
            if (Health <= 0)
            {
                Die();
                return;
            }
            if (invulnerableAfterHurt)
            {            
                StartCoroutine(nameof(SetInvulnerable), invulnerabilityTime);
            }
        }


    }

    /// <summary>
    /// Лечит на указанное количество поинтов.
    /// </summary>
    /// <param name="value">Количество поинтов</param>
    public void Heal(int value)
    {
        Health += value;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    /// <summary>
    /// Увеливает максимальное количество HP на указанное количество поинтов.
    /// </summary>
    /// <param name="value">Количество поинтов</param>
    public void IncreaseMaxHealth(int value)
    {
        MaxHealth += value;
    } 

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
        Health = MaxHealth;
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
