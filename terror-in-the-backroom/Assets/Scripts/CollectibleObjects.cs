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
    public AudioSource collectAudio;

    void Start()
    {
        GameObject healthTextObject = GameObject.Find("PowerUpText");
        healthText = healthTextObject.GetComponent<TextMeshProUGUI>();
        //collectAudio = collectAudioObject.GetComponent<AudioSource>();
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

        gameObject.SetActive(false);
        //set isColliding bool back to false
        isCollidingWithObject = false;
    }

    public void resetText()
    {
        healthText.text = "";
    }



}
