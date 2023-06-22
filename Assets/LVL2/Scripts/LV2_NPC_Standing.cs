using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LV2_NPC_Standing : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab of the NPC
    private Transform waypoint; // Position to spawn the NPC
    public float detectionRadius = 3f; // Radius to detect the player
    private Animator npcAnimator; // Animator component of the NPC

    private GameObject spawnedNPC; // Reference to the spawned NPC

    // Start is called before the first frame update
    void Start()
    {
        SpawnNPC();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player is within the detection radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {

                Vector3 direction = collider.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                spawnedNPC.transform.rotation = Quaternion.Lerp(spawnedNPC.transform.rotation, targetRotation, 5f * Time.deltaTime);
                // Trigger animation change
                npcAnimator.SetBool("isPlayerClose", true);
                break;
            }
			else
			{
                npcAnimator.SetBool("isPlayerClose", false);
			}
        }
    }

    private void SpawnNPC()
    {
        waypoint = gameObject.GetComponent<Transform>();
        spawnedNPC = Instantiate(npcPrefab, waypoint.position, waypoint.rotation);
        npcAnimator = spawnedNPC.GetComponent<Animator>();
    }
}
