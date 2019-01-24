using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{

    private ItemsManager itemsManager;
    
    // Start is called before the first frame update
    void Awake()
    {
        itemsManager = FindObjectOfType<ItemsManager>();
    }

    public void Drop()
    {
        GameObject item = itemsManager.GetItem();
        if (item)
        {
            item.SetActive(true);
            item.GetComponent<CollectableItem>().LifeTimeOver();
            item.transform.position = this.transform.position;
        }
    }
}
