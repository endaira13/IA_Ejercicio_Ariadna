using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    enum State
    {
        Patrolling,
        Chasing,
        Traveling,
        Waiting,
        Attacking
    }

    State currentState;

    NavMeshAgent agent;

    public Transform[] destinationPoints;
    public int destinationIndex = 5;

    public Transform player;
    [SerializeField]
    float visionRange;

    [SerializeField]
    float visionAtack;


    [SerializeField]
    float patrolRange = 10f;
    
    [SerializeField]
    float timer = 0f;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.Patrolling;
        
        
        destinationIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
       switch(currentState)
       {
           case State.Patrolling:
            Patrol();
           break;

           case State.Chasing:
            Chase();
           break;

           case State.Waiting:
            Wait();
           break;

            case State.Attacking:
             Atack();
            break;

           case State.Traveling:
            Chase();
           break;

           deafult:
            Chase();
           break;
       } 
    }

    void Patrol()
    {
        agent.destination = destinationPoints[destinationIndex].position;

        if(Vector3. Distance(transform.position,destinationPoints[destinationIndex].position) < 1 )
        {
            destinationIndex ++;
            timer = 0;
            
        }

        else if(destinationIndex +1 == destinationPoints.Length)
        {
            destinationIndex = 0;
            timer = 0;
        }

        if(timer <= 5f)
        {
            currentState = State.Waiting;
        }
        

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }

        if(Vector3.Distance(transform.position, player.position) < visionAtack)
        {
            currentState = State.Attacking;
        }
        
    }


    /* void Patrol()
    {
        Vector3 randomPosition;
        if(RandomPoint(transform.position, patrolRange, out randomPosition))
        {
            agent.destination = randomPosition;
        }
        
        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }

        currentState = State.Traveling;
        
    }

    /* bool RandomPoint(Vector3 center, float range, out Vector3 point)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range;
        NavMeshHit hit;
       if(NavMesh.SamplePosition(randomPoint, out hit, 4, NavMesh.AllAreas))
       {
          point = hit.position;
          return true;
       }
       point = Vector3.zero;
       return false;
    }

    void Travel()
    {
        if(agent.remainingDistance <= 0.2)
        {
            currentState = State.Patrolling;
        }

        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Chasing;
        }


    }*/

    void Chase()
    {
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) > visionRange)
        {
            currentState = State.Patrolling;
        }

        if(Vector3.Distance(transform.position, player.position) < visionAtack)
        {
            currentState = State.Attacking;
        }
    }

    void Wait()
    {
        agent.destination = destinationPoints[destinationIndex].position;
        timer += Time.deltaTime;

        if(timer <= 5f)
        {
            currentState = State.Waiting;
        }
        else if(timer >= 0f)
        {
            currentState = State.Patrolling;
        }

        if(Vector3.Distance(transform.position, player.position) < visionRange)
        {
            currentState = State.Chasing;
        }
    }

    void Atack()
    {
        agent.destination = player.position;

        if(Vector3.Distance(transform.position, player.position) < visionAtack)
        {
            Debug.Log("Atack");
        }

        else if(Vector3.Distance(transform.position, player.position) > visionAtack)
        {
            currentState = State.Chasing;
        }
    }

    void OnDrawGizmos()
    {
        foreach(Transform point in destinationPoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(point.position, 1);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, visionAtack);
    }
}
