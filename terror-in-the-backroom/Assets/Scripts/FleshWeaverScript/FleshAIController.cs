using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class FleshAIController : MonoBehaviour
{

    public Transform player;
    public GameObject playerObj;
    public Transform floor;
    public Transform teleportLocation;
    public float viewDistance = 15f; 
    public float attackRange = 2f; 
    public float stopChaseDistance = 20f; 
    public int numberOfWaypoints = 5; 

    private NavMeshAgent flesh;
    private Vector3[] waypoints;
    private int currentWaypoint = 0;
    private bool isChasing = false;

    TextMeshProUGUI alertText;

    // Start is called before the first frame update
    void Start()
    {
        flesh = GetComponent<NavMeshAgent>();
        GenerateWaypoints();

        //get text to alert player that they've been repositioned
        GameObject alertTextObject = GameObject.Find("StolenItemText");
        alertText = alertTextObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChasing)
        {
            HandleChase();
        }
        else
        {
            Patrol();
        }

        //chase if player is in sight and within view distance
        if (!isChasing && IsPlayerInSight())
        {
            StartChase();
        }
    }
        void Patrol()
        {
           
            if (!flesh.hasPath || flesh.remainingDistance < 0.5f)
            {
                currentWaypoint = Random.Range(0, waypoints.Length);
                flesh.SetDestination(waypoints[currentWaypoint]);
            }
        }

        void StartChase()
        {
            isChasing = true;
        }

        void HandleChase()
        {
            //chase player
            flesh.SetDestination(player.position);

            if (Vector3.Distance(transform.position, player.position) > stopChaseDistance)
            {
                isChasing = false;
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

                AlertPlayerOfKidnapping();
            }
        }
       


        bool IsPlayerInSight()
        {
            //check if  player within view distance
            return Vector3.Distance(transform.position, player.position) <= viewDistance;
        }

        void GenerateWaypoints()
        {
            //bounds of floor
            Vector3 floorPosition = floor.position;
            Vector3 floorScale = floor.localScale; 

            float floorMinX = floorPosition.x - (floorScale.x / 2f);
            float floorMaxX = floorPosition.x + (floorScale.x / 2f);
            float floorMinZ = floorPosition.z - (floorScale.z / 2f);
            float floorMaxZ = floorPosition.z + (floorScale.z / 2f);

            // random waypoints 
            waypoints = new Vector3[numberOfWaypoints];
            for (int i = 0; i < numberOfWaypoints; i++)
            {
                float randomX = Random.Range(floorMinX, floorMaxX);
                float randomZ = Random.Range(floorMinZ, floorMaxZ);
                waypoints[i] = new Vector3(randomX, floorPosition.y, randomZ);
            }
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


