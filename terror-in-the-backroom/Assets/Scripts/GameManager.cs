using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
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

    public void LoadNextScene()
    {
        //modify it to load next scene when all items have been dropped off
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
  
    }

    public void RestartScene() { 

        //SceneManager.LoadScene(currentLevel);
        // add to player controller script, when the player dies, set GameManager.currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
}
