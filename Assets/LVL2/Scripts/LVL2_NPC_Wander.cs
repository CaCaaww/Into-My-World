using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_NPC_Wander : MonoBehaviour
{
    public float speed = 3f; // Speed of NPC movement
    public float wanderRadius = 10f; // Radius within which NPC will wander
    public float waypointThreshold = 1f; // Minimum distance required to consider waypoint reached
    public Vector3 center; // Center position of the roaming area
    public Vector3 roamingAreaSize; // Size of the roaming area
    public float detectionDistance = 2f; // Distance to detect obstacles
    public float avoidanceForce = 5f; // Force applied to avoid obstacles
    private Vector3 targetPosition; // Position to which NPC will move
    private bool isWandering = false; // Flag to indicate if NPC is currently wandering

    void Start()
    {
        targetPosition = transform.position;
    }

    void Wander()
    {
        Vector3 randomPoint = center + new Vector3(
            Random.Range(-roamingAreaSize.x / 2, roamingAreaSize.x / 2),
            0f,
            Random.Range(-roamingAreaSize.z / 2, roamingAreaSize.z / 2)
        );

        targetPosition = randomPoint;
        isWandering = true;
    }

    void Update()
    {
        // A little heavy for a mobile game right now change to every 10th a second later
        if (isWandering)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, detectionDistance))
            {
                Vector3 avoidanceDirection = Vector3.Reflect(direction, hit.normal);
                targetPosition = hit.point + avoidanceDirection * avoidanceForce;
            }

            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < waypointThreshold)
            {
                isWandering = false;
                Wander();
            }
        }
    }
}