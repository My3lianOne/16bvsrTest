using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private GameController gameController;
    private HealthController playerHealth;
    private LevelSwitcher levelSwitcher;

    [SerializeField]
    private Slider switchCooldown;
    [SerializeField]
    private Toggle [] healthPoints;
    [SerializeField]
    private Text livesCount;
    
    
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        gameController.LivesCountChanged += OnLivesCountUpdate;
        
        playerHealth = FindObjectOfType<HealthController>();
        playerHealth.HealthChanged += OnHealthUpdate;
        playerHealth.MaxHealthChanged += OnMaxHealthUpdate;
                
        levelSwitcher = FindObjectOfType<LevelSwitcher>();       
        switchCooldown.maxValue = levelSwitcher.SwitchCooldown;
        switchCooldown.value = 0;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switchCooldown.value = levelSwitcher.CooldownTimer;
    }
    
    void OnHealthUpdate(int count)
    {
        foreach (Toggle toggle in healthPoints)
        {
            toggle.isOn = false;
        }
        
        for (int i = 0; i < count; i++)
        {
            healthPoints[i].isOn = true;
        }
        
    }
    
    void OnMaxHealthUpdate(int count)
    {
        foreach (Toggle toggle in healthPoints)
        {
            toggle.gameObject.SetActive(false);
        }
        
        for (int i = 0; i < count; i++)
        {
            healthPoints[i].gameObject.SetActive(true);
        }
    }
    
    void OnLivesCountUpdate(int count)
    {
        livesCount.text = count.ToString();
    }
}
