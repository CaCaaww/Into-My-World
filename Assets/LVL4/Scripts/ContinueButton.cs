using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
   public void ReturnToLevel()
    {
        this.GetParent().GetComponent<Canvas>().enabled = false;
        LVL4Manager.instance.TogglePlayerInput();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public GameObject GetParent()
    {
        return this.gameObject.transform.parent.gameObject;
    }
}
