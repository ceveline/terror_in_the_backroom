using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StartLevelBannerController : MonoBehaviour
{
    public GameObject welcomeBanner;
    public TextMeshProUGUI monsterText;
    public TextMeshProUGUI levelText;

    string monster;
    string level;

    // Start is called before the first frame update
    void Start()
    {
        welcomeBanner.SetActive(true);
        setLevelAndMonster();
        Invoke("hideBanner", 3f);
    }

    void setLevelAndMonster()
    {
        string scene = SceneManager.GetActiveScene().name;

        switch (scene)
        {
            case "Level1":
               monster = "Attack the Skeletons";
               level = "Level One";
                break;
            case "Level2":
                monster = "Escape the Fleshweaver";
                level = "Level Two";
                break;
            case "Level3":
                monster = "Avoid Slenderman";
                level = "Level Three";
                break;
            default:
                level = "";
                break;
        }

        levelText.text = level;
        monsterText.text = monster;
    }

    void hideBanner()
    {
        welcomeBanner.SetActive(false);
    }
}
