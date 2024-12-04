using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class InventoryManager : MonoBehaviour   
{
    public static InventoryManager Instance; 
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    public TextMeshProUGUI itemsCollectedText;

    string sceneName = "";
    int itemsToCollect = 0;

    void Start()
    {
        sceneName = GameManager.Instance.getLevel();
        itemsToCollect = GameManager.Instance.setLevelItems();
    }

    private void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        //update the items text
        itemsCollectedText.text = "Items: " + GameManager.Instance.itemsCollected.ToString() + " / " + itemsToCollect.ToString();
    }

    public void Add(Item item)
    {
        Items.Add(item);

        //Update the number of items that have been collected
        GameManager.Instance.UpdateItemsCollected();

        //update the itemsCollected text
        itemsCollectedText.text = "Items: " + GameManager.Instance.itemsCollected.ToString() + " / " + itemsToCollect.ToString();
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        //prevent items from being duplicated each time the inventory is opened
        //destroy all items currently in the inventory because we reinstatiate all the items in the list anyways
        foreach(Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach(var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);

            // get Item Name
           var itemName = obj.transform.Find("ItemName").GetComponent<TMPro.TextMeshProUGUI>();

            //get Item image
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<UnityEngine.UI.Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;
        }
    }

    public Item inInventory(string name)
    {
        foreach (var item in Items)
        {
            if (item.itemName == name)
        {
            return item;
        }
        }
        
        return null;
    }


}
