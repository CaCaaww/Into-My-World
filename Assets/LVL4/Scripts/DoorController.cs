using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;


public class DoorController : MonoBehaviour
{
    #region Inspector
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private bool onLeftSideOfHall;

    [Header("Listening Event Channels")]
    [SerializeField] private GenericEventChannelSO<DoorOpenedEvent> DoorOpenedEventChannel;
    #endregion

    #region Private Variables
    private const float xRotation = 0;
    // The open position for a door on the left side of the hall
    private const float yRotationLeft = 0;
    // The open position for a door on the right side of the hall
    private const float yRotationRight = -180;
    private const float zRotation = 0;
    #endregion

    #region Unity Methods
    private void OnEnable() {
        DoorOpenedEventChannel.OnEventRaised += OnDoorOpened;
    }
    #endregion

    #region Callbacks
    private void OnDoorOpened(DoorOpenedEvent evt) {
        if (evt.controller == this || evt.debugCall == true) { toggleDoor(); }
    }
    #endregion

    #region Public Methods
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
    #endregion
}