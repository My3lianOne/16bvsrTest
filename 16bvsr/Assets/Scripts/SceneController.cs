using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private Animator anim;

    [SerializeField]
    private GameObject mainMenuUI;
    [SerializeField]
    private GameObject gameUI;

    private GameController gameController;
    
    public static SceneController instance;
    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        anim = GetComponent<Animator>();

        gameController = GetComponent<GameController>();

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

    public void PlayGame()
    {
        anim.SetTrigger("NextScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void LoadNextScene()
    {
        mainMenuUI.SetActive(false);
        gameUI.SetActive(true);
        gameController.enabled = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene(0);
        mainMenuUI.SetActive(true);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        anim.SetTrigger("GameStart");

    }
    
    public void OnSceneUnloaded(Scene scene)
    {
        
    }
}
