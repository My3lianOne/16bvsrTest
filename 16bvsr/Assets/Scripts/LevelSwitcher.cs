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
    
    
    [SerializeField] 
    private bool canSwitch;
    
    [SerializeField]
    private GameObject realLevel;
    [SerializeField]
    private GameObject pixelLevel;

    [SerializeField] private GameObject fader;

    private Animation fAnim;

    private bool isSwitching;

    [SerializeField]
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        fAnim = fader.GetComponent<Animation>();

        currentWorld = WORLDS.Real;        
        
        // Настройки слайдера
        slider.maxValue = switchCooldown;
        slider.value = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWorld == WORLDS.Pixel)
        {
            if (cooldownTimer <= 0)
            {
                Switch();
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
                Switch();
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
            Switch();       
        }

        slider.value = cooldownTimer;
    }

    public void Switch()
    {
        fAnim.Play();
        
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
