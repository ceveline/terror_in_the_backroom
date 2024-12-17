using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SlenderAIController : MonoBehaviour
{
    public Transform player;
    public float attackDistance = 2f;
    private NavMeshAgent slenderAgent;
    public float viewAngle = 60f;
    public float viewDistance = 50f;
    private Animator slenderAnimator;

    // Start is called before the first frame update
    void Start()
    {
            slenderAgent = GetComponent<NavMeshAgent>();
            slenderAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerFacingSlender())
        {

            // slender stops moving when player face him
            slenderAgent.isStopped = true;
            slenderAnimator.SetBool("IsWalking", false);

        }
        else
        {
            slenderAgent.isStopped = false;
            slenderAgent.SetDestination(player.position);
            slenderAnimator.SetBool("IsWalking", true);

        }

        float distanceToPlayer = Vector3.Distance(transform.position,player.position);

        if (distanceToPlayer <= attackDistance){
            slenderAnimator.SetTrigger("AttackTrigger");
            AttackPlayer();
        }

    }

    void AttackPlayer(){
        Debug.Log("Monster Attacked Player");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    bool IsPlayerFacingSlender()
    {
        //position of slender relative to the player, which way slender is from the player
        Vector3 directionSlender = transform.position - player.position;
        directionSlender.y = 0;

        //forawrd direction of player
        Vector3 playerForward= player.forward;

        //angle between playerForawrd and directionSlender
        float angle = Vector3.Angle(playerForward, directionSlender);

        //raycast to check if slender is visible
        Ray ray = new Ray(player.position, directionSlender);
        RaycastHit hit;

        if(angle <= viewAngle && Physics.Raycast(ray, out hit, viewDistance))
        {
           
                if (hit.transform == this.transform)
                {
                    return true;
                }

        }
        return false;
    }

   

}

