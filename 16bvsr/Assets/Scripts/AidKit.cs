using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AidKit : CollectableItem
{
    private HealthController player;
    
    [SerializeField]
    private int hp;
    // Start is called before the first frame update
    public override void OnPick()
    {
        player.Heal(hp);
        Deactivate();
    }

    private void OnEnable()
    {
        player = FindObjectOfType<HealthController>();
    }
    
    
}
