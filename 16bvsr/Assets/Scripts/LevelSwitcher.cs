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

    [SerializeField] private GameObject fader;

    private Animation anim;

    private bool isSwitching;

    [SerializeField]
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        anim.clip.legacy = true;
        currentWorld = WORLDS.Real;        
        
        // Настройки слайдера
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWorld == WORLDS.Pixel)
        {
            if (cooldownTimer <= 0)
            {
                anim.Play();   
            }
            else
            {
                if (cooldownTimer > 0)
                {
                    cooldownTimer -= Time.deltaTime;
                }                    
            }
                           
            if(Input.GetButtonDown("Fire1"))
            {
                anim.Play();   
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
        
        if (Input.GetButtonDown("Fire1") && canSwitch && !isSwitching)
        {
            anim.Play(); 
        }

        slider.value = cooldownTimer;
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
    
    public void DisableFader()
    {
        fader.SetActive(false);
    }
}
