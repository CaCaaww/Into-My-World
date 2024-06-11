using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class ObjectSelect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }
    public void RayCast() {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 forwardDirection = Camera.main.transform.forward;

        RaycastHit hit;
        // float maxDistance = 1.2F;
        if (Physics.Raycast(cameraPosition, forwardDirection, out hit, 5.0F)) {
            // If the ray hits something, you can access the hit information
            //Debug.Log("Hit: " + hit.collider.gameObject.transform.parent.tag);
            if (hit.collider.gameObject.transform.parent.CompareTag("DoorController")) {
                hit.collider.gameObject.transform.parent.GetComponent<DoorController>().toggleDoor();
                Debug.Log("DoorTag");
            }
            if (hit.collider.gameObject.CompareTag("JailCell")) {
                int randNum = RandomNumberGenerator.GetInt32(0, 3);
                switch (randNum) {
                    case 0:
                        Debug.Log("Load Pipe Game");
                        break;
                    case 1:
                        Debug.Log("Load Matching Game");
                        break;
                    case 2:
                        Debug.Log("Load Maths Game");
                        break;
                }
            }
        }
    }
}
