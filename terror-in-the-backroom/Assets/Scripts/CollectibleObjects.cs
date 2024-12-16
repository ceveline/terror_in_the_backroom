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

        //if the item is a first aid kit, increase player heath
            if (item.itemName == "First Aid")
            {
            Debug.Log("first aid collected");

                //Get player's healthbar
                GameObject healthbarObject = GameObject.FindGameObjectWithTag("playerHealth");

                HealthBar healthbar = healthbarObject.GetComponent<HealthBar>();

                // Power up to increase player health by 25 points
                healthbar.health += 25;
            }

        gameObject.SetActive(false);
        //set isColliding bool back to false
        isCollidingWithObject = false;
    }

   

}
