using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEditor.ShaderGraph;
using UnityEngine;

using UnityEngine.Audio;

public class GuardMaster : MonoBehaviour
{
    // Serialized fields
    [SerializeField, Tooltip("The model/body of the guard")]
    private GameObject guardBody;
    [SerializeField, Tooltip("The points that the guard stops at")]
    private List<GameObject> stoppingPoints;
    [SerializeField, Tooltip("The range around the guard that the player cannot be in")]
    private float agroRange;
    [SerializeField, Tooltip("The speed the guard moves at")]
    private float moveSpeed;

    // Private variables
    private GameObject nextPoint;
    private List<GameObject> possiblePoints = new List<GameObject>();

    void Start()
    {
        if (stoppingPoints.Count < 2)
        {
            Debug.LogError(this.name + " has less than 2 stopping points");
            Destroy(this);
            return;
        }

        guardBody.transform.position = new Vector3(
            stoppingPoints[0].transform.position.x,
            guardBody.transform.position.y,
            stoppingPoints[0].transform.position.z
            );

        nextPoint = stoppingPoints[Random.Range(1, stoppingPoints.Count)];

        foreach (GameObject i in stoppingPoints)
        {
            possiblePoints.Add(i);
        }

        possiblePoints.Remove(nextPoint);
    }

    void Update()
    {
        Vector3 guardPos = new Vector3(guardBody.transform.position.x, 0, guardBody.transform.position.z);
        Vector3 targetPos = new Vector3(nextPoint.transform.position.x, 0, nextPoint.transform.position.z);
        Vector3 normalizedDirection = (targetPos - guardPos).normalized;
        if (Vector3.Distance(guardPos, targetPos) <= moveSpeed * Time.deltaTime)
        {
            guardBody.transform.position = nextPoint.transform.position;
            GameObject prev_nextPoint = nextPoint;
            nextPoint = possiblePoints[Random.Range(0, possiblePoints.Count)];
            possiblePoints.Remove(nextPoint);
            possiblePoints.Add(prev_nextPoint);
        }
        else
        {
            guardBody.transform.position = guardBody.transform.position + normalizedDirection * moveSpeed * Time.deltaTime;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(guardBody.transform.position, agroRange);
        foreach (GameObject i in stoppingPoints)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(i.transform.position, 0.5f);
        }
    }
}
