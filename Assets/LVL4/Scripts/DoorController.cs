using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private bool onLeftSideOfHall;
    //[SerializeField] private GameObject lockObject;
    //[SerializeField] private RectTransform pivot;
    public void toggleDoor()
    {
        //Debug.Log("Toggling doors");
        if (onLeftSideOfHall)
        {
            leftDoor.transform.rotation = Quaternion.Euler(0, 0, 0);
            rightDoor.transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            leftDoor.transform.rotation = Quaternion.Euler(0, -180, 0);
            rightDoor.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}