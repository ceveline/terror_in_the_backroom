using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class TimerController : MonoBehaviour
{
    public float timeRemaining;
    public bool timerOn = false;
    public TextMeshProUGUI timerText;


    // Start is called before the first frame update
    void Start()
    {
        //10 minutes in seconds is 600
        timeRemaining = 600;
        timerOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            // if there is still time remaining continue to decrease timer
            if(timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                //update timer on screen so player knows how much time is left
                UpdateTimer(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerOn = false;

                //remove all items from inventory
                GameManager.Instance.ResetItems();

                //load game over menu
                SceneManager.LoadScene("GameOver");
            }
        }
    }

    void UpdateTimer(float currentTime)
    {
        currentTime += 1;

        //current time is held in seconds
        float minutes = Mathf.FloorToInt(currentTime / 60);
        float seconds = Mathf.FloorToInt(currentTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes ,seconds);
    }
}
