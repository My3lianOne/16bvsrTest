using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> poolOfAids;
    [SerializeField]
    private List<GameObject> poolOfLives;
    
    [SerializeField] private int amountOfAids;
    [SerializeField] private int amountOfLives;
    
    [SerializeField]
    private AidKit aids;       
    
    [SerializeField]
    private AdditionalLife lives;

    public static ItemsManager instance;

    private Random rnd = new Random();
    
    private void Awake()
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

    void Start()
    {                

    }

    public GameObject GetItem()
    {
        int s = Random.Range(0, 10);

        switch (s)
        {
            case 0 :
                return poolOfLives.Find(o => o.activeSelf != true);
            
            case 2: 
            case 5:
            case 8:
                return poolOfAids.Find(o => o.activeSelf != true);
            default:
                return null;                
        }
        
    }

    public void FillPull()
    {
        poolOfAids = new List<GameObject>();
        
        for (int i = 0; i < amountOfAids; i++)
        {
            GameObject obj = Instantiate(aids.gameObject);
            
            obj.SetActive(false);
            poolOfAids.Add(obj);                         
        }
        
        for (int i = 0; i < amountOfLives; i++)
        {
            GameObject obj = Instantiate(lives.gameObject);          
            obj.SetActive(false);
            poolOfLives.Add(obj);                         
        }  
    }
    
}
