using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int itemsCollected = 0;
    public int itemsDroppedOff = 0;

    string sceneName;
    public int itemsToCollect = 0;
    // Singleton pattern
    public static int currentLevel = 0;


    void Awake()
    {
        // singleton pattern
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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

    public void SkeletonStoleItem()
    {
        itemsCollected -= 1;
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

    public void ResetItemsDropped()
    {
        itemsDroppedOff = 0;
    }

   public string getLevel()
    {
        string level = SceneManager.GetActiveScene().name;
        itemsToCollect = setLevelItems(level);
        return level;
    }

    public int setLevelItems(string scene)
    {
        switch (scene)
        {
            case "Level1":
                return 15;
            case "Level2":
                return 20;
            case "Level3":
                return 25;
            default:
                return 15;
        }
    }
    void checkLevelPassingCondition()
    {
        if (itemsCollected >= itemsToCollect && itemsCollected != 0 && itemsDroppedOff >= itemsToCollect)
        {
            //reset items collected
            ResetItems();

            //reset items dropped
            ResetItemsDropped();

            LoadNextScene();
        }
    }

    public void LoadNextScene()
    {
        //modify it to load next scene when all items have been dropped off
        if (SceneManager.GetActiveScene().name == "Level3")
        {
            SceneManager.LoadScene("GameWon");
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void RestartScene() { 

        //SceneManager.LoadScene(currentLevel);
        // add to player controller script, when the player dies, set GameManager.currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
}
