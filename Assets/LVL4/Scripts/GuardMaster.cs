using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class GuardMaster : MonoBehaviour
{
    private enum GuardState
    {
        Patrolling,
        Searching,
        Stopped
    }

    #region Inspector
    [SerializeField, Tooltip("The model/body of the guard")]
    private GameObject guardBody;
    [SerializeField, Tooltip("The animator to control guard body animations")]
    private Animator animator;
    [SerializeField, Tooltip("The speed of the guard's animations")]
    private float animationSpeed = 1;
    [SerializeField, Tooltip("The points that the guard stops at")]
    private List<GameObject> stoppingPoints;
    [SerializeField, Tooltip("The lowest to highest amount of time that the guard can stop for")]
    private Vector2 stopTimeRange;
    [SerializeField, Tooltip("The range around the guard that the player cannot be in")]
    private float aggroRange;
    [SerializeField, Tooltip("The time it takes for the guard to cause the player to lose")]
    private float aggroTime;
    [SerializeField, Tooltip("The speed the guard moves at")]
    private float moveSpeed;
    [SerializeField, Tooltip("Male and female guard bodies")]
    private List<GameObject> guardPrefabs;
    [SerializeField, Tooltip("Face textures for the wandering guard")]
    private Texture2D angryFace, neutralFace;
    [SerializeField, Tooltip("Sprite to display above guard head when player is in aggro range")]
    private GameObject warningSprite;
    [SerializeField, Tooltip("Player data")]
    private PlayerDataSO playerData;

    [Header("Listening Event Channels")]
    [SerializeField, Tooltip("Happens when the player is caught")]
    private GameOverEventChannel gameOverEventChannel;
    #endregion

    #region Private Variables
    private GuardState state = GuardState.Patrolling;
    private GuardState prevState = GuardState.Patrolling;
    private GameObject nextPoint;
    private List<GameObject> possiblePoints = new List<GameObject>();
    private float playerAggroTimer;
    private float guardAggroCooldownTimer;
    private float guardStopTimer;
    private float guardLerpTimer;
    private Vector3 curRotation;
    private Vector3 targetRotation;
    private float stopTime;
    private MeshRenderer facePlate;
    private Transform guardHead;
    private bool isGameOver = false;
    #endregion

    private const float turnAnimationTime = 0.5f;

    void Start()
    {
        // Log an error and destroy the guard if the stopping points is less than two
        if (stoppingPoints.Count < 2)
        {
            Debug.LogError(this.name + " has less than 2 stopping points");
            Destroy(this);
            return;
        }

        // Assign female or male guard body
        GameObject model = Instantiate(guardPrefabs[Random.Range(0, guardPrefabs.Count)], animator.transform);
        foreach (MeshRenderer i in model.GetComponentsInChildren<MeshRenderer>())
        {
            if (i.gameObject.name.Contains("Face_Plate"))
            {
                facePlate = i;
                break;
            }
        }

        animator.Rebind();

        // Make the guard start at the first stopping point
        guardBody.transform.position = new Vector3(
            stoppingPoints[0].transform.position.x,
            guardBody.transform.position.y,
            stoppingPoints[0].transform.position.z
            );

        // Randomly set next point
        nextPoint = stoppingPoints[Random.Range(1, stoppingPoints.Count)];

        // Add the points to the set of possible points and remove the point the guard is currently heading to
        foreach (GameObject i in stoppingPoints)
        {
            possiblePoints.Add(i);
        }
        possiblePoints.Remove(nextPoint);

        // Set the timer to zero
        playerAggroTimer = 0.0f;

        animator.SetBool("Walking", true);
        guardBody.transform.LookAt(nextPoint.transform);
    }

    void Update()
    {
        animator.SetFloat("AnimationSpeed", animationSpeed);

        switch (state)
        {
            case GuardState.Patrolling:

                animator.SetBool("Walking", true);
                animator.SetBool("Turning", false);
                facePlate.material.mainTexture = neutralFace;
                playerAggroTimer = 0.0f;
                guardAggroCooldownTimer += Time.deltaTime;

                // The guard moves in the direction of the target position
                Vector3 guardPos = new Vector3(guardBody.transform.position.x, 0, guardBody.transform.position.z);
                Vector3 targetPos = new Vector3(nextPoint.transform.position.x, 0, nextPoint.transform.position.z);
                Vector3 normalizedDirection = (targetPos - guardPos).normalized;
                guardBody.transform.rotation = Quaternion.Slerp(guardBody.transform.rotation, Quaternion.LookRotation(normalizedDirection), Time.deltaTime * 5f);
                if (Vector3.Distance(guardPos, targetPos) <= moveSpeed * Time.deltaTime)
                {
                    // Finds the next point to move to and removes it from the possible points
                    guardBody.transform.position = nextPoint.transform.position;
                    GameObject prev_nextPoint = nextPoint;
                    nextPoint = possiblePoints[Random.Range(0, possiblePoints.Count)];
                    possiblePoints.Remove(nextPoint);
                    possiblePoints.Add(prev_nextPoint);

                    curRotation = guardBody.transform.rotation.eulerAngles;
                    targetRotation = Quaternion.LookRotation((nextPoint.transform.position - guardBody.transform.position).normalized, guardBody.transform.up).eulerAngles;
                    guardLerpTimer = 0.0f;

                    stopTime = Random.Range(stopTimeRange.x, stopTimeRange.y);

                    state = GuardState.Stopped;
                }
                else
                {
                    guardBody.transform.position = guardBody.transform.position + normalizedDirection * moveSpeed * Time.deltaTime;
                }

                if (Vector3.Distance(guardBody.transform.position, playerData.Transform.position) < aggroRange && guardAggroCooldownTimer >= 0.2f)
                {
                    state = GuardState.Searching;
                    prevState = GuardState.Patrolling;

                    Debug.Log("Play guard audio");
                    guardBody.GetComponent<AudioSource>().Play();

                    warningSprite.SetActive(true);

                }
                break;
            case GuardState.Searching:
                animator.SetBool("Walking", false);
                animator.SetBool("Turning", false);
                facePlate.material.mainTexture = angryFace;
                playerAggroTimer += Time.deltaTime;

                Vector3 playerDirection = (playerData.Transform.position - guardBody.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(playerDirection.x, 0, playerDirection.z));
                guardBody.transform.rotation = Quaternion.Slerp(guardBody.transform.rotation, lookRotation, Time.deltaTime * 5f);

                GetComponentInChildren<LVL4_GuardLookAt>().lookAt = true;

                if (playerAggroTimer >= aggroTime && !isGameOver)
                {
                    gameOverEventChannel.RaiseEvent();
                    isGameOver = true;
                    /* ========================== SEND DATA TO SERVER HERE ==============================*/
                }

                if (Vector3.Distance(guardBody.transform.position, playerData.Transform.position) > aggroRange)
                {
                    state = prevState;
                    GetComponentInChildren<LVL4_GuardLookAt>().lookAt = false;
                    guardAggroCooldownTimer = 0.0f;
                    warningSprite.SetActive(false);
                }
                break;
            case GuardState.Stopped:
                animator.SetBool("Walking", false);
                if (guardStopTimer <= turnAnimationTime)
                {
                    animator.SetBool("Turning", true);
                }
                else
                {
                    animator.SetBool("Turning", false);
                }
                facePlate.material.mainTexture = neutralFace;
                playerAggroTimer = 0.0f;
                guardAggroCooldownTimer += Time.deltaTime;
                guardStopTimer += Time.deltaTime;
                guardLerpTimer += Time.deltaTime;

                Quaternion newRotation = Quaternion.Lerp(Quaternion.Euler(curRotation), Quaternion.Euler(targetRotation), guardLerpTimer / turnAnimationTime);
                guardBody.transform.rotation = newRotation;

                if (guardStopTimer >= stopTime)
                {
                    state = GuardState.Patrolling;
                    guardStopTimer = 0.0f;
                }

                if (Vector3.Distance(guardBody.transform.position, playerData.Transform.position) < aggroRange && guardAggroCooldownTimer >= 0.2f)
                {
                    state = GuardState.Searching;
                    prevState = GuardState.Stopped;

                    Debug.Log("Play guard audio");
                    guardBody.GetComponent<AudioSource>().Play();

                    warningSprite.SetActive(true);
                }
                break;
        }
    }

    void OnDrawGizmos()
    {
        // Draws the visual representation of the aggro zone
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(guardBody.transform.position, aggroRange);

        // Draws the visual representation of each point
        foreach (GameObject i in stoppingPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(i.transform.position, 0.5f);
        }
    }
}
