using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalMaxHp : CollectableItem
{
    private HealthController player;

    public override void OnPick()
    {
        FindObjectOfType<HealthController>().IncreaseMaxHealth(1);
        Deactivate();
    }

    private void OnEnable()
    {
        
    }
}
