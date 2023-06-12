using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVL2_NPC_Generator : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab of the NPC
    public int npcCount = 10; // Number of NPCs to generate

    private void Start()
    {
        GenerateNPCs();
    }

    private void GenerateNPCs()
    {

        Collider[] childColliders = GetComponentsInChildren<Collider>();
        for (int i = 0; i < npcCount; i++)
        {

            int randomArea = Random.Range(0, childColliders.Length);
            Bounds childBound = childColliders[randomArea].bounds;
            Vector3 randomPosition = GetRandomPositionWithinBounds(childBound);
            Instantiate(npcPrefab, randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionWithinBounds(Bounds bounds)
    {
        Vector3 randomPosition = Vector3.zero;

        randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            10.6f, // Set Y-position to ground level
            Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPosition;
    }
}