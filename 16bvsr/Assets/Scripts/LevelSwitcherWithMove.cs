using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSwitcherWithMove : MonoBehaviour
{
    [SerializeField] private float switchTime;
    [SerializeField]
    private GameObject normalLevel;
    [SerializeField]
    private GameObject pixelLevel;
    [SerializeField]
    private GameObject player;
    
    [SerializeField] private GameObject fader;
    [SerializeField] private GameObject camera;
    [SerializeField] private GameObject worldBound;
    private Animation fAnim;

    private bool isSwitching;
    
    private GameObject currLevel;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        fAnim = fader.GetComponent<Animation>();
        currLevel = normalLevel;
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
        isSwitching = false;
        
        if (currLevel == normalLevel)
        {
            player.transform.Translate(new Vector3 (0,-20));
            worldBound.transform.Translate(new Vector3 (0,-20));
            currLevel = pixelLevel;
            return;
        }

        if (currLevel == pixelLevel)
        {
            worldBound.transform.Translate(new Vector3 (0,20));
            player.transform.Translate(new Vector3 (0,20));
            currLevel = normalLevel;
            return;
        }
        camera.transform.Translate(player.transform.position);   
    }
    
    public void DisableFader()
    {
        fader.SetActive(false);
    }        
}
