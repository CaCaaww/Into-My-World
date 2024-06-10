using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToLookAt;

    void Update()
    {
        this.gameObject.transform.LookAt(objectToLookAt.transform);
    }
}
