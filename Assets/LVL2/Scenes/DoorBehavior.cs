using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class DoorBehavior : MonoBehaviour{

    // Adjustable numbers to change how the door is interacted with
    public float doorOpenAngle = 90f;
    public float doorOpenDuration = 1f;
    public float doorOpenDistance = 3f;

    private Quaternion initialRotation;
    private Quaternion targetRotation;
    private bool isOpen = false;

    private void Start(){
        // Sets the rotation for the door
        initialRotation = transform.rotation;
        targetRotation = initialRotation * Quaternion.Euler(0f, doorOpenAngle, 0f);
    }

    private void Update(){
        // Check the distance between the door and the player Can be changed to mouse click if needed
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            return;

        // Distance from player to the door
        float distance = Vector3.Distance(transform.position, player.transform.position);

        // Open and close
        if (distance <= doorOpenDistance && !isOpen)
        {
            OpenDoor();
        }
        else if (distance > doorOpenDistance && isOpen)
        {
            CloseDoor();
        }
    }

    private void OpenDoor(){
        transform.DORotateQuaternion(targetRotation, doorOpenDuration);
        isOpen = true;
    }

    private void CloseDoor(){
        transform.DORotateQuaternion(initialRotation, doorOpenDuration); 
        isOpen = false;
    }
}