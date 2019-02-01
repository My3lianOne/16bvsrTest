using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LevelSwitcher : MonoBehaviour
{
    public enum WORLDS
    {
        Real = 0,
        Pixel = 1
    }

    [FormerlySerializedAs("currentLevel")] public WORLDS currentWorld;

    [SerializeField] private float switchTime;

    [SerializeField] 
    private float switchCooldown;
    [SerializeField] 
    private float cooldownTimer;

    public float CooldownTimer => cooldownTimer;

    public float SwitchCooldown => switchCooldown;

    [SerializeField] 
    private bool canSwitch;
    
    [SerializeField]
    private GameObject realLevel;
    [SerializeField]
    private GameObject pixelLevel;   

    private Animation anim;

    private bool isSwitching;

    public delegate void LevelSwitchEvents();

    public event LevelSwitchEvents LevelSwitched;
    // Start is called before the first frame update
    void Start()
    {
        currentWorld = WORLDS.Real;
        cooldownTimer = switchCooldown;
        // Настройки слайдера

    }

    // Update is called once per frame
    void Update()
    {
        if (currentWorld == WORLDS.Pixel)
        {
            cooldownTimer = cooldownTimer < 0 ? 0 : cooldownTimer;
            if (cooldownTimer <= 0)
            {
                if (!isSwitching)
                {
                    LevelSwitched?.Invoke();
                    isSwitching = true;
                }               
                    
            }
            else
            {
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
                }                    
            }
                           
            if(Input.GetButtonDown("Switch"))
            {
                if (!isSwitching)
                {
                    LevelSwitched?.Invoke(); 
                    isSwitching = true;
                } 
                    
            }
        }

        if (currentWorld == WORLDS.Real)
        {           
            if (cooldownTimer >= switchCooldown)
            {
                canSwitch = true;
            }
            else
            {
                if(cooldownTimer < switchCooldown)
                    cooldownTimer += Time.deltaTime;
            }
        }
        
        if (Input.GetButtonDown("Switch") && canSwitch && !isSwitching)
        {
                LevelSwitched?.Invoke(); 
        }
    }

    public void Switch()
    {                
        switch (currentWorld)
        {
            // Switch to PixelWorld
            case WORLDS.Real:
                realLevel.SetActive(false);
                pixelLevel.SetActive(true);
                currentWorld = WORLDS.Pixel;
                canSwitch = true;
                break;

            // Switch to RealWorld
            case WORLDS.Pixel:
                realLevel.SetActive(true);
                pixelLevel.SetActive(!pixelLevel.activeSelf);
                currentWorld = WORLDS.Real;
                canSwitch = false;
                cooldownTimer = 0;
                break;
        }

        isSwitching = false;
    }    
}
