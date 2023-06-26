using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class LVL2_NPC_WanderNavMesh : MonoBehaviour
{

    //serialized fields
    [SerializeField]
    private float collisionScalerFront = 1f;
    [SerializeField]
    private float collisionScalerLeft = 1f;
    [SerializeField]
    private float collisionScalerRight = 1f;

    //time for updating collisions
    float time = 0;
    public float updateSpeed = 0f;

    //navmesh agent, and hits
    private NavMeshAgent agent;
    private NavMeshHit hitFront;
    private NavMeshHit hitLeft;
    private NavMeshHit hitRight;




    // Gotten from parent and calcuted on start up
    public Transform[] waypoints;
    public int currentLocation = 0;
    public Transform currentWaypoint;

    //prevPos to stop player
    Vector3 prevPos;

    // Animation
    Animator animator;

    //bools for directions
    bool blockedForward;
    bool blockedRight;
    bool blockedLeft;

    //transforms so they won't get reinitialized every frame, same with additional vectors
    Vector3 transformForward;
    Vector3 transformRight;
    Vector3 transformLeft;
    Vector3 targetVector;
    Vector3 VTo0 = Vector3.zero;
    float rightDot, forawrdDot;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        GetStartPoint();
        SetNextWaypoint();
        agent.SetDestination(currentWaypoint.position);
        prevPos = transform.position;
    }



        
        private void Update()
        {

            time += Time.deltaTime;

            // Debug and warning 
            if (waypoints.Length == 0)
            {
                Debug.LogWarning("No waypoints assigned to LVL2_NPC_Wander on " + gameObject.name);
                return;
            }
            
            //if the npc hasn't moved, cancel the walking animation
            if(Vector3.Distance(transform.position, prevPos) < .5 * Time.deltaTime)
            {
                animator.SetBool("isMoving", false);
            }
            else animator.SetBool("isMoving", true);

            prevPos = transform.position;
            
            //check collisions and reupdate direction every update speed seconds
            if (time > updateSpeed)
            {

              time = 0;

              //raycast points, can be scaled to detect collisions farther out
              transformForward = transform.position + transform.forward * collisionScalerFront; 
              transformRight = transform.position + transform.right * collisionScalerRight;
              transformLeft = transform.position + -transform.right * collisionScalerLeft;


             targetVector = Vector3.zero;

             //raycast in the forward direction
             blockedForward = NavMesh.Raycast(transform.position, transformForward, out hitFront, NavMesh.AllAreas);
            
             Debug.DrawLine(transform.position, transformForward, blockedForward ? Color.red : Color.green);

            //if colliding in front, calculate a way around and add it to the new target vector
            if (blockedForward)
            {
                Debug.DrawRay(hitFront.position, Vector3.up, Color.red);

                VTo0 = Vector3.zero;

                VTo0 = hitFront.position - transform.position;
                forawrdDot = Vector3.Dot(VTo0, transform.forward);
                if (forawrdDot >= 0)
                {
                    rightDot = Vector3.Dot(VTo0, transform.right);

                    if (rightDot < 0)
                    {
                        //turn right, scale with forwardDot
                        if (forawrdDot > 0)
                            targetVector += transform.right * (1f / forawrdDot);
                    }
                    else
                    {
                        //turn left
                        if (forawrdDot > 0)
                            targetVector += -transform.right * (1f / forawrdDot);
                    }
                }
            }


              //raycast left
             blockedLeft = NavMesh.Raycast(transform.position, transformLeft, out hitLeft, NavMesh.AllAreas);
            
             Debug.DrawLine(transform.position, transformLeft, blockedLeft ? Color.red : Color.green);

            //if blocked add to the target vector
            if (blockedLeft)
            {
               Debug.DrawRay(hitLeft.position, Vector3.up, Color.red);
               targetVector += hitLeft.position - transformLeft;
            }

           

            //raycast right
            blockedRight = NavMesh.Raycast(transform.position, transformRight, out hitRight, NavMesh.AllAreas);

            Debug.DrawLine(transform.position, transformRight, blockedRight ? Color.red : Color.green);

            //if blocked add to the vector
            if (blockedRight)
            {
                targetVector += hitRight.position - transformRight;
                Debug.DrawRay(hitRight.position, Vector3.up, Color.red);
            }


            Debug.DrawLine(transform.position, transform.position + targetVector * 5f, Color.black);

            //if blocked, redirect the agent
            if ((blockedLeft || blockedForward || blockedRight))
            {              
                    agent.SetDestination(transform.position + targetVector.normalized * 5f);                
            }
            else //otherwise go back to the waypoint
            {
                agent.SetDestination(currentWaypoint.position);
            }   
        }
        

        float stoppingDistance = Random.Range(1, 5);
        // Check if reached the current waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < stoppingDistance)
        {
            SetNextWaypoint();
            agent.SetDestination(currentWaypoint.position);
        }
    }


    // On start to get current location since NPCS are randomly spawned
    private void GetStartPoint()
        {
            float closestDistance = Vector3.Distance(transform.position, waypoints[0].position);
            float distance = 0;
            int closestIndex = 0;

            for (int i = 1; i < waypoints.Length; i++)
            {
                distance = Vector3.Distance(transform.position, waypoints[i].position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestIndex = i;
                }
            }

            currentLocation = closestIndex;
            currentWaypoint = waypoints[currentLocation];
        }

    // Gets next point to go HARDCODED. You will need to make a node map if you want to change the waypoints
    private void SetNextWaypoint()
    {
        if (Vector3.Distance(transform.position, waypoints[currentLocation].position) <= 0)
        {
            StartCoroutine(StopAtWaypoint());
        }

        switch (currentLocation)
        {
            case 0:
                int[] numbers = {1,3,6,7};
                int selectedIndex = Random.Range(0, numbers.Length);
                currentLocation = numbers[selectedIndex];
                break;

            case 1:
                int[] numbers1 = {0,2, 4, 5};
                int selectedIndex1 = Random.Range(0, numbers1.Length);
                currentLocation = numbers1[selectedIndex1];
                break;
            case 2:
                int[] numbers2 = {1, 3};
                int selectedIndex2 = Random.Range(0, numbers2.Length);
                currentLocation = numbers2[selectedIndex2];
                break;
            case 3:
                int[] numbers3 = {0,3,11};
                int selectedIndex3 = Random.Range(0, numbers3.Length);
                currentLocation = numbers3[selectedIndex3];
                break;
            case 4:
                int[] numbers4 = {1,5};
                int selectedIndex4 = Random.Range(0, numbers4.Length);
                currentLocation = numbers4[selectedIndex4];
                break;
            case 5:
                int[] numbers5 = {1,4};
                int selectedIndex5 = Random.Range(0, numbers5.Length);
                currentLocation = numbers5[selectedIndex5];
                break;
            case 6:
                int[] numbers6 = {0,7,8};
                int selectedIndex6 = Random.Range(0, numbers6.Length);
                currentLocation = numbers6[selectedIndex6];
                break;
            case 7:
                int[] numbers7 = {6, 8, 10};
                int selectedIndex7 = Random.Range(0, numbers7.Length);
                currentLocation = numbers7[selectedIndex7];
                break;
            case 8:
                int[] numbers8 = {6,9,10};
                int selectedIndex8 = Random.Range(0, numbers8.Length);
                currentLocation = numbers8[selectedIndex8];
                break;
            case 9:
                int[] numbers9 = {8,10};
                int selectedIndex9 = Random.Range(0, numbers9.Length);
                currentLocation = numbers9[selectedIndex9];
                break;
            case 10:
                int[] numbers10 = {7,9,13};
                int selectedIndex10 = Random.Range(0, numbers10.Length);
                currentLocation = numbers10[selectedIndex10];
                break;
            case 11:
                int[] numbers11 = {2,12};
                int selectedIndex11 = Random.Range(0, numbers11.Length);
                currentLocation = numbers11[selectedIndex11];
                break;
            case 12:
                int[] numbers12 = {11,13};
                int selectedIndex12 = Random.Range(0, numbers12.Length);
                currentLocation = numbers12[selectedIndex12];
                break;
            case 13:
                int[] numbers13 = {10,12};
                int selectedIndex13 = Random.Range(0, numbers13.Length);
                currentLocation = numbers13[selectedIndex13];
                break;
            default:
                Debug.LogWarning("No point was found for NPC");
                break;
        }
        currentWaypoint = waypoints[currentLocation];
        StartCoroutine(StopAtWaypoint());
    }

    
    // Move for animatior, and stops the navAgent from moving
    private IEnumerator StopAtWaypoint()
    {
        agent.isStopped = true;
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        animator.SetBool("isMoving", true);
        agent.isStopped = false;
    }


}