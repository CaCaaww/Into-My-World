using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_NPC_Wander : MonoBehaviour
{
    public float movementSpeed = 2f;
    public float stoppingDistance = 1f; // Distance to stop before the waypoint

    public Transform[] waypoints;
    public int currentLocation = 0;
    private Transform currentWaypoint;

    private bool isMoving = true;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetNextWaypoint();
    }

    private void Update()
    {
        if (!isMoving)
        {
            return;
        }

        if (waypoints.Length == 0)
        {
            Debug.LogWarning("No waypoints assigned to LVL2_NPC_Wander on " + gameObject.name);
            return;
        }

        // Move towards the current waypoint
        Vector3 direction = (currentWaypoint.position - transform.position).normalized;
        direction.y = 0;
        rb.AddForce(direction * movementSpeed * Time.deltaTime, ForceMode.VelocityChange);

        // Rotate towards the current waypoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 5f * Time.deltaTime);

        // Check if reached the current waypoint
        if (Vector3.Distance(transform.position, currentWaypoint.position) < stoppingDistance)
        {
            SetNextWaypoint();
        }
    }

    private void SetNextWaypoint()
    {
        currentLocation = (currentLocation + 1) % waypoints.Length;
        currentWaypoint = waypoints[currentLocation];
        StartCoroutine(StopAtWaypoint());
    }

    private IEnumerator StopAtWaypoint()
    {
        isMoving = false;
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        isMoving = true;
    }
}
