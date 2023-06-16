using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_NPC_Wander : MonoBehaviour
{
    public float movementSpeed = 3f;

    public Transform[] waypoints;
    public int currentLocation = 0;
    private Transform currentWaypoint;
    public float waypointReachedThreshold = 0.1f; // Distance threshold to consider a waypoint reached


    private void Update()
    {
        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned to NPCController on " + gameObject.name);
            return;
        }

        // Move towards the current waypoint
        Transform currentWaypoint = waypoints[currentLocation];
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, movementSpeed * Time.deltaTime);

        // Rotate towards the current waypoint
        Quaternion targetRotation = Quaternion.LookRotation(currentWaypoint.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        // Check if reached the current waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < waypointReachedThreshold)
        {
            // Move to the next waypoint
            currentLocation = (currentLocation + 1) % waypoints.Length;
        }
    }
}