using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;

    //[SerializeField] private RectTransform pivot;
    void Start()
    {
        toggleDoor();
    }

    private void toggleDoor()
    {
        Debug.Log("Rotating doors?");
        leftDoor.transform.rotation = Quaternion.Euler(0, 0, 0);
        rightDoor.transform.rotation = Quaternion.Euler(0, -180, 0);
    }
}