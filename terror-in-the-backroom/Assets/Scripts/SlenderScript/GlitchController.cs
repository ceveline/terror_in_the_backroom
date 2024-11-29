using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchController : MonoBehaviour
{

    public GameObject[] glitchImages;  
    public Transform slender;
    public Transform player;
    public float viewAngle = 60f;     
    public float viewDistance = 100f; 
    public float flickerRate = 0.1f;
    private bool isFlickering = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerFacingSlender())
        {
            if (!isFlickering)
            {
                StartCoroutine(FlickerGlitchEffect());
                isFlickering = true;
            }
        }
        else
        {
            if (isFlickering)
            {
                StopCoroutine(FlickerGlitchEffect());
                DisableAllGlitches();
                isFlickering = false;
            }
        }
    }

    bool IsPlayerFacingSlender()
    {
        Vector3 directionToSlender = slender.position - player.position; 
        directionToSlender.y = 0; 

        Vector3 playerForward =  player.forward;

       
        float angle = Vector3.Angle(playerForward, directionToSlender);

        if (angle <= viewAngle / 2)

        {
            if (Vector3.Distance(player.position, slender.position) <= viewDistance)
            {
                return true;
            }
        }

        return false;
    }

    IEnumerator FlickerGlitchEffect()
    {
        while (isFlickering)
        {
           
            int randomIndex = Random.Range(0, glitchImages.Length);
            glitchImages[randomIndex].SetActive(true);

            yield return new WaitForSeconds(Random.Range(flickerRate, flickerRate * 2));
           
            glitchImages[randomIndex].SetActive(false);

            yield return new WaitForSeconds(Random.Range(flickerRate, flickerRate * 2));
        }
    }
    void DisableAllGlitches()
    {
        foreach (GameObject glitchImage in glitchImages)
        {
            glitchImage.SetActive(false);
        }
    }


}
