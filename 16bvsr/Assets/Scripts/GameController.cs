using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameController : MonoBehaviour
{
    
    [SerializeField]
    private GameObject playerPrefab;

    private GameObject player;


    public GameObject Player
    {
        get
        {
            if (player )
            {                
                return player;
            }
            else
            {    
                player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
                player.SetActive(false);
                return player;
            }
        }        
    }
    
    private HealthController playerHealthController;
    
    [SerializeField]
    private LevelSwitcher levelSwitcher;

    private Scene scenes;

    private Scene currentScene;
    
    public static GameController instance;

    public delegate void LivesCountEvents(int count);
    public delegate void GameEvents();
    public event LivesCountEvents LivesCountChanged; 
    public event GameEvents PlayerDie;

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

    private Animation anim;
    
    private int lives;
    
    /// <summary>
    /// Количество жизней
    /// </summary>
    [SerializeField] private int defaultLivesCount;

    CinemachineVirtualCamera followCam;
    
    
    public int LivesCount
    {
        get => lives;
        set
        {
            lives = value;
            LivesCountChanged?.Invoke(lives);
        }
    }
    

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
    }

    private void Initialize()
    {
        
        currentCheckPoint = firstCheckPoint;
        player = Instantiate(playerPrefab, currentCheckPoint.transform.position, Quaternion.identity);
        followCam = FindObjectOfType<CinemachineVirtualCamera>();
        followCam.Follow = player.transform;
        
        LivesCount = defaultLivesCount;

        SceneManager.sceneLoaded += OnSceneLoad;       

        gameObjects = new List<GameObject>();

        anim = GetComponent<Animation>();
    }

    void OnPlayerDie()
    {
        if (lives > 0)
        {
            lives--;
            
            // resurrect player
            // restore all objects on scene
            Invoke(nameof(OnGameEnd), 3);
            PlayerDie?.Invoke();
        }
        else
        {
            // GameOver
            LivesCount = defaultLivesCount;
            currentCheckPoint = firstCheckPoint;
            Invoke(nameof(OnGameEnd), 3);
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

    void ReloadGame()
    {
        DespawnObjects();
        ResurrectPlayer();
    }

    void OnGameEnd()
    {
        anim.Play("OnGameEndFader");
    }


    private void OnEnable()
    {
        //Initialize();
    }

    private void GameStart()
    {
        InitializeCheckPoints();
        InitializeCamera();
        InitializePlayer();
        InitializeScene();
    }

    private void InitializeCamera()
    {
        followCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (player)
        {
            followCam.Follow = player.transform;
        }
        else
        {
            Debug.LogError("Player not found by camera");
        }
        
    }

    private void InitializeCheckPoints()
    {
        firstCheckPoint = GameObject.FindWithTag("FirstCheckPoint");
        currentCheckPoint = firstCheckPoint;
    }

    private void InitializePlayer()
    {
        playerHealthController = Player.GetComponentInChildren<HealthController>();
        playerHealthController.PlayerDieEvent += OnPlayerDie;
        //playerHealthController.Health = установить хп в зависимости от уровня сложности. 
    }

    private void InitializeScene()
    {
        
    }
}
