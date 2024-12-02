using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    // Start is called before the first frame update
    public void StartGame()
    {

        SceneManager.LoadScene("Level1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void ExitGame()
    {

    }

    public void RestartScene()
    {
        SceneManager.LoadScene(GameManager.currentLevel);
        // add to player controller script, when the player dies, set GameManager.currentLevel = SceneManager.GetActiveScene().buildIndex;
    }
}
