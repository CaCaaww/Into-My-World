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
            if (hit.collider.gameObject.CompareTag("JailCellGuard")) {
                hit.collider.gameObject.transform.parent.GetComponent<DoorController>().toggleDoor();
                Debug.Log("DoorTag");
            }
            if (hit.collider.gameObject.CompareTag("JailCell")) {
                hit.collider.gameObject.GetComponent<MiniGameController>().lockClicked();
            }
        }
    }
}
