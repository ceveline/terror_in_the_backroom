using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class FleshAIController : MonoBehaviour
{

    public Transform player;
    public GameObject playerObj;
    public Transform teleportLocation;
    public float viewDistance = 15f; 
    public float lostSightTime = 3f;
    private float lastSeenTime = -1f;
    private NavMeshAgent flesh;
    public Transform[] patrolPoints;
    private bool isChasing = false;
    private Animator fleshAnimator; 
    private AudioSource audioSource; 
    public AudioClip growlSound;  
    public AudioClip biteSound; 


    TextMeshProUGUI alertText;


    void Start()
    {
        flesh = GetComponent<NavMeshAgent>();
        fleshAnimator = GetComponent<Animator>(); 
        audioSource = GetComponent<AudioSource>();
        //get text to alert player that they've been repositioned
        GameObject alertTextObject = GameObject.Find("StolenItemText");
        alertText = alertTextObject.GetComponent<TextMeshProUGUI>();

        RandomPatrol();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            HandleChase();
        }
        else if (!flesh.pathPending && flesh.remainingDistance<0.5f)
        {
            RandomPatrol();
        }

        //chase if player is in sight and within view distance
        if (!isChasing && IsPlayerInSight())
        {
            StartChase();
        } 
            else if(isChasing && !IsPlayerInSight()){
                //only stop chasing after delay
                if(Time.time-lastSeenTime>lostSightTime){
                StopChase();
            }
        }
        
    }


    void RandomPatrol()
    {
        //getting random patrol point
        Transform randomPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        //setting flesh destination
        flesh.SetDestination(randomPoint.position);
        Debug.Log("Patrolling... " + randomPoint.position);
    }


    void StartChase()
    {
        isChasing = true;
        lastSeenTime = Time.time;
        Debug.Log("Starting chase...");

        audioSource.PlayOneShot(growlSound);
    }

    void StopChase(){
        isChasing = false;
        Debug.Log("Stopped chasing...");
        RandomPatrol();
    }

    void HandleChase()
    {
        //chase player
        flesh.SetDestination(player.position);
        Debug.Log("Chasing..");
    }

    public void OnTriggerEnter(Collider other)
    {
        audioSource.PlayOneShot(biteSound);
        Debug.Log("colliding with player");
        if (other.gameObject.CompareTag("Player"))
        {
            playerObj.SetActive(false);
            player.position=teleportLocation.position;
            playerObj.SetActive(true);
            //avoid repeated tp
            StartCoroutine(DisableFleshCollider());
            AlertPlayerOfKidnapping();
        }
    }

    IEnumerator DisableFleshCollider(){

        //turn off collider
        GetComponent<Collider>().enabled = false;
        //delay
        yield return new WaitForSeconds(3f);
        //turn back on
        GetComponent<Collider>().enabled = true;
        }
    


    bool IsPlayerInSight()
    {
        //check if  player within view distance
       if (Vector3.Distance(transform.position, player.position) > viewDistance){
        return false;
       }

       Vector3 directionToPlayer = (player.position -transform.position).normalized;
       RaycastHit hit;
       if(Physics.Raycast(transform.position,directionToPlayer, out hit, viewDistance)){

            if(hit.collider.CompareTag("Player")){
                return true;
            }
       }
       return false;
    }

    

     void AlertPlayerOfKidnapping()
    {
        alertText.text = "Oh no! You've been kidnapped by the Fleshweaver";

        //reset the text after 5 seconds
        Invoke("resetText", 5f);

    }

     void resetText()
    {
        alertText.text = "";
    }
}


