using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int itemsCollected = 0;
    public int itemsDroppedOff = 0;

    string sceneName = "";
    int itemsToCollect = 0;
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        sceneName = getLevel();
        itemsToCollect = setLevelItems();
    }
    void Update()
    {
        checkLevelPassingCondition();
    }
    public void UpdateItemsCollected()
    {
        itemsCollected += 1;
        Debug.Log("Items Collected: " + itemsCollected);

    }

    public void UpdateItemsDroppedOff()
    {
        itemsDroppedOff += 1;
        Debug.Log("Items Dropped Off: " + itemsDroppedOff);

    }

    public void ResetItems()
    {
        itemsCollected = 0;
        Debug.Log("Items Collected:: " + itemsCollected);

    }

   public string getLevel()
    {
        return SceneManager.GetActiveScene().name;
    }
    public int setLevelItems()
    {
        switch (sceneName)
        {
            case "Level1":
                return 10;
            case "Level2":
                return 14;
            case "Level3":
                return 18;
            default:
                return 10;
        }
    }
    void checkLevelPassingCondition()
    {
        if (itemsCollected == itemsToCollect && itemsCollected != 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
