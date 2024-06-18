using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    #region Inspector
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private bool onLeftSideOfHall;
    #endregion

    #region Private Variables
    private const float xRotation = 0;
    private const float yRotationLeft = 0;
    private const float yRotationRight = -180;
    private const float zRotation = 0;
    #endregion

    public void toggleDoor()
    {
        // Without checking what side of the hall the door is on, then
        // all doors would open in the same direction globally. Doors should
        // open in the same direction relative to the player
        if (onLeftSideOfHall)
        {
            leftDoor.transform.rotation = Quaternion.Euler(xRotation, yRotationLeft, zRotation);
            rightDoor.transform.rotation = Quaternion.Euler(xRotation, yRotationRight, zRotation);
        }
        else
        {
            leftDoor.transform.rotation = Quaternion.Euler(xRotation, yRotationRight, zRotation);
            rightDoor.transform.rotation = Quaternion.Euler(xRotation, yRotationLeft, zRotation);
        }
    }
}