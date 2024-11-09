using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleObjects : MonoBehaviour
{
    public Item item;
    bool isCollidingWithObject = false;

    void Update()
    {
        //If player is colliding with object and pressing the E key
        CheckForObjectCollecting();
    }

     public void OnTriggerEnter(Collider other)
        {

            if (other.gameObject.CompareTag("Player"))
            {
                isCollidingWithObject = true;
            }
        }

    void CheckForObjectCollecting()
    {
        //only collect item if a player is colliding with is and pressing the E key
        if (isCollidingWithObject && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player is pressing E");
            CollectObject();
        }
    }

    void CollectObject()
    {
        //Add to Inventory
        InventoryManager.Instance.Add(item);

        gameObject.SetActive(false);
        //set isColliding bool back to false
        isCollidingWithObject = false;
    }

   

}
