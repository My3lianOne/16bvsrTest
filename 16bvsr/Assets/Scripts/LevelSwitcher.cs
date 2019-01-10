using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcher : MonoBehaviour
{
    [SerializeField] private float switchTime;
    [SerializeField]
    private GameObject normalLevel;
    [SerializeField]
    private GameObject pixelLevel;

    [SerializeField] private GameObject fader;

    private Animation fAnim;

    private bool isSwitching;
    
    // Start is called before the first frame update
    void Start()
    {
        fAnim = fader.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !isSwitching)
        {
            fAnim.Play();
            isSwitching = true;
            Invoke("Switch", switchTime);
            
        }           
    }

    public void Switch()
    {        
        normalLevel.SetActive(!normalLevel.activeSelf);
        pixelLevel.SetActive(!pixelLevel.activeSelf);
        isSwitching = false;
    }
    
    public void DisableFader()
    {
        fader.SetActive(false);
    }
    
    
}
