using System;
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

    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject gameOverUI;
    

    private Animator anim;
    
    private Action action;
    
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
            gameController.GameEnded += OnGameEnded;
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

    private bool paused = false;
    private bool onLoading = false;
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (SceneManager.GetActiveScene().buildIndex != 0)
            {
                if (paused && !onLoading)
                {
                    SetUnpause();
                
                }
                else if(!onLoading )
                {
                    SetPause();
                }
            }

        }
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
        action = gameController.ReloadGame;
        anim.SetTrigger("FadeIn");
        anim.SetTrigger("FadeOut");
    }   

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            mainMenuUI.SetActive(true);
            gameUI.SetActive(false);
            pauseUI.SetActive(false);
            gameOverUI.SetActive(false);
        }
        else
        {
            mainMenuUI.SetActive(false);
            pauseUI.SetActive(false);
            gameOverUI.SetActive(false);
            gameUI.SetActive(true);
                        
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

        anim.SetTrigger("FadeOut");
        onLoading = false;
    }

    public void OnLevelSwitched()
    {
        action = gameController.SwitchLevel;
        anim.SetTrigger("SwitchLevel");
    }




    public void OnGameEnded()
    {
        gameOverUI.SetActive(true);  
        paused = true;
        Time.timeScale = 0;
    }
    
    #region AnimationEvents    
    void AnimatorAction()
    {
        action?.Invoke();
    }

    #endregion

    #region ButtonsMethods
    public void QuitGame()
    {
        gameController.QuitGame();
    }
    
    public void PlayGame()
    {
        action = gameController.LoadNextlevel;
        anim.SetTrigger("FadeIn");
        onLoading = true;
    }
    
    public void ReturnToMainMenu()
    {
        action = gameController.ReturnToMainMenu;
        SetUnpause();
        anim.SetTrigger("FadeIn");
        onLoading = true;
    }
    
    public void SetPause()
    {
        paused = true;
        Time.timeScale = 0;
        pauseUI.SetActive(true);
    }

    public void SetUnpause()
    {
        paused = false;
        Time.timeScale = 1f;
        pauseUI.SetActive(false);
    }

    public void ReloadGame()
    {
        action = gameController.ReloadScene;
        SetUnpause();
        anim.SetTrigger("FadeIn");
        onLoading = true;
    }
    #endregion
}
