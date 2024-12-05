using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAIController : MonoBehaviour
{

    public Transform[] patrolPoints; 
    private NavMeshAgent skeleton;
    private int lastPatrolIndex = -1;
    private bool isWaiting = false;

    // Start is called before the first frame update
    void Start()
    {
        skeleton = GetComponent<NavMeshAgent>();
        MoveToNextPatrolPoint();
    }

    // Update is called once per frame
    void Update()
    {
          if (!isWaiting && !skeleton.pathPending && skeleton.remainingDistance < 0.5f)
        {
            StartCoroutine(DelayMoveToNextPatrolPoint());
        }
    }

    private IEnumerator DelayedMoveToNextPatrolPoint()
    {
        isWaiting = true;

        yield return new WaitForSeconds(2);

        MoveToNextPatrolPoint();
        isWaiting = false;
    }

     private void MoveToNextPatrolPoint()
    {
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, patrolPoints.Length);
        } while (randomIndex == lastPatrolIndex);

        lastPatrolIndex = randomIndex;


        skeleton.SetDestination(patrolPoints[randomIndex].position);
    }

}
