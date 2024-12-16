using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CollectibleObjects : MonoBehaviour
{
  // public GameObject collectAudioObject;
    public Item item;
    bool isCollidingWithObject = false;
    TextMeshProUGUI healthText;
    TextMeshProUGUI itemText;
    public AudioSource collectAudio;

    void Start()
    {
        //get health text
        GameObject healthTextObject = GameObject.Find("PowerUpText");
        healthText = healthTextObject.GetComponent<TextMeshProUGUI>();
        //collectAudio = collectAudioObject.GetComponent<AudioSource>();

        GameObject itemTextObject = GameObject.Find("StolenItemText");
        itemText = itemTextObject.GetComponent<TextMeshProUGUI>();
    }

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
       // collectAudio.Play();

        //if the item is a first aid kit, increase player heath
            if (item.itemName == "First Aid")
            {
                //Get player's healthbar
                GameObject healthbarObject = GameObject.FindGameObjectWithTag("playerHealth");

                HealthBar healthbar = healthbarObject.GetComponent<HealthBar>();

                // Power up to increase player health by 25 points
                healthbar.health += 25;

            //Alert user that health has been increased
            healthText.text = "+25 HP";

            //reset text after 5 seconds
            Invoke("resetText", 5f);

        }

        //destroy game object because setting it as false would allow it to be collected multiple times
        //Destroy(this);
        gameObject.SetActive(false);

        //set isColliding bool back to false
        isCollidingWithObject = false;

        if(GameManager.Instance.itemsCollected >= GameManager.Instance.itemsToCollect)
        {
            itemText.text = "Proceed to the entrance to drop off your items.";
        }
    }

    public void resetText()
    {
        healthText.text = "";
    }



}
