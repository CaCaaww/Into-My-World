using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class LVL2_NPC_Wander : MonoBehaviour
{
    // Changable Variables
    public float movementSpeed = 2f;
    public float minSeparationDistance = 2f; // Minimum separation distance between NPCs

    // Gotten from parent and calcuted on start up
    public Transform[] waypoints;
    public int currentLocation = 0;
    private Transform currentWaypoint;

    // Animation
    private bool isMoving = true;
    Animator animator;

    // Collision detection
    Vector3 desiredVelocity = Vector3.zero;
    Vector3 moveDirection = Vector3.zero;
    bool colliding = false;
    float collisionBuffer = 0f;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        GetStartPoint();
        SetNextWaypoint();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    // Collision Detection
    private void OnCollisionEnter(Collision collision)
    {
        // Use tags to label things the NPC walks around 
        Vector3 separationDirection = Vector3.zero;
        if (collision.gameObject.CompareTag("NPC"))
        {
            // Calculate separation direction away from the other NPC
            separationDirection = transform.position - collision.rigidbody.transform.position;
            desiredVelocity += separationDirection.normalized * (minSeparationDistance - separationDirection.magnitude);
            Debug.Log("NPC");
        }
         if(collision.gameObject.CompareTag("Building"))
         {
             colliding = true;
        
             Vector3 VTo0 = Vector3.zero;
             float rightDot, forawrdDot;
        
             VTo0 =  collision.transform.position - transform.position;
             forawrdDot = Vector3.Dot(VTo0, moveDirection.normalized);
             if (forawrdDot >= 0)
             {
                 rightDot = Vector3.Dot(VTo0, transform.right);
                 
                     if (rightDot < 0)
                     {
        
                         //turn right, scale with forwardDot
                         if (forawrdDot > 0)
                             moveDirection = transform.right * movementSpeed * (1f / forawrdDot);
                             
                     }
                     else
                     {
                         //turn left
                         if (forawrdDot > 0)
                             moveDirection = -transform.right * movementSpeed * (1f / forawrdDot);
                     }
                
             }
        
        
          /*   separationDirection = transform.position - collision.transform.position; ;
             Debug.Log(separationDirection * (minSeparationDistance - separationDirection.magnitude));
        
             separationDirection += Vector3.right;
             desiredVelocity += separationDirection.normalized * (minSeparationDistance - separationDirection.magnitude);
        
             //Quaternion targetRotation = Quaternion.LookRotation(separationDirection);
            // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);
        
             // Check if reached the current waypoint*/
             Debug.Log("Building");
         }
    }
    private void Update()
    {
        collisionBuffer += Time.deltaTime;
        // Reduces call time on update 
        if (!isMoving)
        {
            return;
        }

        // Debug and warning 
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned to LVL2_NPC_Wander on " + gameObject.name);
            return;
        }


        // Collision Code
        if (!colliding)
        {
            moveDirection = (currentWaypoint.position - transform.position).normalized;
        }
        if(collisionBuffer > 3)
        {
            collisionBuffer = 0;
            colliding = false;
        }
        moveDirection.y = 0;
        desiredVelocity += moveDirection.normalized * movementSpeed;


        // Apply movement
        transform.position += desiredVelocity * Time.deltaTime;

        // Rotate towards the current waypoint
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        float stoppingDistance = Random.Range(1, 5);

        // Check if reached the current waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < stoppingDistance)
        {
            SetNextWaypoint();
        }

        desiredVelocity = Vector3.zero;
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

    
    // Move for animatior
    private IEnumerator StopAtWaypoint()
    {
        isMoving = false;
        animator.SetBool("isMoving", false);
        yield return new WaitForSeconds(Random.Range(1f, 5f));
        isMoving = true;
        animator.SetBool("isMoving", true);
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, desiredVelocity);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, moveDirection);
    }
}