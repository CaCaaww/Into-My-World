using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_NPC_Generator : MonoBehaviour
{
    public GameObject[] npcPrefabs; // Prefab of the NPC
    public int npcCount = 10; // Number of NPCs to generate can be changed
    public Transform[] waypoints; // Gotten from the Spawner Object and given to children

    private void Start()
    {
        GenerateNPCs();
    }

    private void GenerateNPCs()
    {
        // Gets the colliders which is the spawn areas.
        // Set to be box coliders that are above the player for x and z axis
        Collider[] childColliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < npcCount; i++)
        {
            // Randomly select and place a npc in one of the children collider axis
            int randomArea = Random.Range(0, childColliders.Length);
            int randomPrefab = Random.Range(0, npcPrefabs.Length);
            Bounds childBound = childColliders[randomArea].bounds;
            Vector3 randomPosition = GetRandomPositionWithinBounds(childBound);
            GameObject npc = Instantiate(npcPrefabs[randomPrefab], randomPosition, Quaternion.identity);

            // Attach NPCController script to the NPC
            //  LVL2_NPC_Wander npcController = npc.GetComponent<LVL2_NPC_Wander>();
            LVL2_NPC_WanderNavMesh npcController = npc.GetComponent<LVL2_NPC_WanderNavMesh>();
            if (npcController == null)
            {
                // npcController = npc.AddComponent<LVL2_NPC_Wander>();
                npcController = npc.AddComponent<LVL2_NPC_WanderNavMesh>();
            }

            // Assign waypoints to the NPCController
            npcController.waypoints = waypoints;
        }
    }

    private Vector3 GetRandomPositionWithinBounds(Bounds bounds)
    {
        // Position
        Vector3 randomPosition = Vector3.zero;

        // Selecting position.
        // "out of bounds" which is slightly above and below the max and min
        // To avoid this issue simply make the box smaller.
        // This is because being on the edge of the bound does not calculate the model size
  
        randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            -3.5f, // Sets Y-position to ground level which is -3.5 when game is set to 0
                   // If you wish to change this to ground level then you must drag the entire game to y 0
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPosition;
    }
}