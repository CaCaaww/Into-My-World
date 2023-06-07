using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Generator : MonoBehaviour
{
    public GameObject npcPrefab; // Prefab of the NPC
    public int npcCount = 10; // Number of NPCs to generate
    public MeshCollider spawnArea; // Mesh collider defining the spawn area

    private void Start()
    {
        GenerateNPCs();
    }

    private void GenerateNPCs()
    {
        for (int i = 0; i < npcCount; i++)
        {
            Vector3 randomPosition = GetRandomPositionWithinBounds();
            Instantiate(npcPrefab, randomPosition, Quaternion.identity);
        }
    }

    private Vector3 GetRandomPositionWithinBounds()
    {
        Vector3 randomPosition = Vector3.zero;

        if (spawnArea != null && spawnArea.bounds.size != Vector3.zero)
        {
            Bounds bounds = spawnArea.bounds;

            // Attempt to find a valid random position within the spawn area bounds
            int maxAttempts = 100;
            int attempts = 0;

            do
            {
                randomPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    bounds.center.y,
                    Random.Range(bounds.min.z, bounds.max.z)
                );

                attempts++;
            }
            while (!spawnArea.bounds.Contains(randomPosition) && attempts < maxAttempts);

            // If no valid position is found, use a fallback to a completely random position
            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Failed to find a valid position within the spawn area. Using fallback random position.");
                randomPosition = new Vector3(
                    Random.Range(bounds.min.x, bounds.max.x),
                    bounds.center.y,
                    Random.Range(bounds.min.z, bounds.max.z)
                );
            }
        }
        else
        {
            Debug.LogWarning("Spawn area is not defined. Using completely random position.");
            randomPosition = transform.position + Random.insideUnitSphere * 10f;
            randomPosition.y = 0f;
        }

        return randomPosition;
    }
}