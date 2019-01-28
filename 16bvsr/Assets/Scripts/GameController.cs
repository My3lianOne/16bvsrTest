﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    
    [SerializeField]
    private GameObject player;
    
    private HealthController playerHealthController;
    
    [SerializeField]
    private LevelSwitcher levelSwitcher;

    private Scene scenes;

    private Scene currentScene;
    
    public static GameController instance;

    public delegate void LivesCountEvents(int count);

    public event LivesCountEvents LivesCountChanged;

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
    }


    private void Start()
    {
        LivesCount = defaultLivesCount;
        
        
        playerHealthController = player.GetComponentInChildren<HealthController>();
        playerHealthController.PlayerDieEvent += OnPlayerDie;
        SceneManager.sceneLoaded += OnSceneLoad;
        firstCheckPoint = GameObject.FindWithTag("FirstCheckPoint");
        currentCheckPoint = firstCheckPoint;
        player.GetComponent<Rigidbody2D>().MovePosition(currentCheckPoint.transform.position);

        gameObjects = new List<GameObject>();

        anim = GetComponent<Animation>();
    }


    void Update()
    {
        
    }


    void OnPlayerDie()
    {
        if (lives > 0)
        {
            lives--;
            
            // resurrect player
            // restore all objects on scene
            Invoke(nameof(OnGameEnd), 3);
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
        currentCheckPoint = firstCheckPoint;
        player.GetComponent<Rigidbody2D>().MovePosition(currentCheckPoint.transform.position);   
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
    
}