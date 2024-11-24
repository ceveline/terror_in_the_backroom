using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlenderFollowAI : MonoBehaviour
{
    public Tranform player;
    public float attackDistance = 2f;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);

        float distanceToPlayer = Vector3.Distance(transform.position,player.position);

        if (distanceToPlayer <= attackDistance){
            AttackPlayer();
        }
    }

    void AttackPlayer(){
        Debug.Log("Monster Attacked");
    }
}

