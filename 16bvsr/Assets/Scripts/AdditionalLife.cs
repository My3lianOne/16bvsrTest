using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalLife : CollectableItem
{
    private GameController gameController;
        
    // Start is called before the first frame update
    public override void OnPick()
    {
        gameController.IncreaseLivesCount();
        Deactivate();
    }

    private void OnEnable()
    {
        gameController = FindObjectOfType<GameController>();
    }
}
