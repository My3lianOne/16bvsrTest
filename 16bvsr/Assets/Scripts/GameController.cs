using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Experimental.PlayerLoop;

public class GameController : MonoBehaviour
{
    private Action action;
    
    [SerializeField]
    private GameObject playerPrefab;

    private GameObject player;
    public GameObject Player
    {
        get
        {
            if (player)
            {                
                return player;
            }
            else
            {
                player = GameObject.FindWithTag("Player");
                if (player == null)
                {
                    player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                    player.SetActive(false);
                }
                return player;
            }
        }
    }
    private HealthController playerHealthController;
    
    [SerializeField]
    private LevelSwitcher levelSwitcher;
       
    public delegate void LivesCountEvents(int count);
    public delegate void GameEvents();
    public event LivesCountEvents LivesCountChanged; 
    public event GameEvents PlayerDie;
    public event GameEvents GameEnded;
    public event GameEvents LevelEnded;

    public static List<GameObject> gameObjects;
    
    /// <summary>
    /// Первый чекпоинт на сцене
    /// </summary>
    private GameObject firstCheckPoint;
    
    private GameObject currentCheckPoint;
    public GameObject CurrentCheckPoint
    {
        get => currentCheckPoint;
    }


    private GameObject levelEndPoint;
    
    /// <summary>
    /// Количество жизней
    /// </summary>
    [SerializeField] private int defaultLivesCount;

    private CinemachineVirtualCamera followCam;
    
    private int lives;
    public int LivesCount
    {
        get => lives;
        set
        {
            lives = value;
            LivesCountChanged?.Invoke(lives);
        }
    }

    public SceneController sceneController;
    
    
    public static GameController instance;
    void Awake()
    {
        // Реализуем одиночку.
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoad;

        sceneController = FindObjectOfType<SceneController>();
    }

    private void Initialize()
    {        
        InitScene();
        LivesCount = defaultLivesCount;  
        levelEndPoint = GameObject.FindWithTag("EndPoint");
        gameObjects = new List<GameObject>();

    }

    void OnPlayerDie()
    {
        if (LivesCount > 1)
        {
            LivesCount--;
            
            // resurrect player
            // restore all objects on scene
            PlayerDie?.Invoke();
        }
        else
        {            
            GameEnded?.Invoke();            
        }
        
        
    }

    public void IncreaseLivesCount()
    {
        LivesCount++;
    }


    public void SetCheckPoint(GameObject point)
    {
        currentCheckPoint = point;
    }


    private void ResurrectPlayer()
    {
        player.SetActive(true);
        player.transform.position = currentCheckPoint.transform.position;
    }


    void OnSceneLoad(Scene scene, LoadSceneMode loadSceneMode)
    {
        if(scene.buildIndex != 0)
            Initialize();   
    }

    void DespawnObjects()
    {
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("NPC"));
        gameObjects.AddRange(GameObject.FindGameObjectsWithTag("CollectableItem"));
        foreach (GameObject obj in gameObjects)
        {
            if(obj.activeSelf) obj.SetActive(false);
        }
    }

    public void ReloadGame()
    {
        DespawnObjects();
        ResurrectPlayer();
    }

    void OnGameEnd()
    {
        
    }

    private void InitCamera()
    {
        followCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (player)
        {
            followCam.Follow = player.transform;
        }
        else
        {
            Debug.LogError("Player was not founded by camera");
        }
        
    }

    private void InitCheckPoints()
    {
        firstCheckPoint = GameObject.FindWithTag("FirstCheckPoint");
        currentCheckPoint = firstCheckPoint;
    }

    private void InitPlayer()
    {
        Player.SetActive(true);
        Player.transform.position = currentCheckPoint.transform.position;
        playerHealthController = Player.GetComponentInChildren<HealthController>();
        playerHealthController.PlayerDieEvent += OnPlayerDie;
        //playerHealthController.Health = установить хп в зависимости от уровня сложности. 
    }
    
    private void InitLevelSwitcher()
    {
        levelSwitcher = FindObjectOfType<LevelSwitcher>();
    }

    private void InitScene()
    {
        InitCheckPoints();
        InitPlayer();
        InitCamera();
        InitLevelSwitcher();
    }


    public void ReturnToMainMenu()
    {
        sceneController.LoadMainMenu();
    }

    public void LoadNextlevel()
     {
         sceneController.LoadNextScene();
     }

    public void LoadLevel(int index)
    {
        sceneController.LoadScene(index);
    }

    public void QuitGame()
    {
        sceneController.Quit();
    }

    public void ReloadScene()
    {
        sceneController.ReloadScene();
    }

    public void SwitchLevel()
    {
        if (levelSwitcher)
        {
            levelSwitcher.Switch();
        }
    }

    public void LevelEnd()
    {
        LevelEnded?.Invoke();
    }
}
