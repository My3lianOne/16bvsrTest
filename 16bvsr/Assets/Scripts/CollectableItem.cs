using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableItem : MonoBehaviour, ICollectable
{
    // Start is called before the first frame update

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnPick();
        }
    }

    public virtual void OnPick()
    {
        Debug.Log("Picked");
    }
    
    public virtual void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
