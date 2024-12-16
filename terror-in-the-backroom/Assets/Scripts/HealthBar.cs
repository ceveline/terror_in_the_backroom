using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public float maxHealth = 100;
    public float health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        GameManager.currentLevel = SceneManager.GetActiveScene().buildIndex;

        if (healthSlider.value != health)
        {
            healthSlider.value = health;
        }

    }

    public void takeDamage(float damage)
    {
        health -= damage;

        if (gameObject.tag == "Player" && health < 0)
        {
            Die();
        }
    }

    void Die()
    {

        if (health < 0)
        {
            SceneManager.LoadScene("GameOver");
        }
    }
}
