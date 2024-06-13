using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
   public void ReturnToLevel()
    {
        this.GetParent().SetActive(false);
        LVL4Manager.instance.TogglePlayerInput();
    }

    public GameObject GetParent()
    {
        return this.gameObject.transform.parent.gameObject;
    }
}
