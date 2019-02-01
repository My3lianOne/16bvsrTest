using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
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
    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private GameObject gameUI;

    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        gameController = FindObjectOfType<GameController>();
        if (gameController)
        {
            gameController.LivesCountChanged += OnLivesCountUpdate;        
            gameController.PlayerDie += OnPlayerDie;
        }
        else
        {
            Debug.LogError("GameController not found");
        }
        
        
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(levelSwitcher)
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

    void OnPlayerDie()
    {
        anim.SetTrigger("PlayerDie");
    }
    
    
    public void PlayGame()
    {
        anim.SetTrigger("NextScene");
        //mainMenuUI.SetActive(true);
    }
    
    public void QuitGame()
    {
        gameController.QuitGame();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            mainMenuUI.SetActive(true);
            gameUI.SetActive(false);
        }
        else
        {
            mainMenuUI.SetActive(false);
            gameUI.SetActive(true);
            anim.SetTrigger("SceneLoaded");
        }
                        
        playerHealth = FindObjectOfType<HealthController>();
        if (playerHealth)
        {
            playerHealth.HealthChanged += OnHealthUpdate;
            playerHealth.MaxHealthChanged += OnMaxHealthUpdate;
        }
        else
        {
            Debug.LogError("Player health controller not found");
        }
        
        levelSwitcher = FindObjectOfType<LevelSwitcher>();
        if (levelSwitcher)
        {
            levelSwitcher.LevelSwitched += OnLevelSwitched;
            switchCooldown.maxValue = levelSwitcher.SwitchCooldown;
            switchCooldown.value = levelSwitcher.SwitchCooldown;
        }
        else
        {
            Debug.LogError("LevelSwitcher not found");
        }
    }

    public void OnLevelSwitched()
    {
        anim.SetTrigger("LevelSwitched");
    }


    public void SwitchLevel()
    {
        if (levelSwitcher)
        {
            levelSwitcher.Switch();
        }
    }
    
}
