using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{

    private ItemsManager itemsManager;
    
    // Start is called before the first frame update
    void Start()
    {
        itemsManager = FindObjectOfType<ItemsManager>();
    }

    public void Drop()
    {
        GameObject item = itemsManager.GetItem();
        if (item)
        {
            item.SetActive(true);
            item.transform.position = this.transform.position;
        }
    }
}
