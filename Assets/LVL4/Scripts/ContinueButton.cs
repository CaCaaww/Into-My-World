using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    #region Helper methods
    /// <summary>
    /// Brings the player back to first-person gameplay
    /// </summary>
    public void ReturnToLevel()
    {
        // Get the parent canvas
        this.GetParent().GetComponent<Canvas>().enabled = false;
        // Enable player inputs before returning to first-person POV
        LVL4Manager.instance.TogglePlayerInput();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Returns the parent GameObject
    /// </summary>
    public GameObject GetParent()
    {
        return this.gameObject.transform.parent.gameObject;
    }
    #endregion
}
