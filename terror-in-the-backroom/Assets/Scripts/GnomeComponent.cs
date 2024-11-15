using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeComponent : MonoBehaviour
{

    public HealthBar healthbar;
    Vector3 newObjectPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            //decrease player health by 25 on collsion with gnomes
            healthbar.takeDamage(25);

            //position of where the item will be relocated
            Vector3 newObjectPosition =  new Vector3(other.transform.position.x + Random.Range(-10f, 10f), 1.5f, other.transform.position.z + Random.Range(-10f, 10f));

            StealAndRepositionItem(newObjectPosition);
        }
    }

    public void StealAndRepositionItem(Vector3 newPosition)
    {
        if (InventoryManager.Instance.Items.Count > 0)
        {
            //get first item in inventory
            Item itemToSteal = InventoryManager.Instance.Items[0];

            //Remove that item from inventory
            InventoryManager.Instance.Remove(itemToSteal);

            //get type of object to instantaite by matching it to scriiptable item
           GameObject item =  FindGameObjectWithItem(itemToSteal);

            //instantiate item in the new position 
             GameObject newItem = Instantiate(item, newPosition, Quaternion.identity);

            //set status to Active
            newItem.SetActive(true);
        }
    }

    public GameObject FindGameObjectWithItem(Item itemToFind)
    {
        //get all instances of this script even if the objects are inactive
        CollectibleObjects[] allScripts = Resources.FindObjectsOfTypeAll<CollectibleObjects>();

        //for each script check the associated item and if it matches the item we're trying to find we return it
        foreach (CollectibleObjects script in allScripts)
        {
            if (script.item == itemToFind && script.gameObject.CompareTag("Collectible"))
            {
                return script.gameObject; 
            }
        }
        return null;
    }

}
