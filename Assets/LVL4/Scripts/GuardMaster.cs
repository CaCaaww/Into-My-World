using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.ShaderGraph;
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

    // Serialized fields
    [SerializeField, Tooltip("The model/body of the guard")]
    private GameObject guardBody;
    [SerializeField, Tooltip("The points that the guard stops at")]
    private List<GameObject> stoppingPoints;
    [SerializeField, Tooltip("How long the guard stops at every point")]
    private float stopTime;
    [SerializeField, Tooltip("The range around the guard that the player cannot be in")]
    private float aggroRange;
    [SerializeField, Tooltip("The time it takes for the guard to cause the player to lose")]
    private float aggroTime;
    [SerializeField, Tooltip("The speed the guard moves at")]
    private float moveSpeed;
    [SerializeField, Tooltip("The player's capsule or body")]
    private GameObject playerBody;
    [SerializeField, Tooltip("The popup when the player enters the aggro range")]
    private GameObject warningSprite;
    [SerializeField, Tooltip("Panel that appears when the player is caught by a guard")]
    private GameObject gameOverPanel;

    // Private variables
    private GuardState state = GuardState.Patrolling;
    private GuardState prevState = GuardState.Patrolling;
    private GameObject nextPoint;
    private List<GameObject> possiblePoints = new List<GameObject>();
    private float playerAggroTimer;
    private float guardAggroCooldownTimer;
    private float guardStopTimer;

    void Start()
    {
        // Log an error and destroy the guard if the stopping points is less than two
        if (stoppingPoints.Count < 2)
        {
            Debug.LogError(this.name + " has less than 2 stopping points");
            Destroy(this);
            return;
        }

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
    }

    void Update()
    {
        switch (state)
        {
            case GuardState.Patrolling:
                playerAggroTimer = 0.0f;
                guardAggroCooldownTimer += Time.deltaTime;

                // The guard moves in the direction of the target position
                Vector3 guardPos = new Vector3(guardBody.transform.position.x, 0, guardBody.transform.position.z);
                Vector3 targetPos = new Vector3(nextPoint.transform.position.x, 0, nextPoint.transform.position.z);
                Vector3 normalizedDirection = (targetPos - guardPos).normalized;
                if (Vector3.Distance(guardPos, targetPos) <= moveSpeed * Time.deltaTime)
                {
                    // Finds the next point to move to and removes it from the possible points
                    guardBody.transform.position = nextPoint.transform.position;
                    GameObject prev_nextPoint = nextPoint;
                    nextPoint = possiblePoints[Random.Range(0, possiblePoints.Count)];
                    possiblePoints.Remove(nextPoint);
                    possiblePoints.Add(prev_nextPoint);

                    state = GuardState.Stopped;
                }
                else
                {
                    guardBody.transform.position = guardBody.transform.position + normalizedDirection * moveSpeed * Time.deltaTime;
                }

                if (Vector3.Distance(guardBody.transform.position, playerBody.transform.position) < aggroRange && guardAggroCooldownTimer >= 0.2f)
                {
                    state = GuardState.Searching;
                    prevState = GuardState.Patrolling;

                    Debug.Log("Play guard audio");
                    guardBody.GetComponent<AudioSource>().Play();

                    warningSprite.SetActive(true);

                }
                break;
            case GuardState.Searching:
                playerAggroTimer += Time.deltaTime;
                if (playerAggroTimer >= aggroTime)
                {
                    gameOverPanel.SetActive(true);
                    playerBody.GetComponent<FirstPersonController>().enabled = false;

                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }

                if (Vector3.Distance(guardBody.transform.position, playerBody.transform.position) > aggroRange)
                {
                    if (prevState == GuardState.Stopped)
                    {
                        guardStopTimer += 0.2f;
                    }

                    state = prevState;
                    guardAggroCooldownTimer = 0.0f;

                    warningSprite.SetActive(false);

                }
                break;
            case GuardState.Stopped:
                playerAggroTimer = 0.0f;
                guardAggroCooldownTimer += Time.deltaTime;
                guardStopTimer += Time.deltaTime;

                if (guardStopTimer >= stopTime)
                {
                    state = GuardState.Patrolling;
                    guardStopTimer = 0.0f;
                }

                if (Vector3.Distance(guardBody.transform.position, playerBody.transform.position) < aggroRange && guardAggroCooldownTimer >= 0.2f)
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
