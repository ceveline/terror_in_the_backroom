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

    private NavMeshAgent flesh;
    public Transform[] patrolPoints;
    private bool isChasing = false;

    TextMeshProUGUI alertText;


    void Start()
    {
        flesh = GetComponent<NavMeshAgent>();
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
    }

    void RandomPatrol()
    {
     
        Transform randomPoint = patrolPoints[Random.Range(0, patrolPoints.Length)];
        flesh.SetDestination(randomPoint.position);
        Debug.Log("Patrolling... " + randomPoint.position);
    }


    void StartChase()
    {
        isChasing = true;
    }

    void HandleChase()
    {
        //chase player
        flesh.SetDestination(player.position);
        Debug.Log("Chasing");
        if (!IsPlayerInSight())
        {
            isChasing = false;
            RandomPatrol();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("colliding");
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


